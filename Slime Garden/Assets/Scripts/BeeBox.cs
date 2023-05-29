using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BeeBox : MonoBehaviour, IInteractable
{
    private GridSystem gridSystem;
    private PlaceableObjectSO placeableData;
    private PlayerData pData;
    private StatTracker stats;
    private GardenManager gm;

    [SerializeField] Vector2Int honeyProductionTime;
    public int honeyTimer;

    [SerializeField] private GameObject honey;
    [SerializeField] private GameObject honeyFX;
    [SerializeField] private ParticleSystem beeFX;
    [SerializeField] private List<Vector2Int> positions = new List<Vector2Int>();
    [SerializeField] private List<string> flowers = new List<string>();
    [SerializeField] private bool hasHoney;
    [SerializeField] private int honeyValue;

    void Start()
    {
        gridSystem = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridSystem>();
        placeableData = this.GetComponentInParent<PlaceableObject>().GetPlaceableData();
        pData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        stats = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<StatTracker>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GardenManager>();
        gm.AddToBeeBoxList(this);

        if (hasHoney)
            honey.SetActive(true);

        if (honeyTimer == 0)
            AttemptProduceHoney();
        else if (honeyTimer == -1)
            honeyTimer = Random.Range(honeyProductionTime.x, honeyProductionTime.y);
    }

    public void Interact()
    {
        // Harvest
        if (hasHoney)
        {
            transform.GetChild(0).DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f);
            transform.GetChild(1).DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f);

            hasHoney = false;
            honey.SetActive(false);
            pData.GainMoney(honeyValue);
            honeyValue = 0;
            honeyTimer = Random.Range(honeyProductionTime.x, honeyProductionTime.y);

            stats.IncrementStat("honeyCollected", 1);
        }
    }

    public void GrowTick()
    {
        if (honeyTimer > 0 && !hasHoney)
            honeyTimer--;

        if (honeyTimer == 0 && !hasHoney)
            AttemptProduceHoney();
    }

    private void AttemptProduceHoney()
    {
        CheckNeighbors();
        //Debug.Log("There are " + flowers.Count + "/8 Plant spots nearby");

        if (flowers.Count <= 0)
            return;

        // Effects
        transform.GetChild(0).DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f);
        transform.GetChild(1).DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f);
        Instantiate(honeyFX, new Vector3(transform.position.x+0.5f, transform.position.y+1, transform.position.z+0.5f), Quaternion.identity);
        var fxEmmission = beeFX.emission;
        fxEmmission.rateOverTime = (flowers.Count/8);

        // Set / Reset
        hasHoney = true;
        honeyTimer = 0;
        honey.SetActive(true);
        honeyValue = (10 * flowers.Count);
    }

    private void CheckNeighbors()
    {
        // Get grid pos of this object
        Vector2Int gridOrigin = this.GetComponentInParent<PlaceableObject>().GetGridOrigin();

        flowers.Clear();

        // Loop through all adjacent spaces, If there is a plant spot with a flower add it to the list
        foreach (Vector2Int pos in positions)
        {
            string plant = GetNeighborPlantSpot(gridOrigin.x + pos.x, gridOrigin.y + pos.y);
            if (plant != null)
            {
                if(plant.Contains("Flower"))
                    flowers.Add(plant);
            }
        }
    }

    // Gets the placeable at given pos. If it has a plant spot with a fully grown crop, return name of crop
    private string GetNeighborPlantSpot(int xPos, int yPos)
    {
        if (!placeableData)
            placeableData = this.GetComponentInParent<PlaceableObject>().GetPlaceableData();

        var placeableObj = gridSystem.GetPlaceableObjectAtGridPos(new Vector2Int(xPos, yPos));

        if (placeableObj == null)
            return null;

        PlantSpot crop = placeableObj.gameObject.GetComponentInChildren<PlantSpot>();

        if(crop == null)
            return null;

        if (crop.hasCrop && crop.fullyGrown)
        {
            string cropName = crop.GetCropSO().cropName;
            return cropName;
        }

        return null;
    }

    private void OnDestroy()
    {
        gm.RemoveFromBeeBoxList(this);

        transform.GetChild(0).DOKill();
        transform.GetChild(1).DOKill();
    }
}
