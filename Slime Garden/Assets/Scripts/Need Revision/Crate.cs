using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public GameObject contents;

    private Manager gm;

    public enum Type
    {
        Slime,
        Snack,
        Furniture,
    }
    public Type type;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<Manager>();
    }

    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if(type == Type.Slime)
        {
            Debug.Log("Generating Slime");
            gm.CreateSlime(transform.position);
        }
        Destroy(this.gameObject);
    }
}
