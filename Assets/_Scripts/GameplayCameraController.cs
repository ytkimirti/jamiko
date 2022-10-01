using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GameplayCameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float targetWidth;
    [SerializeField] private float topOffset;
    [SerializeField] private float bottomOffset;
    
    public float Height => (1 / cam.aspect) * targetWidth;
    public float TopPosition => Height - topOffset;
    public float BottomPosition => -Height + bottomOffset;

    void Update()
    {
        cam.orthographicSize = Height;
    }
}
