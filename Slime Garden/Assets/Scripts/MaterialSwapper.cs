using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwapper : MonoBehaviour
{
    [SerializeField] private List<Material> matList = new List<Material>();
    [SerializeField] private int curMat;
    private Renderer render;

    void Awake()
    {
        render = GetComponentInChildren<Renderer>();
    }

    public void Swap()
    {
        curMat++;

        if (curMat >= matList.Count)
            curMat = 0;

        render.material = matList[curMat];
    }

    public int GetMatIndex()
    {
        return curMat;
    }

    public void SetMat(int setTo)
    {
        curMat = setTo;

        if (curMat >= matList.Count)
            curMat = 0;

        render.material = matList[curMat];
    }
}
