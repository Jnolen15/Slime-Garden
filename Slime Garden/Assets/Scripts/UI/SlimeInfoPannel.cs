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
    //[SerializeField] private TextMeshProUGUI baseColorText;
    //[SerializeField] private TextMeshProUGUI patternColorText;
    //[SerializeField] private TextMeshProUGUI patternText;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private SlimeData curslime;

    private void Start()
    {
        UpdateCropMenu();
    }

    public void Setup(SlimeData slime)
    {
        curslime = slime;
        nameTextObj.SetActive(true);
        nameInputObj.SetActive(false);
        nameText.text = curslime.displayName;
        inputText.text = "New Name";
        speciesText.text = curslime.sBaseColor + " " + curslime.sPatternColor + " " + curslime.slimeSpeciesPattern.sPattern;
        //baseColorText.text = "Color 1: " + curslime.sBaseColor;
        //patternColorText.text = "Color 2: " + curslime.sPatternColor;
        //patternText.text = "Pattern: " + curslime.slimeSpeciesPattern.sPattern;
        rarityText.text = "Rarity: " + curslime.sRarity;

        UpdateCropCount();
    }

    public void OpenRename()
    {
        nameTextObj.SetActive(false);
        nameInputObj.SetActive(true);
    }

    public void SetRename(string newName)
    {
        curslime.SetName(newName);

        nameText.text = curslime.displayName;
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
            // Take from inventory
            Debug.Log("Fed " + crop.cropName);
            invManager.AddCrop(crop, -1);
            UpdateCropCount();

            // Set slime state
            var slime = curslime.gameObject.GetComponent<SlimeController>();
            slime.ChangeState(SlimeController.State.eat);

            // Animate Crop
            Vector3 spawnPos = Camera.main.transform.position;
            var instCrop = Instantiate(crop.cropObj, spawnPos, Quaternion.identity);
            instCrop.GetComponent<CropObj>().Setup(crop);

            Vector3 destination = new Vector3(curslime.transform.position.x, curslime.transform.position.y, curslime.transform.position.z - 0.5f);

            instCrop.transform.DOJump(destination, 2f, 1, 0.75f).OnComplete(() => instCrop.GetComponent<CropObj>().DestroySelf());
        }
    }
}
