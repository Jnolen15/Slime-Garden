using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    private RandomSlime rs;
    private PlayerData pData;

    // Start is called before the first frame update
    void Start()
    {
        pData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
        rs = GetComponentInParent<RandomSlime>();
    }

    // Update is called once per frame
    void Update()
    {
        // Give money
        if (Input.GetKeyDown(KeyCode.M))
        {
            pData.GainMoney(100);
        }
        
        // Spawn a random slime on button press
        if (Input.GetKeyDown(KeyCode.G))
        {
            rs.CreateSlime(Vector3.zero);
        }
    }
}