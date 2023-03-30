using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDataPersistence : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GridSystem gridSystem;
    [SerializeField] private InventoryManager invManager;
    private List<PlaceableObject> placeables;
    private List<PlaceableData> placeableDataList = new List<PlaceableData>();

    [System.Serializable]
    public class PlaceableData
    {
        public string pName;
        public Vector2Int pOrigin;
        public PlaceableObjectSO.Dir dir;
        public PlantSpotData plantSpot;

        public PlaceableData(string newName, Vector2Int newOrigin, PlaceableObjectSO.Dir newDir, PlantSpotData pSpot = null)
        {
            pName = newName;
            pOrigin = newOrigin;
            dir = newDir;
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
        
        foreach (PlaceableObjectSO pData in invManager.availablePlaceables)
        {
            if (pData.placeableName == pName)
                return pData;
        }

        return null;
    }

    private CropSO FindCropData(string cName)
    {

        foreach (CropSO cData in invManager.availableCrops)
        {
            if (cData.cropName == cName)
                return cData;
        }

        return null;
    }

    // =================== SAVE LOAD ===================
    public void LoadData(GameData data)
    {
        Debug.Log("Attempting to load placeable list of size: " + data.placeableList.Count);

        foreach (PlaceableData pData in data.placeableList)
        {
            GameObject newBuild = null;

            if (FindPlaceableData(pData.pName))
            {
                gridSystem.RotateTo(pData.dir);
                newBuild = gridSystem.Build(pData.pOrigin, FindPlaceableData(pData.pName), false);
            }
            else
            {
                Debug.LogError("Failed to load, placeable data not found!");
            }

            // If placeable had PlantSpotData, reinstantiate that as well
            var pSpot = newBuild.GetComponentInChildren<PlantSpot>();
            if (pSpot)
            {
                pSpot.SetPlant(FindCropData(pData.plantSpot.cropName), 
                    pData.plantSpot.fullyGrown, pData.plantSpot.curTick, 
                    pData.plantSpot.wateredTicks, pData.plantSpot.growthStage);
            }
        }
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

            // If placeable has a plant spot with a crop, also save plant spot data
            PlantSpot pSpot = pObj.GetComponentInChildren<PlantSpot>();
            if (pSpot && pSpot.hasCrop)
            {
                //Debug.Log("Has Plant spot with: " + pSpot.curCropSO.cropName);

                PlantSpotData pSData = new PlantSpotData(pSpot.curCropSO.cropName,
                    pSpot.fullyGrown, pSpot.curTick, pSpot.wateredTicks, pSpot.growthStage);

                placeableDataList.Add(new PlaceableData(pObj.GetPlaceableData().placeableName,
                    pObj.GetGridOrigin(), pObj.GetPlaceableDir(), pSData));
            } 
            // Otherwise save normally
            else
            {
                placeableDataList.Add(new PlaceableData(pObj.GetPlaceableData().placeableName,
                    pObj.GetGridOrigin(), pObj.GetPlaceableDir()));
            }
        }

        data.placeableList = placeableDataList;
    }
}
