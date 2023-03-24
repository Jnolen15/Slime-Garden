using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterial : MonoBehaviour
{
    [SerializeField]
    private List<Material> matList = new List<Material>();

    public void AssignRandomMat()
    {
        var renderer = this.GetComponent<Renderer>();
        renderer.material = matList[Random.Range(0, matList.Count)];
    }
}
