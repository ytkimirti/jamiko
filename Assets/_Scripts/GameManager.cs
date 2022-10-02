using System;
using System.Collections;
using OFK;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool DisableDebugMode = false;
    public bool GameOver { get; private set; } = false;
    public bool GameStarted { get; private set; } = false;
    public static GameManager Instance;
    [SerializeField] private BlocksManager blocksManager;
    [SerializeField] private IntroSequenceController sequenceController;
    [SerializeField] private SoundData winSound;
    [SerializeField] private SoundData looseSound;
    [SerializeField] private int maxScore;
    [SerializeField] private int minScore;
    [SerializeField] private int scoreChangePerStage;
    [SerializeField] private Transform progressBarTrans;
    
    private int CurrentMaxScore => Mathf.Min(maxScore, minScore + scoreChangePerStage * CurrentStage);
    
    public int Score { get; private set; }

    public void IncreaseScore(int amount)
    {
        Score += amount;
        Score = Mathf.Clamp(Score, 0, CurrentMaxScore);
        if (Score >= CurrentMaxScore)
            OnGameOver(true);
        progressBarTrans.localScale = new Vector3(Score / (float)CurrentMaxScore, 1, 1);
    }
    
    public int CurrentStage
    {
        get
        {
            if (_currentStage == -1)
                _currentStage = PlayerPrefs.GetInt("currentLevel", 1);
            return _currentStage;
        }
        set
        {
            PlayerPrefs.SetInt("currentLevel", value);
            _currentStage = value;
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
        if (Application.isEditor && !DisableDebugMode)
            StartGame();
        else
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

    private void Update()
    {
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.W))
                OnGameOver(true);
            if (Input.GetKeyDown(KeyCode.L))
                OnGameOver(false);
        }
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
        if (didWin)
            winSound.Play();
        else
            looseSound.Play();
        GameOver = true;
        GameStarted = false;
        blocksManager.ExplodeAllBlocks();
        if (!didWin)
            CurrentStage = 1;
        else
            CurrentStage = CurrentStage + 1;
        StartCoroutine(GameOverEnum());
    }

    IEnumerator GameOverEnum()
    {
        yield return new WaitForSeconds(4);
        RestartLevel();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}