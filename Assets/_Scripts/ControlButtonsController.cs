using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlButtonsController : MonoBehaviour
{
    [SerializeField] private GameObject mobileControls;

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            mobileControls.SetActive(true);
        }
    }
}
