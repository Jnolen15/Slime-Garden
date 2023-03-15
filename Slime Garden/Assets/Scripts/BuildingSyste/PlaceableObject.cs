using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public static PlaceableObject Create(Vector3 worldPos, Vector2Int origin, PlaceableObjectSO.Dir dir, PlaceableObjectSO placeableSO)
    {
        Transform placeableObjectTransform = Instantiate(placeableSO.prefab, worldPos, Quaternion.Euler(0, placeableSO.GetRotationAngle(dir), 0));

        PlaceableObject placeableObject = placeableObjectTransform.GetComponent<PlaceableObject>();

        placeableObject.placeableObjectSO = placeableSO;
        placeableObject.origin = origin;
        placeableObject.dir = dir;

        return placeableObject;
    }
    
    private PlaceableObjectSO placeableObjectSO;
    private Vector2Int origin;
    private PlaceableObjectSO.Dir dir;

    // Returns a list of all spaces this placeable is occupying on the grid
    public List<Vector2Int> GetGridPositionList()
    {
        return placeableObjectSO.GetGridPositionList(origin, dir);
    }

    // Returns the origin point this placeable is occupying on the grid
    public Vector2Int GetGridOrigin()
    {
        return origin;
    }

    public PlaceableObjectSO.Dir GetPlaceableDir()
    {
        return dir;
    }

    public void DestroySelf()
    {
        // If this is a tileable object, update surrounding tiles
        var tilePlaceable = this.GetComponentInChildren<TileablePlaceable>();
        if (tilePlaceable)
            tilePlaceable.UpdateNeighbors();

        Destroy(gameObject);
    }

    public PlaceableObjectSO GetPlaceableData()
    {
        return placeableObjectSO;
    }
}
