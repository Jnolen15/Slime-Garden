using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Grid system code originally by CodeMonkey on youtube
// youtube.com/watch?v=dulosHPl82A&t

public class GridSystem : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayerMask;
    [Header("Grid Parameters")]
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private int cellSize;
    [SerializeField] private Vector3 offsett = Vector3.zero;
    [SerializeField] private PlaceableObjectSO placeable;

    private WorldGrid<GridObject> grid;
    private PlaceableObjectSO.Dir dir = PlaceableObjectSO.Dir.Down;

    /*private void Awake()
    {
        // Moved grind instantiation to GridDataPersistence on load
        // This way more can be controlled about how it instantiates

        grid = new WorldGrid<GridObject>(gridWidth, gridHeight, cellSize, offsett,
                                        (WorldGrid<GridObject> g, int x, int y) => new GridObject(g, x, y));
    }*/

    public void InstantiateGrid(int size)
    {
        Debug.Log("The Grid is being instantiated");

        gridWidth = size;
        gridHeight = size;

        grid = new WorldGrid<GridObject>(gridWidth, gridHeight, cellSize, offsett,
                                        (WorldGrid<GridObject> g, int x, int y) => new GridObject(g, x, y));
    }

    // Grid object class. Holds information about the object that is set at each grid position
    public class GridObject
    {
        private WorldGrid<GridObject> grid;
        private int x;
        private int y;
        private PlaceableObject placeableObject;

        public GridObject(WorldGrid<GridObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetPlaceableObject(PlaceableObject transform)
        {
            this.placeableObject = transform;
        }

        public PlaceableObject GetPlaceableObject()
        {
            return placeableObject;
        }

        public void ClearPlaceableObject()
        {
            placeableObject = null;
        }

        public bool CanBuild()
        {
            return placeableObject == null;
        }

        public void print()
        {
            if(placeableObject != null)
                Debug.Log(x + "," + y + ": " + placeableObject.gameObject.name);
            else
                Debug.Log(x + "," + y);
        }
    }

    private void Update()
    {
        // ================== DEBUG
        // Middle click to get info on a grid space
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit mousePos, 999f, groundLayerMask))
        {
            if (Input.GetMouseButtonDown(2))
            {
                // Debug
                grid.GetXZ(mousePos.point, out int a, out int b);
                GridObject gridObj = grid.GetValue(a, b);

                if (gridObj != null)
                {
                    gridObj.print();
                }
                else
                {
                    Debug.Log("GridObject is null " + a + "," + b);
                }
            }
        }
    }

    // ================ Place ================
    public bool Place(Vector3 mousePos)
    {
        grid.GetXZ(mousePos, out int x, out int y);
        var newBuild = Build(new Vector2Int(x, y), placeable, true);

        if (newBuild)
            return true;
        else
            return false;
    }

    public GameObject Build(Vector2Int placeablePos, PlaceableObjectSO placeableToBuild, bool firstPlacement)
    {
        GameObject newBuild = null;

        List<Vector2Int> gridPosList = placeableToBuild.GetGridPositionList(placeablePos, dir);
        GridObject gridObject = grid.GetValue(placeablePos.x, placeablePos.y);

        //Test can Build
        bool canBuild = CanBuildHere(gridPosList);

        // Create placeable
        if (gridObject != null && canBuild)
        {
            Vector2Int rotationOffset = placeableToBuild.GetRotationOffset(dir);
            Vector3 objWorldPos = grid.GetWorldPosition(placeablePos.x, placeablePos.y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * cellSize;

            PlaceableObject placeableObj = PlaceableObject.Create(objWorldPos, placeablePos, dir, placeableToBuild, this);

            foreach (Vector2Int gridPos in gridPosList)
            {
                grid.GetValue(gridPos.x, gridPos.y).SetPlaceableObject(placeableObj);
            }

            newBuild = placeableObj.gameObject;

            if(firstPlacement)
                placeableObj.AnimatePlacement();
        }
        else if (gridObject != null)
        {
            Debug.Log("Can't build here!");
        }
        else
        {
            Debug.Log("GridObject is null " + placeablePos.x + "," + placeablePos.y);
        }

        return newBuild;
    }

    private bool CanBuildHere(List<Vector2Int> gridPosList)
    {
        bool canBuild = true;

        foreach (Vector2Int gridPos in gridPosList)
        {
            if (grid.GetValue(gridPos.x, gridPos.y) == null)
                canBuild = false;
            else if (!grid.GetValue(gridPos.x, gridPos.y).CanBuild())
                canBuild = false;
        }

        return canBuild;
    }

    // ================ Rotate ================
    public void Rotate()
    {
        dir = PlaceableObjectSO.GetNextDir(dir);
    }

    public void RotateTo(PlaceableObjectSO.Dir newDir)
    {
        dir = newDir;
    }

    // ================ Demolish ================
    public void Demolish(Vector3 mousePos)
    {
        grid.GetXZ(mousePos, out int a, out int b);
        GridObject gridObj = grid.GetValue(a, b);
        
        if (gridObj == null)
        {
            Debug.Log("Grid space is NULL");
            return;
        }

        PlaceableObject placeableObj = gridObj.GetPlaceableObject();

        if (placeableObj != null)
        {
            List<Vector2Int> gridPosList = placeableObj.GetGridPositionList();

            foreach (Vector2Int gridPos in gridPosList)
            {
                grid.GetValue(gridPos.x, gridPos.y).ClearPlaceableObject();
            }

            placeableObj.DestroySelf();
        }
        else
        {
            Debug.Log("Nothing at " + a + "," + b + " to demolish");
        }
    }

    // ================ Swap Placeable ================
    public void SwapPlaceable(PlaceableObjectSO newP)
    {
        placeable = newP;
        Debug.Log("Updated placecable to " + placeable.name);
    }

    // ================ Paint ================
    public bool Paint(Vector3 mousePos)
    {
        grid.GetXZ(mousePos, out int a, out int b);
        GridObject gridObj = grid.GetValue(a, b);

        if (gridObj == null)
            return false;

        PlaceableObject placeableObj = gridObj.GetPlaceableObject();

        if (placeableObj != null)
        {
            var matSwapper = placeableObj.gameObject.GetComponent<MaterialSwapper>();
            if(matSwapper != null)
            {
                matSwapper.Swap();
                return true;
            } else
            {
                return false;
            }
        }
        else
        {
            Debug.Log("Nothing at " + a + "," + b + " to paint");
            return false;
        }
    }

    // ================ Helpers ================
    public Vector3 GetSnappedWorldPos(Vector3 mousePos)
    {
        // Returns the world position snapped to the grid
        grid.GetXZ(mousePos, out int x, out int y);
        Vector2Int rotationOffset = placeable.GetRotationOffset(dir);
        Vector3 objWorldPos = grid.GetWorldPosition(x, y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * cellSize;

        return objWorldPos;
    }

    public PlaceableObjectSO GetPlaceableSO()
    {
        return placeable;
    }
    
    public PlaceableObjectSO.Dir GetRotationDir()
    {
        return dir;
    }

    public List<Vector2Int> GetPlaceablePositions(Vector3 mousePos)
    {
        // Returns a list of grid spaces the current placeable to build would take up
        grid.GetXZ(mousePos, out int x, out int y);
        List<Vector2Int> gridPosList = placeable.GetGridPositionList(new Vector2Int(x, y), dir);
        return gridPosList;
    }

    public Vector3 GetGridAsWorldPos(int x, int y)
    {
        return grid.GetWorldPosition(x, y);
    }

    public PlaceableObject GetPlaceableObject(Vector3 mousePos)
    {
        // Returns placeable object at grid space mouse is on
        grid.GetXZ(mousePos, out int a, out int b);
        GridObject gridObj = grid.GetValue(a, b);

        if (gridObj == null)
        {
            //Debug.Log("Grid space is NULL");
            return null;
        }

        return gridObj.GetPlaceableObject();
    }

    public List<Vector2Int> GetPlacedObjPositions(Vector3 mousePos)
    {
        // Returns a list of grid spaces of object mouse is on
        grid.GetXZ(mousePos, out int a, out int b);
        GridObject gridObj = grid.GetValue(a, b);

        if (gridObj == null)
            return null;
        
        PlaceableObject placeableObj = gridObj.GetPlaceableObject();
        if (placeableObj != null)
        {
            List<Vector2Int> gridPosList = placeableObj.GetGridPositionList();
            return gridPosList;
        }

        return null;
    }

    public PlaceableObject GetPlaceableObjectAtGridPos(Vector2Int pos)
    {
        GridObject gridObj = grid.GetValue((int)pos.x, (int)pos.y);

        if (gridObj == null)
        {
            //Debug.Log("Grid space is NULL");
            return null;
        }

        return gridObj.GetPlaceableObject();
    }

    public List<PlaceableObject> GetAllPlaceableObjects()
    {
        List<PlaceableObject> placeables = new List<PlaceableObject>();

        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                GridObject gridObj = grid.GetValue(i, j);

                if (gridObj == null)
                {
                    Debug.LogError("NULL grid space found when looping through all spaces");
                } else
                {
                    PlaceableObject curPlaceable = gridObj.GetPlaceableObject();
                    if (curPlaceable)
                    {
                        // Only add the placeable to the list once (when at its origin)
                        if (curPlaceable.GetGridOrigin() == new Vector2Int(i, j))
                        {
                            placeables.Add(curPlaceable);
                        }
                    }
                }
            }
        }

        return placeables;
    }

    public bool PlacementViable(Vector3 mousePos)
    {
        grid.GetXZ(mousePos, out int x, out int y);

        List<Vector2Int> gridPosList = placeable.GetGridPositionList(new Vector2Int(x, y), dir);

        return CanBuildHere(gridPosList);
    }
}
