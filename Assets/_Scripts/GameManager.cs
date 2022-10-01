using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool GameStarted { get; private set; } = false;
    public static GameManager Instance;
    [SerializeField] private BlocksManager blocksManager;
    [SerializeField] private Animator startSequenceAnimator;
    
    public int CurrentStage
    {
        get
        {
            if (_currentStage == -1)
                _currentStage = PlayerPrefs.GetInt("currentLevel", 1);
            return _currentStage;
        }
    }

    private int _currentStage = -1;

    private void Awake()
    {
        Application.targetFrameRate = 140;
        Instance = this;
    }

    private void Start()
    {
        StartGameSequence();
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
        StartGame();
    }

    private void StartGameSequence()
    {
        StartCoroutine(StartSequenceEnum());
    }

    private void StartGame()
    {
        if (GameStarted)
            return;
        GameStarted = true;
    }

    public void GameOver(bool didWin)
    {
        blocksManager.ExplodeAllBlocks();
    }
}