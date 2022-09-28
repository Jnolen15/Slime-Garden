using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int cs = 0;  // The ammount of Congealed Slime held by the player

    public Text csDisplay;

    void Start()
    {
        
    }

    void Update()
    {
        csDisplay.text = "Monies: " + cs;
    }
}
