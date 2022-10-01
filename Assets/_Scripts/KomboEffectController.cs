using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class KomboEffectController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer renderer;

    private void Start()
    {
        transform.DOMoveY(transform.position.y + 1, 0.6f).OnComplete(() => Destroy(gameObject));
        renderer.DOFade(0, 0.6f);
    }
}
