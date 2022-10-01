using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private BlocksManager blocksManager;

    

    private void Awake()
    {
        Application.targetFrameRate = 140;
        Instance = this;
    }

    public void GameOver(bool didWin)
    {
        blocksManager.ExplodeAllBlocks();
    }
}