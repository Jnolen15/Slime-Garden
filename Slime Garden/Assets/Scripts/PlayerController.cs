using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public int cs = 0;  // The ammount of Congealed Slime held by the player

    public TextMeshProUGUI csDisplay;

    void Update()
    {
        csDisplay.text = "Crystalized Slime: " + cs;
    }
}
