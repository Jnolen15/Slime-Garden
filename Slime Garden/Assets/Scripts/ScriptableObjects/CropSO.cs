using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OtherSOs/Crop")]
public class CropSO : ScriptableObject
{
    public string cropName;
    public int[] growTicks;
    public Sprite[] spriteStages;
}
