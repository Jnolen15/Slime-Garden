using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OtherSOs/Crop")]
public class CropSO : ScriptableObject
{
    public string cropName;
    public int price;

    [TextArea]
    public string description;

    public int[] growTicks;
    public Sprite[] spriteStages;
}
