using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class SlimeInfoPannel : MonoBehaviour
{
    [SerializeField] private InventoryManager invManager;
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private GameObject feedMenu;
    [SerializeField] private GameObject cropUIPrefab;
    [SerializeField] private GameObject nameTextObj;
    [SerializeField] private GameObject nameInputObj;
    [SerializeField] private TMP_InputField inputText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI speciesText;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private SlimeData curSlimeData;
    [SerializeField] private SlimeController curSlimeControl;

    private void Start()
    {
        UpdateCropMenu();
    }

    public void Setup(GameObject slime)
    {
        curSlimeData = slime.GetComponent<SlimeData>();
        curSlimeControl = slime.GetComponent<SlimeController>();
        nameTextObj.SetActive(true);
        nameInputObj.SetActive(false);
        nameText.text = curSlimeData.displayName;
        inputText.text = "New Name";
        speciesText.text = curSlimeData.sBaseColor + " " + curSlimeData.sPatternColor + " " + curSlimeData.slimeSpeciesPattern.sPattern;
        rarityText.text = "Rarity: " + curSlimeData.sRarity;

        UpdateCropCount();
    }

    public void OpenRename()
    {
        nameTextObj.SetActive(false);
        nameInputObj.SetActive(true);
    }

    public void SetRename(string newName)
    {
        curSlimeData.SetName(newName);

        nameText.text = curSlimeData.displayName;
        nameTextObj.SetActive(true);
        nameInputObj.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }

    // ============== Crop Menus ==============
    public void UpdateCropMenu()
    {
        var contentMenu = feedMenu.transform.GetChild(0);
        foreach (InventoryManager.CropInventroyEntry crop in invManager.inventoryList)
        {
            var element = Instantiate(cropUIPrefab, contentMenu);
            element.GetComponent<CropUIContent>().Setup(crop, menuManager);
        }

        feedMenu.GetComponent<ContentMenu>().UpdateContent();
    }

    public void UpdateCropCount()
    {
        Debug.Log("Updating crop count");

        var contentMenu = feedMenu.transform.GetChild(1);
        foreach (Transform child in contentMenu)
        {
            child.GetComponent<CropUIContent>().UpdateValues();
        }
    }

    public void FeedSlime(CropSO crop)
    {
        if (0 < invManager.GetNumHeld(crop))
        {
            // Create Crop
            var slimePos = curSlimeData.transform.position;
            Vector3 spawnPos = new Vector3(slimePos.x - 1.5f, slimePos.y, slimePos.z - 3);
            var instCrop = Instantiate(crop.cropObj, spawnPos, Quaternion.identity);
            var cropObj = instCrop.GetComponent<CropObj>();
            cropObj.Setup(crop);

            // Set slime state
            bool wasFed = curSlimeControl.FeedSlime(cropObj);

            // If feeding was successful
            if (wasFed)
            {
                // Animate Crop
                Vector3 destination = new Vector3(slimePos.x, slimePos.y, slimePos.z - 0.5f);
                instCrop.transform.DOJump(destination, 1f, 1, 0.6f).SetEase(Ease.Flash);

                // Take from inventory
                Debug.Log("Fed " + crop.cropName);
                invManager.AddCrop(crop, -1);
                UpdateCropCount();
            } else
            {
                Debug.Log("Feeding failed");
                Destroy(cropObj.gameObject);
            }
        }
    }


    // ============== Release Slime ==============
    public void Release()
    {
        curSlimeControl.ReleaseSlime();
    }
}
