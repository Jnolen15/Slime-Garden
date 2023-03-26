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
    [SerializeField] private GameObject cropMenu;
    [SerializeField] private GameObject cropUIPrefab;
    [SerializeField] private GameObject infoBox;
    [SerializeField] private GameObject slimeInfoBox;
    [SerializeField] private InventoryManager invManager;

    void Start()
    {
        UpdateBuildMenu();
        buildMenu.SetActive(false);
        UpdateSeedMenu();
        seedMenu.SetActive(false);
        UpdateCropMenu();
        cropMenu.SetActive(false);
        infoBox.SetActive(false);
        slimeInfoBox.SetActive(false);
    }

    public void CloseAllSubmenus()
    {
        buildMenu.SetActive(false);
        seedMenu.SetActive(false);
        cropMenu.SetActive(false);
    }

    // ============== Build Menus ==============
    public void UpdateBuildMenu()
    {
        var contentMenu = buildMenu.transform.GetChild(0);

        foreach (PlaceableObjectSO so in invManager.availablePlaceables)
        {
            var element = Instantiate(placeableUIPrefab, contentMenu);
            element.GetComponent<placeableUIContent>().Setup(so, this);
        }

        buildMenu.GetComponent<ContentMenu>().UpdateContent();
    }

    public void BuildMenuActive(bool value)
    {
        buildMenu.SetActive(value);
    }

    // ============== Seed Menus ==============
    public void UpdateSeedMenu()
    {
        var contentMenu = seedMenu.transform.GetChild(0);

        foreach (CropSO so in invManager.availableCrops)
        {
            var element = Instantiate(seedUIPrefab, contentMenu);
            element.GetComponent<SeedUIContent>().Setup(so, this);
        }

        seedMenu.GetComponent<ContentMenu>().UpdateContent();
    }

    public void SeedMenuActive(bool value)
    {
        seedMenu.SetActive(value);
    }

    // ============== Crop Menus ==============
    public void UpdateCropMenu()
    {
        var contentMenu = cropMenu.transform.GetChild(0);
        foreach (Transform child in contentMenu)
        {
            Destroy(child.gameObject);
        }

        foreach (InventoryManager.CropInventroyEntry crop in invManager.inventoryList)
        {
            var element = Instantiate(cropUIPrefab, contentMenu);
            element.GetComponent<CropUIContent>().Setup(crop, this);
        }

        cropMenu.GetComponent<ContentMenu>().UpdateContent();
    }

    public void UpdateCropCount()
    {
        var contentMenu = cropMenu.transform.GetChild(0);
        foreach (Transform child in contentMenu)
        {
            child.GetComponent<CropUIContent>().UpdateValues();
        }
    }

    public void CropMenuActive(bool value)
    {
        if (value)
            UpdateCropCount();

        cropMenu.SetActive(value);
    }

    // ============== Info Box ==============
    public void SetInfoBox(string title, int price, string description)
    {
        StopAllCoroutines();
        infoBox.SetActive(true);
        infoBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = title;
        infoBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = price.ToString();
        infoBox.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = description;
    }
    
    public void CloseInfoBox()
    {
        StopAllCoroutines();
        StartCoroutine(WaitCloseInfoBox());
    }

    IEnumerator WaitCloseInfoBox()
    {
        yield return new WaitForSeconds(0.4f);
        infoBox.SetActive(false);
    }

    // ============== Slime Inspect ==============
    public void ShowSlimeStats(SlimeData slime)
    {
        slimeInfoBox.SetActive(true);
        slimeInfoBox.GetComponent<SlimeInfoPannel>().Setup(slime);
    }

    public void CloseSlimeStats()
    {
        slimeInfoBox.SetActive(false);
    }
}
