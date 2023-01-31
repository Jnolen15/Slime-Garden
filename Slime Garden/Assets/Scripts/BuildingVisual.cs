using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingVisual : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayerMask;
    private Transform visual;
    private Transform topLeft;
    private Transform topRight;
    private Transform botLeft;
    private Transform botRight;
    private GridSystem gridSystem;
    private PlaceableObjectSO placeableSO;

    private void Start()
    {
        visual = transform.GetChild(0);
        var bounds = transform.GetChild(1);
        topLeft = bounds.transform.GetChild(0);
        topRight = bounds.transform.GetChild(1);
        botLeft = bounds.transform.GetChild(2);
        botRight = bounds.transform.GetChild(3);

        gridSystem = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridSystem>();

        RefreshVisual();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit mousePos, 999f, groundLayerMask))
        {
            Vector3 targetpos = gridSystem.GetSnappedWorldPos(mousePos.point);
            targetpos = new Vector3(targetpos.x, 0.5f, targetpos.z);

            visual.transform.position = targetpos;
            visual.transform.rotation = Quaternion.Euler(0, placeableSO.GetRotationAngle(gridSystem.GetRotationDir()), 0);

            AdjustBounds(mousePos.point);
        }
    }

    public void RefreshVisual()
    {
        if (visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }

        placeableSO = gridSystem.GetPlaceableSO();

        if (placeableSO != null)
        {
            visual = Instantiate(placeableSO.visual, Vector3.zero, Quaternion.identity);
            visual.parent = transform;
        }
    }

    private void AdjustBounds(Vector3 mousePos)
    {
        List<Vector2Int> positions = gridSystem.GetPlaceablePositions(mousePos);

        // Account for rotiation
        var rotDir = 0;
        if (gridSystem.GetRotationDir() == PlaceableObjectSO.Dir.Down || gridSystem.GetRotationDir() == PlaceableObjectSO.Dir.Up)
            rotDir = placeableSO.height;
        else
            rotDir = placeableSO.width;

        // BOT LEFT
        SetCorner(botLeft, positions, 0);

        // TOP RIGHT
        SetCorner(topRight, positions, positions.Count - 1);

        // BOT RIGHT
        var num = (positions.Count) - rotDir;
        SetCorner(botRight, positions, num);

        // TOP LEFT
        SetCorner(topLeft, positions, rotDir - 1);
    }

    private void SetCorner(Transform corner, List<Vector2Int> positions, int listPos)
    {
        var worldpos = gridSystem.GetGridAsWorldPos(positions[listPos].x, positions[listPos].y);
        Vector3 newPos = new Vector3(worldpos.x, 0.1f, worldpos.z);
        corner.transform.position = newPos;
    }
}
