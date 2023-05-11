using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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
    [SerializeField] private Image hungerBarFill;
    [SerializeField] private SlimeData curSlimeData;
    [SerializeField] private SlimeController curSlimeControl;

    private int prevHungerLevel = 0;

    private void Start()
    {
        UpdateCropMenu();
    }

    private void Update()
    {
        // Update hunger
        if (!hungerBarFill.IsActive())
            return;

        if(curSlimeData.hungerLevel != prevHungerLevel)
        {
            prevHungerLevel = curSlimeData.hungerLevel;
            SetHungerBar();
        }
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

        prevHungerLevel = 0;
        SetHungerBar();

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

    // ============== Crop / Feeding ==============
    public void UpdateCropMenu()
    {
        Debug.Log("UPDATING SLIME FEED MENU");
        feedMenu.GetComponent<ContentMenu>().UpdateContent("crop", cropUIPrefab, invManager, menuManager);
    }

    public void UpdateCropCount()
    {
        Debug.Log("Updating crop count");

        // Updaate hidden
        var contentMenu = feedMenu.transform.GetChild(0);
        foreach (Transform child in contentMenu)
        {
            child.GetComponent<CropUIContent>().UpdateValues();
        }

        // update shown
        contentMenu = feedMenu.transform.GetChild(1);
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

    private void SetHungerBar()
    {
        //Debug.Log($" Hunger {curSlimeData.hungerLevel} out of 100");
        hungerBarFill.fillAmount = (curSlimeData.hungerLevel / 100f);
    }


    // ============== Release Slime ==============
    public void Release()
    {
        curSlimeControl.ReleaseSlime();
    }
}
