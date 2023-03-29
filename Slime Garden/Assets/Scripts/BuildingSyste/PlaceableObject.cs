using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlaceableObject : MonoBehaviour
{
    public static PlaceableObject Create(Vector3 worldPos, Vector2Int origin, PlaceableObjectSO.Dir dir, PlaceableObjectSO placeableSO, GridSystem grid)
    {
        Transform placeableObjectTransform = Instantiate(placeableSO.prefab, worldPos, Quaternion.Euler(0, placeableSO.GetRotationAngle(dir), 0));

        PlaceableObject placeableObject = placeableObjectTransform.GetComponent<PlaceableObject>();

        placeableObject.placeableObjectSO = placeableSO;
        placeableObject.gridParent = grid;
        placeableObject.origin = origin;
        placeableObject.dir = dir;

        return placeableObject;
    }
    
    private PlaceableObjectSO placeableObjectSO;
    private GridSystem gridParent;
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

        // Dust VFX
        DustCoudEffect();

        // Stop any DOTween functions that may be going
        transform.DOKill();
        
        Destroy(gameObject);
    }

    public PlaceableObjectSO GetPlaceableData()
    {
        return placeableObjectSO;
    }

    public void AnimatePlacement()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y+1, transform.position.z);
        transform.DOMoveY(0, 0.5f).SetEase(Ease.OutBounce);
        DustCoudEffect();
    }

    private void DustCoudEffect()
    {
        foreach (Vector2Int pos in placeableObjectSO.GetGridPositionList(origin, dir))
        {
            Vector3 worldPos = gridParent.GetGridAsWorldPos(pos.x, pos.y);
            worldPos = new Vector3(worldPos.x + 0.5f, worldPos.y, worldPos.z + 0.5f);
            Instantiate(Resources.Load("Particles/DustCloud_FX"), worldPos, transform.rotation);
        }
    }
}
