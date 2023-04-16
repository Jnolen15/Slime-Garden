using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwapper : MonoBehaviour
{
    [SerializeField] private List<Material> matList = new List<Material>();
    [SerializeField] private int curMat;
    [SerializeField] private Renderer[] renders;

    public void Swap()
    {
        curMat++;

        if (curMat >= matList.Count)
            curMat = 0;

        foreach(Renderer render in renders)
        {
            render.material = matList[curMat];
        }

        if (this.GetComponent<ParticleSystem>() != null)
            this.GetComponent<ParticleSystem>().Emit(15);
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

        foreach (Renderer render in renders)
        {
            render.material = matList[curMat];
        }
    }
}
