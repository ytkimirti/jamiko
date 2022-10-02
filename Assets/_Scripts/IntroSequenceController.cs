using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroSequenceController : MonoBehaviour
{
    [SerializeField] private Sprite[] stageSprites;
    [SerializeField] private Image stageImage;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Image goImage;
    [SerializeField] private Sprite gameOverSprite;
    [SerializeField] private Sprite gameWonSprite;

    

    

    
    
    void Start()
    {
        stageImage.sprite = stageSprites[Mathf.Min(stageSprites.Length, GameManager.Instance.CurrentStage - 1)];
    }

    public void Play()
    {
        anim.SetTrigger("Start");
        Invoke("PlayAudio", 3);
    }

    private void PlayAudio()
    {
        audioSource.Play();
    }

    public void OnGameOver(bool didWin)
    {
        goImage.sprite = didWin ? gameWonSprite : gameOverSprite;
        anim.SetTrigger("End");
    }
    
}
