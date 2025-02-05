using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private bool inWild;
    [SerializeField] private GameObject buildTool;
    [SerializeField] private GameObject destroyTool;
    [SerializeField] private GameObject plantTool;
    [SerializeField] private GameObject paintTool;
    [SerializeField] private GameObject tooltips;
    [SerializeField] private GameObject buildMenu;
    [SerializeField] private GameObject placeableUIPrefab;
    [SerializeField] private GameObject seedMenu;
    [SerializeField] private GameObject seedUIPrefab;
    [SerializeField] private GameObject infoBox;
    [SerializeField] private GameObject slimeInfoBox;
    [SerializeField] private GameObject cropSellBox;
    [SerializeField] private GameObject habitatUpgradeBox;
    [SerializeField] private GameObject taskBoard;
    [SerializeField] private GameObject fps;
    [SerializeField] private GameObject cSToken;
    [SerializeField] private GameObject textPopup;
    [SerializeField] private RectTransform tokenEndPos;
    [SerializeField] private InventoryManager invManager;
    [SerializeField] private PreviewStageManager stageManager;

    void Start()
    {
        UpdateBuildMenu();
        buildMenu.SetActive(false);
        UpdateSeedMenu();
        seedMenu.SetActive(false);
        infoBox.SetActive(false);
        slimeInfoBox.SetActive(false);
        cropSellBox.SetActive(false);
        habitatUpgradeBox.SetActive(false);
        taskBoard.SetActive(false);

        if (PlayerPrefs.GetInt("TooltipsEnabled") == 1 && !inWild)
            tooltips.SetActive(true);
        else if (PlayerPrefs.GetInt("TooltipsEnabled") == 0)
            tooltips.SetActive(false);

        if (PlayerPrefs.GetInt("FPSEnabled") == 1)
            fps.SetActive(true);
        else if (PlayerPrefs.GetInt("FPSEnabled") == 0)
            fps.SetActive(false);
    }

    public void CloseAllSubmenus()
    {
        buildMenu.SetActive(false);
        seedMenu.SetActive(false);
    }

    // ============== Level Up ==============
    public void UnlockTool(string tool)
    {
        if (inWild)
            return;

        if (tool == "build")
            buildTool.SetActive(true);
        else if (tool == "destroy")
            destroyTool.SetActive(true);
        else if (tool == "plant")
            plantTool.SetActive(true);
        else if (tool == "paint")
            paintTool.SetActive(true);
        else
            Debug.LogError("Tool upgrade not found");
    }

    // ============== Build Menus ==============
    public void UpdateBuildMenu()
    {
        buildMenu.GetComponent<ContentMenu>().UpdateContent("placeable", placeableUIPrefab, invManager, this);
    }

    public void BuildMenuActive(bool value)
    {
        buildMenu.SetActive(value);

        if (value)
        {
            var menuPos = buildMenu.transform.localPosition;
            var xPos = menuPos.x;
            buildMenu.transform.localPosition = new Vector3(menuPos.x - 40, menuPos.y, menuPos.z);
            buildMenu.transform.DOLocalMoveX(xPos, 0.2f);
        }
    }

    // ============== Seed Menus ==============
    public void UpdateSeedMenu()
    {
        seedMenu.GetComponent<ContentMenu>().UpdateContent("seed", seedUIPrefab, invManager, this);
    }

    public void SeedMenuActive(bool value)
    {
        seedMenu.SetActive(value);

        if (value)
        {
            var menuPos = seedMenu.transform.localPosition;
            var xPos = menuPos.x;
            seedMenu.transform.localPosition = new Vector3(menuPos.x - 40, menuPos.y, menuPos.z);
            seedMenu.transform.DOLocalMoveX(xPos, 0.2f);
        }
    }

    // ============== Info Box ==============
    public void SetInfoBox(string title, int price, string description, Sprite previewSprite = null)
    {
        StopAllCoroutines();
        infoBox.SetActive(true);
        InfoPannelManager ip = infoBox.GetComponent<InfoPannelManager>();
        ip.Setup(title, price, description);

        // Optional sprite if not a placeable on the stage
        if(previewSprite != null)
            ip.DisplayImage(previewSprite);
    }

    public void SetupPreview(PlaceableObjectSO data)
    {
        stageManager.gameObject.SetActive(true);
        stageManager.Setup(data);
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

        if(stageManager != null)
            stageManager.gameObject.SetActive(false);
    }

    // ============== Tooltips ==============
    public void ToggleTooltips()
    {
        tooltips.SetActive(!tooltips.activeSelf);

        if (tooltips.activeSelf)
            PlayerPrefs.SetInt("TooltipsEnabled", 1);
        else
            PlayerPrefs.SetInt("TooltipsEnabled", 0);
    }

    public void ShowTooltip(string tool)
    {
        tooltips.transform.GetChild(0).gameObject.SetActive(false);
        tooltips.transform.GetChild(1).gameObject.SetActive(false);
        tooltips.transform.GetChild(2).gameObject.SetActive(false);
        tooltips.transform.GetChild(3).gameObject.SetActive(false);
        tooltips.transform.GetChild(4).gameObject.SetActive(false);

        if (tool == "interact")
            tooltips.transform.GetChild(0).gameObject.SetActive(true);
        else if (tool == "plant")
            tooltips.transform.GetChild(1).gameObject.SetActive(true);
        else if (tool == "build")
            tooltips.transform.GetChild(2).gameObject.SetActive(true);
        else if (tool == "destroy")
            tooltips.transform.GetChild(3).gameObject.SetActive(true);
        else if (tool == "paint")
            tooltips.transform.GetChild(4).gameObject.SetActive(true);
        else
            Debug.LogError("Tooltip not found");
    }

    // ============== Slime Inspect ==============
    public void UpdateSlimeFeed()
    {
        slimeInfoBox.GetComponent<SlimeInfoPannel>().UpdateCropMenu();
    }

    public void ShowSlimeStats(GameObject slime)
    {
        slimeInfoBox.SetActive(true);
        slimeInfoBox.GetComponent<SlimeInfoPannel>().Setup(slime);

        var menuPos = slimeInfoBox.transform.localPosition;
        var yPos = menuPos.y;
        slimeInfoBox.transform.localPosition = new Vector3(menuPos.x, menuPos.y - 40, menuPos.z);
        slimeInfoBox.transform.DOLocalMoveY(yPos, 0.2f);
    }

    public void CloseSlimeStats()
    {
        slimeInfoBox.SetActive(false);
        CloseInfoBox();
    }

    // ============== Shops ==============
    public void CloseAllShops()
    {
        CloseCropSell();
        CloseHabitatUpgrade();
        CloseTaskBoard();
    }

    // ============== Crop Sell ==============
    public void UpdateCropSell()
    {
        cropSellBox.GetComponent<CropSellMenu>().UpdateCropMenu();
    }

    public void ShowCropSell()
    {
        cropSellBox.SetActive(true);
        cropSellBox.GetComponent<CropSellMenu>().UpdateCropCount();

        var menuPos = cropSellBox.transform.localPosition;
        var yPos = menuPos.y;
        cropSellBox.transform.localPosition = new Vector3(menuPos.x, menuPos.y - 40, menuPos.z);
        cropSellBox.transform.DOLocalMoveY(yPos, 0.2f);
    }

    public void CloseCropSell()
    {
        cropSellBox.SetActive(false);
        CloseInfoBox();
    }

    // ============== Habitat Upgrade ==============
    public void ShowHabitatUpgrade()
    {
        habitatUpgradeBox.SetActive(true);

        var menuPos = habitatUpgradeBox.transform.localPosition;
        var yPos = menuPos.y;
        habitatUpgradeBox.transform.localPosition = new Vector3(menuPos.x, menuPos.y - 40, menuPos.z);
        habitatUpgradeBox.transform.DOLocalMoveY(yPos, 0.2f);
    }

    public void CloseHabitatUpgrade()
    {
        habitatUpgradeBox.SetActive(false);
        stageManager.gameObject.SetActive(false);
        CloseInfoBox();
    }

    // ============== Task Board ==============
    public void ShowTaskBoard()
    {
        taskBoard.SetActive(true);

        taskBoard.GetComponent<TaskUI>().UpdateTasks();

        var menuPos = taskBoard.transform.localPosition;
        var yPos = menuPos.y;
        taskBoard.transform.localPosition = new Vector3(menuPos.x, menuPos.y - 40, menuPos.z);
        taskBoard.transform.DOLocalMoveY(yPos, 0.2f);
    }

    public void CloseTaskBoard()
    {
        taskBoard.SetActive(false);
    }

    // ============== UI Popups =================
    public void AnimateToken(Color color)
    {
        //Vector3 pos = Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue());
        Vector3 pos = Mouse.current.position.ReadValue();
        var token = Instantiate(cSToken, pos, Quaternion.identity, transform);
        token.GetComponent<UIToken>().Setup(tokenEndPos.position, color);
    }

    public void TextPopup(string str, Color color)
    {
        Vector3 pos = Mouse.current.position.ReadValue();
        GameObject popup = Instantiate(textPopup, pos, Quaternion.identity, transform);
        popup.GetComponent<TextPopup>().setup(str.ToString(), color);
    }


    // ============== Misc =================
    public void ToggleFPS(bool toggle)
    {
        fps.SetActive(toggle);

        Debug.Log("FPS Toggled to " + toggle + " saved value is " + PlayerPrefs.GetInt("FPSEnabled"));
    }
}
