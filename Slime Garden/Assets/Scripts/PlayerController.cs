using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public int cs = 0;  // The ammount of Congealed Slime held by the player
    public TextMeshProUGUI csDisplay;

    [SerializeField] private LayerMask groundLayerMask;
    private GridSystem gridSystem;

    private GameObject buildVisual;

    public enum State
    {
        Default,
        Build,
        Demolish
    }
    public State state;

    private void Start()
    {
        gridSystem = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridSystem>();
        buildVisual = GameObject.FindGameObjectWithTag("BuildVisual");
    }

    void Update()
    {
        csDisplay.text = "Crystalized Slime: " + cs;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit mousePos, 999f, groundLayerMask))
        {
            // ================ PLACE / Demolish
            if (Input.GetMouseButtonDown(0))
            {
                if(state == State.Build)
                    gridSystem.Place(mousePos.point);
                if (state == State.Demolish)
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
            case (State.Demolish):
                buildVisual.SetActive(false);
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
            case "Demolish":
                state = State.Demolish;
                break;
        }
    }
}
