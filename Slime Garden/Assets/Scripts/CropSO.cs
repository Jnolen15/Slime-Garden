using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CropSO : ScriptableObject
{
    public string cropName;
    public float growthTime;
    public List<Sprite> spriteStages = new List<Sprite>();
}
