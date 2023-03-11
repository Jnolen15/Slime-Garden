using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileablePlaceable : MonoBehaviour
{
    private GridSystem gridSystem;
    private PlaceableObjectSO data;

    // Configuration game objects
    [SerializeField] private GameObject post;
    [SerializeField] private GameObject end;
    [SerializeField] private GameObject straight;
    [SerializeField] private GameObject corner;
    [SerializeField] private GameObject threeWay;
    [SerializeField] private GameObject fourWay;
    
    // Neighboring fence placeables
    [SerializeField] private TileablePlaceable northNeighbor;
    [SerializeField] private TileablePlaceable eastNeighbor;
    [SerializeField] private TileablePlaceable southNeighbor;
    [SerializeField] private TileablePlaceable westNeighbor;

    void Start()
    {
        gridSystem = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridSystem>();
        data = this.GetComponentInParent<PlaceableObject>().GetPlaceableData();

        InitialConfiguration();
    }

    public void InitialConfiguration()
    {
        // Check NESW
        CheckNeighbors();

        // Swap GameObj
        UpdateObject();

        // Update neighbors
        UpdateNeighbors();
    }

    public void UpdateConfiguration()
    {
        // Reset
        post.SetActive(false);
        end.SetActive(false);
        straight.SetActive(false);
        corner.SetActive(false);
        threeWay.SetActive(false);
        fourWay.SetActive(false);

        // Check NESW
        CheckNeighbors();

        // Swap GameObj
        UpdateObject();
    }

    public void UpdateNeighbors()
    {
        if (northNeighbor != null)
            northNeighbor.UpdateConfiguration();
        if (eastNeighbor != null)
            eastNeighbor.UpdateConfiguration();
        if (southNeighbor != null)
            southNeighbor.UpdateConfiguration();
        if (westNeighbor != null)
            westNeighbor.UpdateConfiguration();
    }

    private void CheckNeighbors()
    {
        // Get grid pos of this object
        Vector2Int gridOrigin = this.GetComponentInParent<PlaceableObject>().GetGridOrigin();

        // Get placeable at each direction
        northNeighbor = GetNeighborTileable(gridOrigin.x, gridOrigin.y + 1);
        eastNeighbor = GetNeighborTileable(gridOrigin.x + 1, gridOrigin.y);
        southNeighbor = GetNeighborTileable(gridOrigin.x, gridOrigin.y - 1);
        westNeighbor = GetNeighborTileable(gridOrigin.x - 1, gridOrigin.y);

        /*if (northNeighbor != null)
            Debug.Log("North Neighbor Found", northNeighbor.gameObject);
        if (eastNeighbor != null)
            Debug.Log("East Neighbor Found", eastNeighbor.gameObject);
        if (southNeighbor != null)
            Debug.Log("South Neighbor Found", southNeighbor.gameObject);
        if (westNeighbor != null)
            Debug.Log("Weat Neighbor Found", westNeighbor.gameObject);*/
    }

    // Gets the placeable at given pos. If the name matches this one its returned
    private TileablePlaceable GetNeighborTileable(int xPos, int yPos)
    {
        var placeableObj = gridSystem.GetPlaceableObjectAtGridPos(new Vector2Int(xPos, yPos));
        
        if(placeableObj == null)
            return null;

        var placeableData = placeableObj.GetPlaceableData();
        if (placeableData.name == data.name)
            return placeableObj.gameObject.GetComponentInChildren<TileablePlaceable>();

        return null;
    }

    private void UpdateObject()
    {
        var count = 0;
        if (northNeighbor != null)
            count++;
        if (eastNeighbor != null)
            count++;
        if (southNeighbor != null)
            count++;
        if (westNeighbor != null)
            count++;

        // If 0 adjacent, set post
        if (count == 0)
        {
            post.SetActive(true);
            return;
        }
        else
            post.SetActive(false);

        // If 4 adjacent, set fourway
        if (count == 4)
        {
            fourWay.SetActive(true);
            return;
        }

        // If 1 adjacent, set end and rotate twords it
        if (count == 1)
        {
            end.SetActive(true);

            if (northNeighbor)
                end.transform.rotation = Quaternion.Euler(0, 180, 0);
            else if (eastNeighbor)
                end.transform.rotation = Quaternion.Euler(0, -90, 0);
            else if (southNeighbor)
                end.transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (westNeighbor)
                end.transform.rotation = Quaternion.Euler(0, 90, 0);

            return;
        }

        // If 3 adjacent, set threeway and rotate
        if (count == 3)
        {
            threeWay.SetActive(true);

            if (!northNeighbor)
                threeWay.transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (!eastNeighbor)
                threeWay.transform.rotation = Quaternion.Euler(0, 90, 0);
            else if (!southNeighbor)
                threeWay.transform.rotation = Quaternion.Euler(0, 180, 0);
            else if (!westNeighbor)
                threeWay.transform.rotation = Quaternion.Euler(0, -90, 0);

            return;
        }

        // If 2 adjacent, set either fence or corner, then rotate
        if (count == 2)
        {
            if (northNeighbor)
            {
                // Straight fence
                if (southNeighbor)
                {
                    straight.SetActive(true);
                    straight.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                // Corner fence
                else if (eastNeighbor)
                {
                    corner.SetActive(true);
                    corner.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (westNeighbor)
                {
                    corner.SetActive(true);
                    corner.transform.rotation = Quaternion.Euler(0, 90, 0);
                }

            }
            else if (eastNeighbor)
            {
                // Straight fence
                if (westNeighbor)
                {
                    straight.SetActive(true);
                    straight.transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                // Corner fence
                else if (southNeighbor)
                {
                    corner.SetActive(true);
                    corner.transform.rotation = Quaternion.Euler(0, -90, 0);
                }
            }
            else if (westNeighbor)
            {
                // Corner fence
                if (southNeighbor)
                {
                    corner.SetActive(true);
                    corner.transform.rotation = Quaternion.Euler(0, -0, 0);
                }
            }

            return;
        }
    }
}
