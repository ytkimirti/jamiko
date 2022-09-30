using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockKind
{
    Red,
    Blue,
    Yellow,
    Green
}

public class Block : MonoBehaviour
{
    [SerializeField] private LayerMask blocksLayerMask;
    private BlocksManager _blocksManager;
    public BlockKind kind;

    private void Start()
    {
        _blocksManager.AddBlock(this);
    }


    private void OnDrawGizmos()
    {
        FindNeighbours();
    }

    public List<Block> FindNeighbours()
    {
        List<Block> blocks = new List<Block>();
        
        Vector2[] directions =
        {
            new ( 1, 0),
            new (0, 1),
            new (-1, 0),
            new (0, -1),
        };
        foreach (Vector2 dir in directions)
        {
            var origin = (Vector2)transform.position + dir;

            var col = Physics2D.OverlapCircle(origin, 0.1f, blocksLayerMask);

            if (!col) continue;
            
            var block = col.gameObject.GetComponent<Block>();
                
            Debug.Assert(block != null);
                
            blocks.Add(block);
            Gizmos.DrawLine(transform.position, origin);
        }

        return blocks;
    }

    public void Explode()
    {
        _blocksManager.RemoveBlock(this);
        Destroy(gameObject);
    }
}
