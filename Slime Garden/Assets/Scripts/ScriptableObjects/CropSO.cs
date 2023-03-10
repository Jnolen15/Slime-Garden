using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OtherSOs/Crop")]
public class CropSO : ScriptableObject
{
    public string cropName;
    public Sprite previewImage;
    public int price;
    public int sellValue;

    [TextArea]
    public string seedDescription;
    [TextArea]
    public string CropDescription;

    public int[] growTicks;
    public Mesh[] cropStages;
    public Material cropMat;

    public GameObject cropObj;
}
