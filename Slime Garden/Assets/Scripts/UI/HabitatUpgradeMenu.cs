using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HabitatUpgradeMenu : MonoBehaviour
{
    private GridDataPersistence gridData;
    private DataPersistenceManager gameData;
    private PlayerData pData;
    [SerializeField] private PreviewStageManager stageManager;
    [SerializeField] private TextMeshProUGUI upgradePriceText;
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private int[] upgradePrices;

    [SerializeField] private int borderStyleIndex;
    [SerializeField] private int borderPaintIndex;
    private Transform borderPreview;

    void Start()
    {
        gridData = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridDataPersistence>();
        gameData = GameObject.FindGameObjectWithTag("Data").GetComponent<DataPersistenceManager>();
        pData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
    }

    private void OnEnable()
    {
        SwapStageStyle();
        UpdateUpgradePage();
    }

    // ============== Habitat Upgrading ==============
    public void Upgrade()
    {
        if (pData.Money < upgradePrices[gridData.GetHabitatTeir()])
            return;

        Debug.Log("Upgrading grid");

        pData.GainMoney(-upgradePrices[gridData.GetHabitatTeir()]);

        gridData.UpgradeHabitat();

        gameData.SaveGame();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateUpgradePage()
    {
        string priceTxt = "Upgrade Habitat: ";
        if (gridData.GetHabitatTeir() == 0)
            priceTxt += upgradePrices[gridData.GetHabitatTeir()].ToString();
        else if (gridData.GetHabitatTeir() == 1)
            priceTxt += upgradePrices[gridData.GetHabitatTeir()].ToString();
        else if (gridData.GetHabitatTeir() == 2)
            priceTxt += upgradePrices[gridData.GetHabitatTeir()].ToString();
        else if (gridData.GetHabitatTeir() == 3)
        {
            priceTxt = "Habitat at max teir";
            upgradeButton.SetActive(false);
        }

        upgradePriceText.text = priceTxt;
    }

    // ============== Border Styling ==============
    public List<BorderStyles> borderStyles = new List<BorderStyles>();

    [System.Serializable]
    public class BorderStyles
    {
        public GameObject style;
        public Material[] materials;
    }

    public void ChangeBorderStyle(bool increase)
    {
        if(increase)
            borderStyleIndex++;
        else
            borderStyleIndex--;

        borderPaintIndex = 0;

        if (borderStyleIndex >= borderStyles.Count)
            borderStyleIndex = 0;
        else if (borderStyleIndex < 0)
            borderStyleIndex = borderStyles.Count-1;

        SwapStageStyle();
    }

    public void ChangeBorderPaint(bool increase)
    {
        if (increase)
            borderPaintIndex++;
        else
            borderPaintIndex--;

        if (borderPaintIndex >= borderStyles[borderStyleIndex].materials.Length)
            borderPaintIndex = 0;
        else if (borderPaintIndex < 0)
            borderPaintIndex = borderStyles[borderStyleIndex].materials.Length-1;

        SwapStagePaint();
    }

    public void UpdateBorders()
    {
        gridData.UpdateBorderStyle(new Vector2Int(borderStyleIndex, borderPaintIndex));
    }

    private void SwapStageStyle()
    {
        borderPreview = Instantiate(borderStyles[borderStyleIndex].style).transform;

        stageManager.gameObject.SetActive(true);
        stageManager.ShowGameObject(borderPreview);
    }

    private void SwapStagePaint()
    {
        foreach (MeshRenderer rend in borderPreview.GetComponentsInChildren<MeshRenderer>())
        {
            rend.material = borderStyles[borderStyleIndex].materials[borderPaintIndex];
        }
    }
}
