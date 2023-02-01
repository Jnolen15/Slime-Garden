using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject buildMenu;
    [SerializeField] private GameObject placeableUIPrefab;
    [SerializeField] private GameObject seedMenu;
    [SerializeField] private GameObject seedUIPrefab;
    [SerializeField] private List<UIPlaceableSO> availablePlaceables = new List<UIPlaceableSO>();
    [SerializeField] private List<UISeedSO> availableSeeds = new List<UISeedSO>();

    void Start()
    {
        UpdateBuildMenu();
        buildMenu.SetActive(false);
        UpdateSeedMenu();
        seedMenu.SetActive(false);
    }

    public void CloseAllSubmenus()
    {
        buildMenu.SetActive(false);
        seedMenu.SetActive(false);
    }

    // ============== Build Menus ==============
    public void UpdateBuildMenu()
    {
        var contentMenu = buildMenu.transform.GetChild(0).GetChild(0);

        foreach (UIPlaceableSO so in availablePlaceables)
        {
            var element = Instantiate(placeableUIPrefab, contentMenu);
            element.GetComponent<placeableUIContent>().Setup(so);
        }
    }

    public void BuildMenuActive(bool value)
    {
        buildMenu.SetActive(value);
    }

    // ============== Seed Menus ==============
    public void UpdateSeedMenu()
    {
        var contentMenu = seedMenu.transform.GetChild(0).GetChild(0);

        foreach (UISeedSO so in availableSeeds)
        {
            var element = Instantiate(seedUIPrefab, contentMenu);
            element.GetComponent<SeedUIContent>().Setup(so);
        }
    }

    public void SeedMenuActive(bool value)
    {
        seedMenu.SetActive(value);
    }
}
