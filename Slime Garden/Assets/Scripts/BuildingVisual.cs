using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingVisual : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayerMask;
    private Transform visual;
    private GridSystem gridSystem;
    private PlaceableObjectSO placeableSO;

    private void Start()
    {
        visual = transform.GetChild(0);
        gridSystem = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridSystem>();

        RefreshVisual();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit mousePos, 999f, groundLayerMask))
        {
            Vector3 targetpos = gridSystem.GetSnappedWorldPos(mousePos.point);

            transform.position = targetpos;
            transform.rotation = Quaternion.Euler(0, placeableSO.GetRotationAngle(gridSystem.GetRotationDir()), 0);
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
            visual = Instantiate(placeableSO.prefab, Vector3.zero, Quaternion.identity);
            visual.parent = transform;
            visual.transform.position = transform.position;
            visual.transform.rotation = transform.rotation;
        }
    }
}
