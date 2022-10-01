using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool GameStarted { get; private set; } = false;
    public static GameManager Instance;
    [SerializeField] private BlocksManager blocksManager;
    [SerializeField] private Animator startSequenceAnimator;

    private void Awake()
    {
        Application.targetFrameRate = 140;
        Instance = this;
    }

    private void Start()
    {
        
    }

    IEnumerator StartSequenceEnum()
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < 3; i++)
        {
            blocksManager.SpawnRandomRow();
            yield return new WaitForSeconds(0.1f);
        }
        startSequenceAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(5f);
    }

    private void StartGame()
    {
        GameStarted = true;
    }

    public void GameOver(bool didWin)
    {
        blocksManager.ExplodeAllBlocks();
    }
}