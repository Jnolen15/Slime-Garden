using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    private Manager gm;
    private PlayerController pc;

    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.Find("PlayerController").GetComponent<PlayerController>();
        gm = GetComponentInParent<Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Give money
        if (Input.GetKeyDown(KeyCode.M))
        {
            pc.cs += 100;
        }
        
        // Spawn a random slime on button press
        if (Input.GetKeyDown(KeyCode.G))
        {
            gm.CreateSlime(Vector3.zero);
        }
    }
}