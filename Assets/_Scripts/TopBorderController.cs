using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TopBorderController : MonoBehaviour
{
    [SerializeField] private GameplayCameraController camera;

    private void Update()
    {
        if (camera)
            transform.position = Vector3.up * camera.Height;
    }
}
