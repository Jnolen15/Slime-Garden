using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject buildMenu;
    [SerializeField] private GameObject placeableUIPrefab;
    [SerializeField] private List<UIPlaceableSO> availablePlaceables = new List<UIPlaceableSO>();

    void Start()
    {
        UpdateBuildMenu();
    }

    public void UpdateBuildMenu()
    {
        var contentMenu = buildMenu.transform.GetChild(0).GetChild(0);

        foreach (UIPlaceableSO so in availablePlaceables)
        {
            var element = Instantiate(placeableUIPrefab, contentMenu);
            element.GetComponent<placeableUIContent>().Setup(so);
        }
    }
}
