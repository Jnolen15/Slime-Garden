using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UI Content/Seed Entry")]
public class UISeedSO : ScriptableObject
{
    public string title;
    public int price;
    public CropSO cropData;
}
