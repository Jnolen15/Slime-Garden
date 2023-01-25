using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
        buildVisual.SetActive(false);
    }

    public void ChangeState(string newState)
    {
        switch (newState)
        {
            case "Default": 
                state = State.Default;
                buildVisual.SetActive(false);
                break;
            case "Build":
                state = State.Build;
                buildVisual.SetActive(true);
                break;
        }
    }

    public void SwapPlaceable(PlaceableObjectSO newP)
    {
        gridSystem.SwapPlaceable(newP);
        buildVisual.GetComponent<BuildingVisual>().RefreshVisual();
    }

    // ========== CONTROLS ==========
    public void OnPrimary(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        // Click Ground
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit mousePos, 999f, groundLayerMask))
        {
            if (state == State.Build)
                gridSystem.Place(mousePos.point);
        }

        // Click Other
        Ray objectRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(objectRay, out RaycastHit objectRayHit, 999f))
        {
            if (objectRayHit.collider.gameObject.tag == "Interactable")
            {
                var interactable = objectRayHit.collider.gameObject.GetComponent<IInteractable>();
                if (interactable == null) return;

                interactable.Interact();
            }
        }
    }

    public void OnSecondary(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit mousePos, 999f, groundLayerMask))
        {
            if (state == State.Build)
                gridSystem.Demolish(mousePos.point);
        }
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (state == State.Build)
            gridSystem.Rotate();
    }
}
