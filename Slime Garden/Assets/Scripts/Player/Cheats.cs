using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    private RandomSlime rs;
    private PlayerController pc;

    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.Find("PlayerController").GetComponent<PlayerController>();
        rs = GetComponentInParent<RandomSlime>();
    }

    // Update is called once per frame
    void Update()
    {
        // Give money
        if (Input.GetKeyDown(KeyCode.M))
        {
            pc.Money += 100;
        }
        
        // Spawn a random slime on button press
        if (Input.GetKeyDown(KeyCode.G))
        {
            rs.CreateSlime(Vector3.zero);
        }
    }
}