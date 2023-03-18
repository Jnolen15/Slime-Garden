using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour, IInteractable
{
    public GameObject contents;

    private RandomSlime rs;

    public enum Type
    {
        Slime,
        Snack,
        Furniture,
    }
    public Type type;

    void Start()
    {
        rs = GameObject.Find("GameManager").GetComponent<RandomSlime>();
    }

    public void Interact()
    {
        if (type == Type.Slime)
        {
            Debug.Log("Generating Slime");
            rs.CreateSlime(transform.position, false);
        }
        Destroy(this.gameObject);
    }
}
