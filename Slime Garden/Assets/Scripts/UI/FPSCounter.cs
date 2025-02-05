﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsDisplay;

    private void Start()
    {
        InvokeRepeating("UpdateFPS", 0.1f, 1f);
    }

    private void UpdateFPS()
    {
        int fps = (int)(1f / Time.unscaledDeltaTime);
        fpsDisplay.text = "" + fps;
    }
}
