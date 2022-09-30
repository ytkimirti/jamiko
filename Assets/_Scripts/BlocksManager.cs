using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlocksManager : MonoBehaviour
{
    public Block testBlock;
    
    private List<Block> _allBlocks = new List<Block>();
    [SerializeField] private GameplayCameraController cam;
    [SerializeField] private LayerMask blocksLayer;

    

    public void AddBlock(Block b) => _allBlocks.Add(b);
    public void RemoveBlock(Block b) => _allBlocks.Remove(b);
    
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
        Vector3 origin = new Vector3(horizontalPos, -cam.Height);
        var hit = Physics2D.Raycast(origin, Vector2.up, float.PositiveInfinity, blocksLayer);

        Vector3 placePos;

        if (!hit)
            placePos = new Vector3(horizontalPos, cam.Height - 0.5f);
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
        
        CheckBlockForExplode(blocks[0]);
    }

    public List<Block> GetVerticalBlocksFromBottom(int horizontalPos, int maxCount)
    {
        var list = new List<Block>();

        Vector3 origin = new Vector3(horizontalPos, -cam.Height);
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
