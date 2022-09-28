using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 10;
    [SerializeField] private int cellSize = 1;
    [SerializeField] private Vector3 offsett = Vector3.zero;
    [SerializeField] private Transform testTransform;
    private WorldGrid<GridObject> grid;

    private void Awake()
    {
        grid = new WorldGrid<GridObject>(gridWidth, gridHeight, cellSize, offsett,
                                        (WorldGrid<GridObject> g, int x, int y) => new GridObject(g, x, y));
    }

    // Grid object class. Holds information about the object that is set at each grid position
    public class GridObject
    {
        private WorldGrid<GridObject> grid;
        private int x;
        private int y;
        private Transform transform;

        public GridObject(WorldGrid<GridObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetTransform(Transform transform)
        {
            this.transform = transform;
        }

        public void ClearTransform()
        {
            transform = null;
        }

        public bool CanBuild()
        {
            return transform == null;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.GetXY(mousePos, out int x, out int y);

            GridObject gridObject = grid.GetValue(x, y);
            if (gridObject != null && gridObject.CanBuild())
            {
                Transform builtObj = Instantiate(testTransform, grid.GetWorldPosition(x, y), Quaternion.identity);
                gridObject.SetTransform(builtObj);
            }
            else
            {
                Debug.Log("Can't build here!");
            }
        }
    }
}
