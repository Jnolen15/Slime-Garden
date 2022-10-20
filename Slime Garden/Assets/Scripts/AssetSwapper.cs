using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetSwapper : MonoBehaviour
{
    [SerializeField] GameObject myLowPolly;
    [SerializeField] GameObject myBlocky;
    [SerializeField] GameObject syntyLowPolly;

    private int num;

    public void CycleDecor()
    {
        num++;
        if(num == 0)
        {
            myLowPolly.SetActive(true);
            myBlocky.SetActive(false);
            syntyLowPolly.SetActive(false);
        }
        else if (num == 1)
        {
            myLowPolly.SetActive(false);
            myBlocky.SetActive(true);
            syntyLowPolly.SetActive(false);
        }
        else if (num == 2)
        {
            myLowPolly.SetActive(false);
            myBlocky.SetActive(false);
            syntyLowPolly.SetActive(true);
        }
        else
        {
            num = 0;
            myLowPolly.SetActive(true);
            myBlocky.SetActive(false);
            syntyLowPolly.SetActive(false);
        }
    }
}
