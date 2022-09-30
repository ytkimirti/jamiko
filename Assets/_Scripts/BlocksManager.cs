using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksManager : MonoBehaviour
{
    public Block testBlock;
    
    private List<Block> _allBlocks = new List<Block>();

    public void AddBlock(Block b) => _allBlocks.Add(b);
    public void RemoveBlock(Block b) => _allBlocks.Remove(b);
    
    private void OnDrawGizmos()
    {
        if (!testBlock)
            return;
        var blocks = GetSameColorBlocks(testBlock);

        foreach (var b in blocks)
        {
            Gizmos.DrawSphere(b.transform.position + Vector3.back * 2, 0.2f);
        }
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
            b.Explode();
            
            // Call the function again since probably a lot of blocks are removed
            // from the list. So it's better to restart iterating the list
            CheckAllBlocksForExplode();
            break;
        }
    }

    public List<Block> GetSameColorBlocks(Block block)
    {
        List<Block> blocks = new List<Block>();
        
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
