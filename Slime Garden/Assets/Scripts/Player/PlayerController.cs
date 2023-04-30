using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Variables
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask slimeLayerMask;
    private CropSO crop;
    private bool isOverUI;
    private bool inspectingSlime;
    private bool dragBuild;
    private bool dragDestroy;
    private bool inShop;

    // Script/Object Refrences
    private PlayerInput pInput;
    private PlayerData pData;
    private InventoryManager invManager;
    private MenuManager menus;
    private CameraControl camcontrol;
    private GridSystem gridSystem;
    private GameObject buildVisual;
    private SlimeController curInspectingSlime;
    private AudioSource audioSrc;
    [SerializeField] private AudioClip[] paintSounds;

    // Player State enum
    public enum State
    {
        Default,
        Build,
        Plant,
        Inspect,
        Paint,
        InMenus
    }
    public State state;


    private void Start()
    {
        pInput = this.GetComponentInParent<PlayerInput>();
        pData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        invManager = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<InventoryManager>();

        menus = GameObject.FindGameObjectWithTag("UIManager").GetComponent<MenuManager>();
        camcontrol = GameObject.FindGameObjectWithTag("CamControl").GetComponent<CameraControl>();
        gridSystem = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridSystem>();
        buildVisual = GameObject.FindGameObjectWithTag("BuildVisual");
        buildVisual.SetActive(false);

        audioSrc = this.GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Check to see if mouse is over UI element
        // This proboably is not the best way to do this, so maybe fix later
        if (EventSystem.current.IsPointerOverGameObject())
            isOverUI = true;
        else
            isOverUI = false;

        // Stop inspecting edge cases
        if (inspectingSlime)
        {
            // If inspect state is changed
            if (state != State.Inspect)
            {
                StopInspectSlime();
                return;
            }

            // If inspecting slime becomes tamed
            if (curInspectingSlime.GetState() == SlimeController.State.tamed)
                StopInspectSlime();
        }

        // Close shop edge cases
        if (inShop && state != State.InMenus)
            CloseShop();

        // Swap action maps if in an input field
        GameObject currentFocus = EventSystem.current.currentSelectedGameObject;
        if (currentFocus != null)
        {
            if (currentFocus.TryGetComponent(out TMP_InputField _) && pInput.currentActionMap.name != "InputField")
                pInput.SwitchCurrentActionMap("InputField");
        }
        else if (pInput.currentActionMap.name != "Basic")
            pInput.SwitchCurrentActionMap("Basic");

        // Deactivate build visual if over UI
        if (state == State.Build)
        {
            if (MouseOverUI() && buildVisual.activeSelf)
                buildVisual.SetActive(false);
            else if (!MouseOverUI() && !buildVisual.activeSelf)
                buildVisual.SetActive(true);
        }

        // Drag build
        if (state == State.Build && dragBuild)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit mousePos, 999f, groundLayerMask))
                BuyAndPlace(mousePos.point);
        }
        
        // Drag destroy
        if (state == State.Build && dragDestroy)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit mousePos, 999f, groundLayerMask))
                RemoveBuildable(mousePos.point);
        }
    }

    // ========== STATE MANAGEMENT ==========
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
                gridSystem.SwapPlaceable(invManager.availablePlaceables[0]);
                buildVisual.GetComponent<BuildingVisual>().SetMode("Place");
                buildVisual.GetComponent<BuildingVisual>().RefreshVisual();
                buildVisual.SetActive(true);
                menus.BuildMenuActive(true);
                break;
            case "Inspect":
                state = State.Inspect;
                break;
            case "Plant":
                state = State.Plant;
                menus.SeedMenuActive(true);
                break;
            case "Paint":
                buildVisual.GetComponent<BuildingVisual>().SetMode("Select");
                buildVisual.SetActive(true);
                state = State.Paint;
                break;
            case "InMenus":
                state = State.InMenus;
                break;
        }
    }

    // ========== SO MANAGEMENT ==========
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

        if(MouseOverUI())
            return;

        // Click Ground
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit mousePos, 999f, groundLayerMask))
        {
            if (state == State.Build)
                BuyAndPlace(mousePos.point);

            if (state == State.Paint)
                Paint(mousePos.point);
        }

        // Click Interactable / Plantable / Shop
        Ray objectRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(objectRay, out RaycastHit objectRayHit, 999f))
        {
            GameObject colObj = objectRayHit.collider.gameObject;
            if (state == State.Default && (colObj.tag == "Interactable" || colObj.tag == "Crop" || colObj.tag == "Shop"))
            {
                var interactable = colObj.GetComponent<IInteractable>();
                if (interactable == null) return;

                if (colObj.tag == "Shop")
                    OpenShop(colObj.transform);

                interactable.Interact();
            }

            if ((state == State.Plant || state == State.Default) && colObj.tag == "Plantable")
                PlantInteraction(colObj.GetComponent<PlantSpot>());
        }

        // Click slime
        Ray clickray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(clickray, out RaycastHit clickraycastHit, 999f, slimeLayerMask))
        {
            // Pet Slime
            if (state == State.Inspect && clickraycastHit.collider.gameObject.tag == "Slime")
            {
                var clickedSlime = clickraycastHit.collider.gameObject.GetComponent<SlimeController>();

                if (clickedSlime.InNonInteruptableState())
                    return;

                clickedSlime.ChangeState(SlimeController.State.pet);
                Debug.Log("Pet Slime");
            }
        }
    }

    public void OnSecondary(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (MouseOverUI())
            return;

        // Click buildable
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit mousePos, 999f, groundLayerMask))
        {
            if (state == State.Build)
                RemoveBuildable(mousePos.point);
        }

        // Click slime
        Ray clickray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(clickray, out RaycastHit clickraycastHit, 999f, slimeLayerMask))
        {
            // Inspect Slime Popup
            if (state == State.Default && clickraycastHit.collider.gameObject.tag == "Slime")
            {
                var clickedSlime = clickraycastHit.collider.gameObject;
                InspectSlime(clickedSlime);
            }
        }
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (state == State.Build)
            gridSystem.Rotate();
    }

    public void OnPrimaryHold(InputAction.CallbackContext context)
    {
        // Execute hold (Start pickup)
        if (context.performed)
        {
            if (state == State.Build)
            {
                dragBuild = true;
            }

            Debug.Log("Started primary hold");
        }
        // Cancel hold (let go)
        else if (context.canceled)
        {
            dragBuild = false;
        }

    }

    public void OnSecondaryHold(InputAction.CallbackContext context)
    {
        // Execute hold (Start pickup)
        if (context.performed)
        {
            if (state == State.Build)
            {
                dragDestroy = true;
            }

            Debug.Log("Started secondary hold");
        }
        // Cancel hold (let go)
        else if (context.canceled)
        {
            dragDestroy = false;
        }

    }

    // ========== ACTIONS ==========
    private void BuyAndPlace(Vector3 pos)
    {
        var so = gridSystem.GetPlaceableSO();
        bool placed = false;

        if (pData.CanAfford(so.price))
            placed = gridSystem.Place(pos);

        if (placed)
            pData.MakePurchase(so.price);
    }

    private void Paint(Vector3 pos)
    {
        bool painted = false;

        painted = gridSystem.Paint(pos);

        if (painted)
        {
            audioSrc.PlayOneShot(paintSounds[Random.Range(0, paintSounds.Length)]);
            Debug.Log("Painted");
        }
        else
            Debug.Log("Nothing to Paint");
    }

    private void PlantInteraction(PlantSpot plantSpot)
    {
        if (plantSpot == null) return;

        // Plant in an empty plot
        if (state == State.Plant)
        {
            if (crop == null)
                return;

            if (plantSpot.GetCropSO() == null)
            {
                if(pData.TryAndPurchase(crop.price))
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

    private void InspectSlime(GameObject slime)
    {
        ChangeState("Inspect");
        inspectingSlime = true;
        curInspectingSlime = slime.GetComponent<SlimeController>();
        menus.ShowSlimeStats(slime);
        camcontrol.FollowObject(slime.gameObject.transform, "slime");
    }

    public void StopInspectSlime()
    {
        if (!inspectingSlime)
            return;

        ChangeState("Default");
        inspectingSlime = false;
        curInspectingSlime = null;
        menus.CloseSlimeStats();
        camcontrol.EndFollowObject();
    }

    private void OpenShop(Transform shop)
    {
        ChangeState("InMenus");
        inShop = true;
        //menus.ShowCropSell();
        camcontrol.FollowObject(shop, "shop");
    }

    public void CloseShop()
    {
        if (!inShop)
            return;

        ChangeState("Default");
        inShop = false;
        menus.CloseAllShops();
        //menus.CloseCropSell();
        camcontrol.EndFollowObject();
    }

    // ========== MISC ==========
    public bool MouseOverUI()
    {
        return isOverUI;
    }
}
