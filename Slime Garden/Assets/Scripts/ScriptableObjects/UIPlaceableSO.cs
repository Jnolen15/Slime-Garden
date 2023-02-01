using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UI Content/Placeable Entry")]
public class UIPlaceableSO : ScriptableObject
{
    public string title;
    public int price;
    public PlaceableObjectSO placeableData;
}
