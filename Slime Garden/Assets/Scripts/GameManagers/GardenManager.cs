using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenManager : MonoBehaviour
{
    [SerializeField] private float tickSpeed;
    [SerializeField] private float tickTime;

    [SerializeField]
    private List<PlantSpot> plantSpotList = new List<PlantSpot>();

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

    private void OnTick()
    {
        //Debug.Log("TICK");

        foreach(PlantSpot spot in plantSpotList)
        {
            spot.GrowTick();
        }
    }
}
