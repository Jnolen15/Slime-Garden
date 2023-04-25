using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [SerializeField] private GameObject airBalloon;
    [SerializeField] private GameObject cropCart;
    [SerializeField] private GameObject habitatUpgade;

    void Start()
    {
        // ?
    }

    public void UnlockNPC(string npc, bool fromLevelUp)
    {
        if (npc == "wz")
            airBalloon.SetActive(true);
        else if (npc == "cc")
            cropCart.SetActive(true);
        else if (npc == "hu")
            habitatUpgade.SetActive(true);
        else
            Debug.LogError("NPC upgrade not found");
    }
}
