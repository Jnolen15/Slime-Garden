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

        public PlaceableData(string newName, Vector2Int newOrigin, PlaceableObjectSO.Dir newDir)
        {
            pName = newName;
            pOrigin = newOrigin;
            dir = newDir;
        }
    }

    private PlaceableObjectSO FindPlaceableData(string pName)
    {
        
        foreach (PlaceableObjectSO pData in invManager.availablePlaceables)
        {
            if (pData.placeableName == pName)
                return pData;
        }

        return null;
    }

    public void LoadData(GameData data)
    {
        Debug.Log("Attempting to load placeable list of size: " + data.placeableList.Count);

        foreach (PlaceableData pData in data.placeableList)
        {
            if (FindPlaceableData(pData.pName))
            {
                gridSystem.RotateTo(pData.dir);
                gridSystem.Build(pData.pOrigin, FindPlaceableData(pData.pName));
            }
            else
            {
                Debug.LogError("Failed to load, placeable data not found!");
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
            Debug.Log("Attempting to save: " + pObj.GetPlaceableData().placeableName);

            placeableDataList.Add(new PlaceableData(pObj.GetPlaceableData().placeableName, 
                pObj.GetGridOrigin(), pObj.GetPlaceableDir()));
        }

        data.placeableList = placeableDataList;
    }
}
