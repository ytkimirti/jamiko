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
    public bool IsDed { get; private set; }
    [SerializeField] private LayerMask blocksLayerMask; 
    private BlocksManager _blocksManager;
    public BlockKind kind;
    
    public bool IsHolded { get; private set; }
    [SerializeField] private Vector3Spring _visualPosSpring;
    [SerializeField] private Vector3Spring _visualScaleSpring;
    [SerializeField] private float squashAmountOverVelocity;
    [SerializeField] private int explodingLayer;
    
    [Header("References")]
    [SerializeField] private Collider2D col;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform visual;
    [SerializeField] private SpriteFlasher spriteFlasher;
    [SerializeField] private AudioSource explodeAudioSource;
    [SerializeField] private AudioClip komboSoundClip;
    [SerializeField] private GameObject komboPrefab;

    
    
    
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
        rb.velocity = Vector2.zero;
    }

    private void Start()
    {
        _visualPosSpring.Current = transform.position;
        _visualScaleSpring.Current = visual.localScale;
    }

    public void Highlight()
    {
        // spriteFlasher.Flash();
        _visualScaleSpring.Velocity = Vector3.one * -5f;
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

    // Being host means am I the one that got thrown in the end.
    // It's used to play sounds etc.
    public void Explode(bool isCombo, bool isHost)
    {
        if (isHost)
        {
            if (isCombo)
                explodeAudioSource.clip = komboSoundClip;
            explodeAudioSource.Play();
        }

        if (isCombo)
            Instantiate(komboPrefab, transform.position, Quaternion.identity);
        GameManager.Instance.IncreaseScore(isCombo ? 200 : 100);
        Explode();
    }

    public void Explode()
    {
        if (IsDed)
            return;
        if (_blocksManager)
            _blocksManager.RemoveBlock(this);
        IsDed = true;
        gameObject.layer = explodingLayer;
        StartCoroutine(DieEnum());
    }

    IEnumerator DieEnum()
    {
        while (transform.localScale.x > 0.03f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 5 * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    private void Update()
    {
        _visualPosSpring.UpdateSpring(transform.position);
        visual.position = _visualPosSpring.Current;
        float verticalVel = _visualPosSpring.Velocity.y;

        float squash = 1 + (Mathf.Abs(verticalVel) / squashAmountOverVelocity);
        var squashScaleTarget = new Vector3(1 / squash, squash);
        var springScale = _visualScaleSpring.UpdateSpring(Vector3.one);
        visual.localScale = new Vector3(squashScaleTarget.x * springScale.x, squashScaleTarget.y * springScale.y, 1);
    }

    public void Init(BlocksManager parent)
    {
        _blocksManager = parent;
        _blocksManager.AddBlock(this);
    }
}
