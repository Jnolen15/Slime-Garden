using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenManager : MonoBehaviour
{
    [SerializeField] private float tickSpeed;
    [SerializeField] private float tickTime;

    [SerializeField] private List<PlantSpot> plantSpotList = new List<PlantSpot>();
    [SerializeField] private List<BeeBox> beeBoxList = new List<BeeBox>();

    private void Start()
    {
        tickTime = tickSpeed;
    }

    void Update()
    {
        if(tickTime > 0)
        {
            tickTime -= Time.deltaTime;
        } else
        {
            tickTime = tickSpeed;
            OnTick();
        }
    }

    public void AddToList(PlantSpot spot)
    {
        plantSpotList.Add(spot);
    }

    public void RemoveFromList(PlantSpot spot)
    {
        plantSpotList.Remove(spot);
    }

    public void AddToBeeBoxList(BeeBox box)
    {
        beeBoxList.Add(box);
    }

    public void RemoveFromBeeBoxList(BeeBox box)
    {
        beeBoxList.Remove(box);
    }

    private void OnTick()
    {
        //Debug.Log("TICK");

        foreach(PlantSpot spot in plantSpotList)
        {
            spot.GrowTick();
        }

        foreach (BeeBox box in beeBoxList)
        {
            box.GrowTick();
        }
    }
}
