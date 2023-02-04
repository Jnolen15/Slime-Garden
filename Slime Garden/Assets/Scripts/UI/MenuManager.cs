using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject buildMenu;
    [SerializeField] private GameObject placeableUIPrefab;
    [SerializeField] private GameObject seedMenu;
    [SerializeField] private GameObject seedUIPrefab;
    [SerializeField] private GameObject infoBox;
    [SerializeField] private List<PlaceableObjectSO> availablePlaceables = new List<PlaceableObjectSO>();
    [SerializeField] private List<CropSO> availableSeeds = new List<CropSO>();

    void Start()
    {
        UpdateBuildMenu();
        buildMenu.SetActive(false);
        UpdateSeedMenu();
        seedMenu.SetActive(false);
        infoBox.SetActive(false);
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

        foreach (PlaceableObjectSO so in availablePlaceables)
        {
            var element = Instantiate(placeableUIPrefab, contentMenu);
            element.GetComponent<placeableUIContent>().Setup(so, this);
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

        foreach (CropSO so in availableSeeds)
        {
            var element = Instantiate(seedUIPrefab, contentMenu);
            element.GetComponent<SeedUIContent>().Setup(so, this);
        }
    }

    public void SeedMenuActive(bool value)
    {
        seedMenu.SetActive(value);
    }

    // ============== Info Box ==============
    public void SetInfoBox(string title, string description)
    {
        infoBox.SetActive(true);
        infoBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = title;
        infoBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = description;
    }
    
    public void CloseInfoBox()
    {
        infoBox.SetActive(false);
    }
}
