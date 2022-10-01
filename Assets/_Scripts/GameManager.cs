using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool GameOver { get; private set; } = false;
    public bool GameStarted { get; private set; } = false;
    public static GameManager Instance;
    [SerializeField] private BlocksManager blocksManager;
    [SerializeField] private IntroSequenceController sequenceController;
    
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
        for (int i = 0; i < 3; i++)
        {
            blocksManager.SpawnRandomRow();
            yield return new WaitForSeconds(0.35f);
        }
        sequenceController.Play();
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

    public void OnGameOver(bool didWin)
    {
        if (GameOver)
            return;
        GameOver = true;
        GameStarted = false;
        blocksManager.ExplodeAllBlocks();
    }
}