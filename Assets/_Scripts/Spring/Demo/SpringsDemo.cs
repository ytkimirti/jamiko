using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpringsDemo : MonoBehaviour
{
    public Vector3Spring followSpring;
    public AngleSpring mouseLookSpring;
    public float targetAngle;

    private void Start()
    {
        followSpring.Current = transform.position;
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0;
        // float targetAngle = Vector2.SignedAngle(transform.position, mousePos);
        // transform.position = followSpring.UpdateSpring(mousePos);
        transform.localEulerAngles = Vector3.forward * targetAngle;
    }
}
