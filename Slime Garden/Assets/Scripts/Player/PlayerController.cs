using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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
            csDisplay.text = money.ToString();
        }
    }

    public TextMeshProUGUI csDisplay;
    [SerializeField] private LayerMask groundLayerMask;
    private GridSystem gridSystem;
    private GameObject buildVisual;
    [SerializeField] private bool isOverUI;
    [SerializeField] private CropSO crop;
    private MenuManager menus;

    public enum State
    {
        Default,
        Build,
        Plant
    }
    public State state;

    private void Start()
    {
        gridSystem = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridSystem>();
        buildVisual = GameObject.FindGameObjectWithTag("BuildVisual");
        buildVisual.SetActive(false);
        menus = GameObject.FindGameObjectWithTag("UIManager").GetComponent<MenuManager>();
    }

    private void Update()
    {
        // Check to see if mouse is over UI element
        // This proboably is not the best way to do this, so maybe fix later
        if (EventSystem.current.IsPointerOverGameObject())
            isOverUI = true;
        else
            isOverUI = false;
    }

    public void ChangeState(string newState)
    {
        menus.CloseAllSubmenus();
        buildVisual.SetActive(false);

        switch (newState)
        {
            case "Default": 
                state = State.Default;
                break;
            case "Build":
                state = State.Build;
                buildVisual.SetActive(true);
                menus.BuildMenuActive(true);
                break;
            case "Plant":
                state = State.Plant;
                menus.SeedMenuActive(true);
                break;
        }
    }

    public void SwapPlaceable(PlaceableObjectSO newP)
    {
        gridSystem.SwapPlaceable(newP);
        buildVisual.GetComponent<BuildingVisual>().RefreshVisual();
    }
    
    public void SwapCrop(CropSO newC)
    {
        crop = newC;
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
            if (state == State.Build && !isOverUI)
                BuyAndPlace(mousePos.point);
        }

        // Click Interactable / Plantable
        Ray objectRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(objectRay, out RaycastHit objectRayHit, 999f))
        {
            if (state == State.Default && objectRayHit.collider.gameObject.tag == "Interactable")
            {
                var interactable = objectRayHit.collider.gameObject.GetComponent<IInteractable>();
                if (interactable == null) return;

                interactable.Interact();
            }

            if (objectRayHit.collider.gameObject.tag == "Plantable")
                PlantInteraction(objectRayHit.collider.gameObject.GetComponent<PlantSpot>());
        }
    }

    public void OnSecondary(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit mousePos, 999f, groundLayerMask))
        {
            RemoveBuildable(mousePos.point);
        }
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (state == State.Build)
            gridSystem.Rotate();
    }

    // ========== ACTIONS ==========
    private bool TryPurchase(int cost)
    {
        if (Money >= cost)
        {
            Debug.Log("Purchased!");
            Money -= cost;
            return true;
        }
        else
        {
            Debug.Log("Cannot afford!");
            return false;
        }
    }

    private void BuyAndPlace(Vector3 pos)
    {
        var so = gridSystem.GetPlaceableSO();
        
        if (TryPurchase(so.price))
            gridSystem.Place(pos);
    }

    private void PlantInteraction(PlantSpot plantSpot)
    {
        if (plantSpot == null) return;

        // Plant in an empty plot
        if (state == State.Plant)
        {
            if (plantSpot.GetCropSO() == null)
            {
                if(TryPurchase(crop.price))
                    plantSpot.Plant(crop);
            }
            else
                Debug.Log("spot already has plant: " + plantSpot.GetCropSO().cropName);
        }

        // Water or harvest a planted crop
        else if (state == State.Default)
        {
            plantSpot.Interact();
        }
    }

    private void RemoveBuildable(Vector3 pos)
    {
        if (state == State.Build && !isOverUI)
        {
            if (gridSystem.GetPlaceableObject(pos) == null)
                return;

            // Remove garden refrence
            var ps = gridSystem.GetPlaceableObject(pos).GetComponentsInChildren<PlantSpot>();
            foreach (PlantSpot spot in ps)
            {
                Debug.Log("Getting rid of garden refrence");
                spot.DestroySelf();
            }

            gridSystem.Demolish(pos);
        }
    }
}
