using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OtherSOs/Crop")]
public class CropSO : ScriptableObject
{
    [Header("Basic Info")]
    public string cropName;
    public Sprite previewImage;
    public int price;
    public int sellValue;
    public int expYeild;
    public int multiYeild;
    public int regrowTicks;
    public bool multiHarvest;

    [TextArea]
    public string seedDescription;
    [TextArea]
    public string CropDescription;

    [Header("Growth Data")]
    public int[] growTicks;
    public Mesh[] cropStages;
    public Material cropMat;
    public Material cropWiltedMat;

    [Header("Crop Object")]
    public GameObject cropObj;

    [Header("Nutritional Value")]
    public int tameValue;
}
