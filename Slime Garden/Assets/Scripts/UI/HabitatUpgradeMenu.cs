using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HabitatUpgradeMenu : MonoBehaviour
{
    private GridDataPersistence gridData;
    private DataPersistenceManager gameData;
    [SerializeField] private PreviewStageManager stageManager;

    [SerializeField] private int borderStyleIndex;
    [SerializeField] private int borderPaintIndex;
    private Transform borderPreview;

    void Start()
    {
        gridData = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridDataPersistence>();
        gameData = GameObject.FindGameObjectWithTag("Data").GetComponent<DataPersistenceManager>();
    }

    private void OnEnable()
    {
        SwapStageStyle();
    }

    // ============== Habitat Upgrading ==============
    public void Upgrade()
    {
        Debug.Log("Upgrading grid");

        gridData.UpgradeHabitat();

        gameData.SaveGame();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
