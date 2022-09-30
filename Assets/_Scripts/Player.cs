using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;



public class Player : MonoBehaviour
{
    [SerializeField] private int maxBlocksToHold;

    
    private int _horizontalPosition = 0;

    public void MoveRight()
    {
        if (_horizontalPosition >= 2)
            return;
        _horizontalPosition++;
    }

    public void MoveLeft()
    {
        if (_horizontalPosition <= -2)
            return;
        _horizontalPosition--;
    }

    [SerializeField] private FloatSpring horizontalSpring;

    [SerializeField] private Transform visual;
    [SerializeField] private float visualRotateAmount;
    [SerializeField] private BlocksManager blocksManager;
    [SerializeField] private List<HoldPosition> blockHoldTransforms = new List<HoldPosition>();

    [Serializable]
    class HoldPosition
    {
        public List<Transform> positions = new List<Transform>();
    }
    
    private List<Block> _holdedBlocks = new List<Block>();

    public BlockKind HoldedKind => _holdedBlocks.Count > 0 ? _holdedBlocks[0].kind : BlockKind.None;

    public void TakeBlock()
    {
        if (_holdedBlocks.Count == maxBlocksToHold)
            return;
        var blocksToTake = blocksManager.GetVerticalBlocksFromBottom(_horizontalPosition, 1);

        if (blocksToTake.Count == 0)
            return;

        if (_holdedBlocks.Count != 0)
        {
            foreach (var b in blocksToTake)
            {
                if (b.kind != HoldedKind)
                    return;
            }
        }
        
        foreach (var block in blocksToTake)
        {
            block.StartHold();
        }
        
        _holdedBlocks.AddRange(blocksToTake);
    }

    private void UpdateHoldedBlocks()
    {
        for (var i = 0; i < _holdedBlocks.Count; i++)
        {
            _holdedBlocks[i].transform.position =
                blockHoldTransforms[_holdedBlocks.Count - 1].positions[i].transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        var blocks = blocksManager.GetVerticalBlocksFromBottom(_horizontalPosition, maxBlocksToHold - _holdedBlocks.Count);

        Gizmos.color = Color.red;
        foreach (var b in blocks)
        {
            Gizmos.DrawWireSphere(b.transform.position + Vector3.back * 3, 0.3f);
        }
    }

    public void ThrowBlock()
    {
        blocksManager.PlaceBlocks(_holdedBlocks, _horizontalPosition);
        _holdedBlocks.Clear();
    }
    
    public void ActionButton()
    {
        Debug.Log("Action button pressed");
    }

    private void Update()
    {
        horizontalSpring.UpdateSpring(_horizontalPosition);
        transform.localPosition = new Vector3(horizontalSpring.Current, transform.localPosition.y, 0);
        visual.localEulerAngles = Vector3.forward * (horizontalSpring.Velocity * visualRotateAmount);

        UpdateInput();
        UpdateHoldedBlocks();
    }

    private void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            MoveRight();
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            MoveLeft();
        if (Input.GetKeyDown(KeyCode.UpArrow))
            ThrowBlock();
        if (Input.GetKeyDown(KeyCode.DownArrow))
            TakeBlock();
        if (Input.GetKeyDown(KeyCode.Space))
            ActionButton();
    }
}
