using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDataPersistence : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GridSystem gridSystem;
    [SerializeField] private InventoryManager invManager;
    private List<PlaceableObject> placeables;
    private List<PlaceableData> placeableDataList = new List<PlaceableData>();
    [SerializeField] private int habitatTier;
    [SerializeField] private List<HabitatTiers> habitatTiers = new List<HabitatTiers>();
    [SerializeField] private Vector2Int borderStyle;

    [System.Serializable]
    public class HabitatTiers
    {
        public int tNum;
        public int gridSize;
        public GameObject environment;

        public HabitatTiers(int num, int size, GameObject env)
        {
            tNum = num;
            gridSize = size;
            environment = env;
        }
    }

    [System.Serializable]
    public class PlaceableData
    {
        public string pName;
        public Vector2Int pOrigin;
        public PlaceableObjectSO.Dir dir;
        public int matValue;
        public int beeBoxHoneyTimer;
        public List<PlantSpotData> plantSpot;

        public PlaceableData(string newName, Vector2Int newOrigin, PlaceableObjectSO.Dir newDir, int matVal = 0, int honeyTime = 0, List<PlantSpotData> pSpot = null)
        {
            pName = newName;
            pOrigin = newOrigin;
            dir = newDir;
            matValue = matVal;
            beeBoxHoneyTimer = honeyTime;
            plantSpot = pSpot;
        }
    }

    [System.Serializable]
    public class PlantSpotData
    {
        public string cropName;
        public bool fullyGrown;
        public int curTick;
        public int wateredTicks;
        public int growthStage;

        public PlantSpotData(string newName, bool isGrown, int newCurTick, int newWateredTicks, int newGrowthStage)
        {
            cropName = newName;
            fullyGrown = isGrown;
            curTick = newCurTick;
            wateredTicks = newWateredTicks;
            growthStage = newGrowthStage;
        }
    }

    // =================== SO RETREIVAL ===================
    private PlaceableObjectSO FindPlaceableData(string pName)
    {
        
        foreach (PlaceableObjectSO pData in invManager.inGamePlaceables)
        {
            if (pData.placeableName == pName)
                return pData;
        }

        return null;
    }

    private CropSO FindCropData(string cName)
    {

        foreach (CropSO cData in invManager.inGameCrops)
        {
            if (cData.cropName == cName)
                return cData;
        }

        return null;
    }

    // =================== UPGRADE / STYLE HANDLING ===================
    public int GetHabitatTeir()
    {
        return habitatTier;
    }

    public void UpgradeHabitat()
    {
        habitatTier++;
    }

    public void UpdateBorderStyle(Vector2Int index)
    {
        borderStyle = index;

        var bordersToUpdate = GameObject.FindObjectsOfType<BorderFenceStyle>();

        foreach (BorderFenceStyle border in bordersToUpdate)
        {
            border.ChangeStyle(borderStyle.x, borderStyle.y);
        }
    }

    // =================== SAVE LOAD ===================
    public void LoadData(GameData data)
    {
        // Grid setup
        habitatTier = data.habitatTier;
        gridSystem.InstantiateGrid(habitatTiers[habitatTier].gridSize);

        Debug.Log("Attempting to load placeable list of size: " + data.placeableList.Count);

        // Load placeables
        foreach (PlaceableData pData in data.placeableList)
        {
            GameObject newBuild = null;

            if (FindPlaceableData(pData.pName))
            {
                gridSystem.RotateTo(pData.dir);
                newBuild = gridSystem.Build(pData.pOrigin, FindPlaceableData(pData.pName), false);

                // If placeable can be mat swapped
                var matSwapper = newBuild.GetComponent<MaterialSwapper>();
                if (matSwapper != null)
                    matSwapper.SetMat(pData.matValue);

                // If placeable a bee box
                if (newBuild.GetComponent<BeeBox>())
                    newBuild.GetComponent<BeeBox>().honeyTimer = pData.beeBoxHoneyTimer;

                // If placeable had PlantSpotData, reinstantiate that as well
                int count = 0;
                foreach (PlantSpot pSpot in newBuild.GetComponentsInChildren<PlantSpot>())
                {
                    if (pData.plantSpot[count].cropName != "empty")
                    {
                        pSpot.SetPlant(FindCropData(pData.plantSpot[count].cropName),
                            pData.plantSpot[count].fullyGrown, pData.plantSpot[count].curTick,
                            pData.plantSpot[count].wateredTicks, pData.plantSpot[count].growthStage);
                    }

                    count++;
                }
            }
            else
            {
                Debug.LogError("ERROR: Failed to load, placeable data not found!");
            }
        }

        // Environment setup
        habitatTiers[habitatTier].environment.SetActive(true);
        UpdateBorderStyle(data.borderFenceStyle);
    }

    public void SaveData(GameData data)
    {
        // Clear previous data
        data.placeableList.Clear();

        placeables = gridSystem.GetAllPlaceableObjects();

        Debug.Log("Saving Placeable list of size: " + placeables.Count);

        foreach(PlaceableObject pObj in placeables)
        {
            //Debug.Log("Attempting to save: " + pObj.GetPlaceableData().placeableName);

            // If placeable can be mat swapped
            var matSwapper = pObj.GetComponent<MaterialSwapper>();
            int matIndex = 0;
            if (matSwapper != null)
                matIndex = matSwapper.GetMatIndex();

            // If placeable has a plant spot with a crop, also save plant spot data
            List<PlantSpotData> pSpots = new List<PlantSpotData>();
            foreach (PlantSpot pSpot in pObj.GetComponentsInChildren<PlantSpot>())
            {
                //Debug.Log(pObj.name + " has Plant spot with: " + pSpot.curCropSO.cropName);

                PlantSpotData pSData = null;

                // Save if there is a crop, if not save empty
                if (pSpot.hasCrop)
                {
                    pSData = new PlantSpotData(pSpot.curCropSO.cropName,
                        pSpot.fullyGrown, pSpot.curTick, pSpot.wateredTicks, pSpot.growthStage);
                } else
                {
                    pSData = new PlantSpotData("empty", false, 0, 0, 0);
                }

                pSpots.Add(pSData);
            }

            // If had no plant spots
            if (pSpots.Count == 0)
                pSpots = null;

            // If bee box
            int honeyTimer = 0;
            if (pObj.GetComponent<BeeBox>())
                honeyTimer = pObj.GetComponent<BeeBox>().honeyTimer;

            placeableDataList.Add(new PlaceableData(pObj.GetPlaceableData().placeableName,
                            pObj.GetGridOrigin(), pObj.GetPlaceableDir(), matIndex, honeyTimer, pSpots));
        }

        // Save habitat teir / decor
        data.habitatTier = habitatTier;
        data.borderFenceStyle = borderStyle;

        data.placeableList = placeableDataList;
    }
}
