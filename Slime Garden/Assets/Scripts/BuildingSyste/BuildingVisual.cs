using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingVisual : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Material ghostMat;
    [SerializeField] private Material redGhostMat;
    private Transform visual;
    private Transform topLeft;
    private Transform topRight;
    private Transform botLeft;
    private Transform botRight;
    private GridSystem gridSystem;
    private PlaceableObjectSO placeableSO;
    private Vector3 prevPos;
    private Quaternion prevRot;

    public enum Mode
    {
        Place,
        Select
    }
    public Mode mode;

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
            Quaternion targetrot = Quaternion.Euler(0, placeableSO.GetRotationAngle(gridSystem.GetRotationDir()), 0);

            // Don't continue if have not moved or rotated
            if (prevPos == targetpos && prevRot == targetrot)
                return;

            if(mode == Mode.Place)
            {
                visual.transform.position = targetpos;
                visual.transform.rotation = targetrot;

                AdjustBounds(mousePos.point);
                TestCanPlace(mousePos.point);

                prevPos = targetpos;
                prevRot = targetrot;
            }
            else if (mode == Mode.Select)
            {
                // TODO
                AdjustSelectBounds(mousePos.point);

                prevPos = targetpos;
            }
        }
    }

    public void SetMode(string state)
    {
        visual.gameObject.SetActive(false);
        visual.transform.rotation = Quaternion.identity;

        switch (state)
        {
            case "Place":
                mode = Mode.Place;
                visual.gameObject.SetActive(true);
                break;
            case "Select":
                mode = Mode.Select;
                visual.gameObject.SetActive(false);
                break;
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
            visual.GetComponentInChildren<Renderer>().material = ghostMat;
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

    private void AdjustSelectBounds(Vector3 mousePos)
    {
        var selectedPlaceable = gridSystem.GetPlaceableObject(mousePos);
        List<Vector2Int> positions = gridSystem.GetPlacedObjPositions(mousePos);

        if (selectedPlaceable == null || positions == null)
        {
            // When not over buildable, set corners on single space
            gridSystem.RotateTo(PlaceableObjectSO.Dir.Down);
            Vector3 snappedPos = gridSystem.GetSnappedWorldPos(mousePos);
            SetCornerDefault(botLeft, snappedPos);
            SetCornerDefault(topRight, snappedPos);
            SetCornerDefault(botRight, snappedPos);
            SetCornerDefault(topLeft, snappedPos);
        } else
        {
            // Account for rotiation
            var rotDir = 0;
            if (selectedPlaceable.GetPlaceableDir() == PlaceableObjectSO.Dir.Down || selectedPlaceable.GetPlaceableDir() == PlaceableObjectSO.Dir.Up)
                rotDir = selectedPlaceable.GetPlaceableData().height;
            else
                rotDir = selectedPlaceable.GetPlaceableData().width;

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
    }

    private void SetCorner(Transform corner, List<Vector2Int> positions, int listPos)
    {
        var worldpos = gridSystem.GetGridAsWorldPos(positions[listPos].x, positions[listPos].y);
        Vector3 newPos = new Vector3(worldpos.x, 0.1f, worldpos.z);
        corner.transform.position = newPos;
    }

    private void SetCornerDefault(Transform corner, Vector3 position)
    {
        Vector3 newPos = new Vector3(position.x, 0.1f, position.z);
        corner.transform.position = newPos;
    }

    private void TestCanPlace(Vector3 mousePos)
    {
        if (gridSystem.PlacementViable(mousePos))
            visual.GetComponentInChildren<Renderer>().material = ghostMat;
        else
            visual.GetComponentInChildren<Renderer>().material = redGhostMat;
    }
}
