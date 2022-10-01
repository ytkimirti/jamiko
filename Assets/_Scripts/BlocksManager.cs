using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlocksManager : MonoBehaviour
{
    public Block testBlock;
    
    private List<Block> _allBlocks = new List<Block>();
    [SerializeField] private GameplayCameraController cam;
    [SerializeField] private LayerMask blocksLayer;
    [SerializeField] private BlockType[] blockPrefabs;
    
    [Header("Auto Spawning")]
    [SerializeField] private float maxSpawnTime;
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float spawnTimeChangePerLevel;
    private float _spawnTimer;

    private float SpawnTime => Mathf.Max(minSpawnTime, maxSpawnTime - spawnTimeChangePerLevel * GameManager.Instance.CurrentStage);

    [Serializable]
    private class BlockType
    {
        public BlockKind kind;
        public GameObject prefab;
    }

    private void Start()
    {
        ResetSpawnTimer();
    }

    private GameObject GetRandomBlockPrefab()
    {
        return blockPrefabs[Random.Range(0, blockPrefabs.Length)].prefab;
    }

    public void SpawnRandomRow()
    {
        MoveAllBlocks(-1);
        for (int i = 0; i < 5; i++)
        {
            float xPos = (i - 2) * 1;
            var pos = new Vector3(xPos, cam.TopPosition, 0);
            Block block = Instantiate(GetRandomBlockPrefab(), pos, Quaternion.identity, transform).GetComponent<Block>();
            block.Init(this);
        }
        CheckForGameOver();
    }

    public void MoveAllBlocks(float amount)
    {
        foreach (var block in _allBlocks)
        {
            block.transform.Translate(0, amount, 0);
        }
    }

    public void CheckForGameOver()
    {
        float minHeight = float.PositiveInfinity;
        
        foreach (var b in _allBlocks)
        {
            if (b.transform.position.y < minHeight)
                minHeight = b.transform.position.y;
        }

        if (minHeight < cam.BottomPosition - 0.5f)
        {
            GameManager.Instance.OnGameOver(false);
        }

    }

    public void ExplodeAllBlocks()
    {
        StartCoroutine(ExplodeAllBlocksEnum());
    }

    IEnumerator ExplodeAllBlocksEnum()
    {
        var copyBlocks = new List<Block>(_allBlocks);
        
        copyBlocks.Sort((x, y) =>
        {
            int rx = Mathf.RoundToInt(x.transform.position.x);
            int ry = Mathf.RoundToInt(x.transform.position.y);
            int lx = Mathf.RoundToInt(y.transform.position.x);
            int ly = Mathf.RoundToInt(y.transform.position.y);
            return (ry - ly) * 1000 + (rx - lx);
        });
        foreach (var b in copyBlocks)
        {
            b.Explode();
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void AddBlock(Block b) => _allBlocks.Add(b);
    public void RemoveBlock(Block b) => _allBlocks.Remove(b);

    public void FlashBlocks(BlockKind kind)
    {
        foreach (var b in _allBlocks)
        {
            if (b.kind == kind)
                b.Highlight();
        }
    }
    
    private void OnDrawGizmos()
    {
        if (!testBlock)
            return;
        var blocks = GetSameColorBlocks(testBlock);

        foreach (var b in blocks)
        {
            Gizmos.DrawWireSphere(b.transform.position + Vector3.back * 2, 0.2f);
        }
    }

    public void CheckBlockForExplode(Block b)
    {
        Physics2D.SyncTransforms();
        var list = GetSameColorBlocks(b);

        if (list.Count < 3) return;
        
        foreach (var neighbour in list)
        {
            neighbour.Explode();
        }
        b.Explode();
    }

    public void CheckAllBlocksForExplode()
    {
        foreach (var b in _allBlocks)
        {
            var list = GetSameColorBlocks(b);

            if (list.Count < 3) continue;
            
            foreach (var neighbour in list)
            {
                neighbour.Explode();
            }
            
            // Call the function again since probably a lot of blocks are removed
            // from the list. So it's better to restart iterating the list
            CheckAllBlocksForExplode();
            break;
        }
    }

    public void PlaceBlocks(List<Block> blocks, int horizontalPos)
    {
        if (blocks.Count == 0)
            return;
        Vector3 origin = new Vector3(horizontalPos, cam.BottomPosition);
        var hit = Physics2D.Raycast(origin, Vector2.up, float.PositiveInfinity, blocksLayer);

        Vector3 placePos;

        if (!hit)
            placePos = new Vector3(horizontalPos, cam.TopPosition - 0.5f);
        else
        {
            Block bottomBlock = hit.collider.gameObject.GetComponent<Block>();
            
            Debug.Assert(bottomBlock != null);

            placePos = bottomBlock.transform.position + Vector3.down * 0.5f;
        }

        for (int i = blocks.Count - 1; i >= 0; i--)
        {
            blocks[i].transform.position = placePos + Vector3.down * i;
            blocks[i].EndHold();
        }
        
        // CheckBlockForExplode(blocks[0]);
        StartCoroutine(CheckExplodeEnum(blocks[0]));
    }

    IEnumerator CheckExplodeEnum(Block block)
    {
        yield return new WaitForSeconds(0.1f);
        
        if (block && !block.IsDed && !block.IsHolded)
            CheckBlockForExplode(block);
    }

    private void ResetSpawnTimer() => _spawnTimer = SpawnTime;
    
    private void Update()
    {
        if (!GameManager.Instance.GameStarted)
            return;
        if (Input.GetKeyDown(KeyCode.A) || _spawnTimer <= 0)
        {
            SpawnRandomRow();
            ResetSpawnTimer();
        }

        _spawnTimer -= Time.deltaTime;
    }

    public List<Block> GetVerticalBlocksFromBottom(int horizontalPos, int maxCount)
    {
        var list = new List<Block>();

        Vector3 origin = new Vector3(horizontalPos, cam.BottomPosition);
        var hit = Physics2D.Raycast(origin, Vector2.up, float.PositiveInfinity, blocksLayer);

        if (!hit)
            return list;

        Block b = hit.collider.gameObject.GetComponent<Block>();
        
        Debug.Assert(b != null);

        while (b != null && list.Count < maxCount)
        {
            list.Add(b);
            
            Block newBlock = b.CheckDirForBlock(Vector3.up);

            if (!newBlock || b.kind != newBlock.kind)
                break;
            b = newBlock;
        }

        return list;
    }

    public List<Block> GetSameColorBlocks(Block block)
    {
        List<Block> blocks = new List<Block>();

        blocks.Add(block);
        CheckExplodeRecursive(block, blocks);

        return blocks;
    }
    private void CheckExplodeRecursive(Block block, List<Block> checkedBlocks)
    {
        var neighbours = block.FindNeighbours();
        
        foreach (var b in neighbours)
        {
            if (b.kind != block.kind)
                continue;
            if (checkedBlocks.Contains(b))
                continue;
            checkedBlocks.Add(b);
            CheckExplodeRecursive(b, checkedBlocks);
        }
    }
}

