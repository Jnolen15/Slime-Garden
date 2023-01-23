using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int money = 0;
    public int Money
    {
        get { return money; }
        set
        {
            money = value;
            csDisplay.text = "Crystalized Slime: " + money.ToString();
        }
    }

    public TextMeshProUGUI csDisplay;

    [SerializeField] private LayerMask groundLayerMask;
    private GridSystem gridSystem;

    private GameObject buildVisual;

    public enum State
    {
        Default,
        Build
    }
    public State state;

    private void Start()
    {
        gridSystem = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridSystem>();
        buildVisual = GameObject.FindGameObjectWithTag("BuildVisual");
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit mousePos, 999f, groundLayerMask))
        {
            // ================ PLACE / Demolish
            if (state == State.Build)
            {
                if (Input.GetMouseButtonDown(0))
                    gridSystem.Place(mousePos.point);

                if (Input.GetMouseButtonDown(1))
                    gridSystem.Demolish(mousePos.point);
            }

            // =================== ROTATE
            if (Input.GetKeyDown(KeyCode.R) && state == State.Build)
            {
                gridSystem.Rotate();
            }
        }

        switch (state)
        {
            case (State.Default):
                buildVisual.SetActive(false);
                break;
            case (State.Build):
                buildVisual.SetActive(true);
                break;
        }
    }

    public void ChangeState(string newState)
    {
        switch (newState)
        {
            case "Default": 
                state = State.Default;
                break;
            case "Build":
                state = State.Build;
                break;
        }
    }

    public void SwapPlaceable(PlaceableObjectSO newP)
    {
        gridSystem.SwapPlaceable(newP);
        buildVisual.GetComponent<BuildingVisual>().RefreshVisual();
    }
}
