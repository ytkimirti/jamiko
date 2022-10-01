using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BordersController : MonoBehaviour
{
    [SerializeField] private GameplayCameraController camera;
    [SerializeField] private Transform topBorder;
    [SerializeField] private Transform bottomBorder;
    
    private void Update()
    {
        if (!camera) return;
            
        topBorder.position = Vector3.up * camera.TopPosition;
        bottomBorder.position = Vector3.up * -camera.BottomPosition;
    }
}
