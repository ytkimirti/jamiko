using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GameplayCameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float targetWidth;
    
    public float Height => (1 / cam.aspect) * targetWidth;

    void Update()
    {
        cam.orthographicSize = Height;
    }
}
