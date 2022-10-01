using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockKind
{
    None,
    Red,
    Blue,
    Purple,
    Green
}

public class Block : MonoBehaviour
{
    [SerializeField] private LayerMask blocksLayerMask; 
    private BlocksManager _blocksManager;
    public BlockKind kind;
    
    public bool IsHolded { get; private set; }
    [SerializeField] private Vector3Spring _visualPosSpring;
    [SerializeField] private float squashAmountOverVelocity;

    
    
    [Header("Refernces")]
    [SerializeField] private Collider2D col;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform visual;

    
    // public void SetTargetHoldPos(Vector2 pos) => _targetHoldPos = pos;

    public void StartHold()
    {
        col.enabled = false;
        rb.isKinematic = true;
        IsHolded = true;
    }

    public void EndHold()
    {
        col.enabled = true;
        rb.isKinematic = false;
        IsHolded = false;
    }

    private void Start()
    {
        _visualPosSpring.Current = transform.position;
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
            var block = CheckDirForBlock(dir);

            if (!block) continue;
            blocks.Add(block);
        }

        return blocks;
    }

    public Block CheckDirForBlock(Vector2 dir)
    {
        var origin = (Vector2)transform.position + dir;

        var col = Physics2D.OverlapPoint(origin, blocksLayerMask);

        if (!col) return null;

        var block = col.gameObject.GetComponent<Block>();

        Debug.Assert(block != null);
        // Gizmos.DrawLine(transform.position, origin);
        return block;
    }

    public void Explode()
    {
        if (_blocksManager)
            _blocksManager.RemoveBlock(this);
        Destroy(gameObject);
    }

    private void Update()
    {
        _visualPosSpring.UpdateSpring(transform.position);
        visual.position = _visualPosSpring.Current;
        float verticalVel = _visualPosSpring.Velocity.y;

        float squash = 1 + (Mathf.Abs(verticalVel) / squashAmountOverVelocity);
        visual.localScale = new Vector3(1 / squash, squash);
    }

    public void Init(BlocksManager parent)
    {
        _blocksManager = parent;
        _blocksManager.AddBlock(this);
    }
}
