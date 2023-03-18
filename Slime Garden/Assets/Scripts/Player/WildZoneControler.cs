using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class WildZoneControler : MonoBehaviour
{
    // For now this script is basically jus a trimmed down version of the PlayerControler.
    // This will change and it will becomre more unique as the Wildzone mechanics are further developed.
    // But for now its being kept fairly basic to test the idea.

    // Variables
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask slimeLayerMask;
    private CropSO crop;
    private bool isOverUI;
    private bool inspectingSlime;

    // Script/Object Refrences
    private PlayerInput pInput;
    private MenuManager menus;
    private CameraControl camcontrol;
    private InventoryManager invManager;

    // Wild Player State enum
    public enum State
    {
        Default,
        Crops,
        Inspect
    }
    public State state;

    void Start()
    {
        pInput = this.GetComponentInParent<PlayerInput>();
        invManager = this.GetComponent<InventoryManager>();

        menus = GameObject.FindGameObjectWithTag("UIManager").GetComponent<MenuManager>();
        camcontrol = GameObject.FindGameObjectWithTag("CamControl").GetComponent<CameraControl>();
    }

    void Update()
    {
        // Check to see if mouse is over UI element
        // This proboably is not the best way to do this, so maybe fix later
        if (EventSystem.current.IsPointerOverGameObject())
            isOverUI = true;
        else
            isOverUI = false;

        // If inspect state is changed
        if (inspectingSlime && state != State.Inspect)
        {
            StopInspectSlime();
            camcontrol.EndFollowSlime();
        }
    }

    // ========== STATE MANAGEMENT ==========
    public void ChangeState(string newState)
    {
        menus.CloseAllSubmenus();

        switch (newState)
        {
            case "Default":
                state = State.Default;
                break;
            case "Crops":
                state = State.Crops;
                menus.CropMenuActive(true);
                break;
        }
    }

    // ========== CONTROLS ==========
    public void OnPrimary(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (isOverUI)
            return;

        // Click Ground
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit mousePos, 999f, groundLayerMask))
        {
            if (state == State.Crops)
                SpawnCrop(mousePos.point);
        }

        // Click Interactable
        Ray objectRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(objectRay, out RaycastHit objectRayHit, 999f))
        {
            GameObject colObj = objectRayHit.collider.gameObject;
            if (state == State.Default && (colObj.tag == "Interactable" || colObj.tag == "Crop"))
            {
                var interactable = colObj.GetComponent<IInteractable>();
                if (interactable == null) return;

                interactable.Interact();
            }
        }

        // Click slime
        Ray clickray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(clickray, out RaycastHit clickraycastHit, 999f, slimeLayerMask))
        {
            // Do Something?
        }
    }

    public void OnSecondary(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (isOverUI)
            return;

        // Click slime
        Ray clickray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(clickray, out RaycastHit clickraycastHit, 999f, slimeLayerMask))
        {
            // Inspect Slime Popup
            if (state == State.Default && clickraycastHit.collider.gameObject.tag == "Slime")
            {
                var clickedSlime = clickraycastHit.collider.gameObject.GetComponent<SlimeData>();
                InspectSlime(clickedSlime);
            }
        }
    }


    // ========== ACTIONS ==========
    private void InspectSlime(SlimeData slime)
    {
        state = State.Inspect;
        inspectingSlime = true;
        menus.ShowSlimeStats(slime);
        camcontrol.FollowSlime(slime.gameObject.transform);
    }

    public void StopInspectSlime()
    {
        state = State.Default;
        inspectingSlime = false;
        menus.CloseSlimeStats();
    }

    public void SwapCrop(CropSO newC)
    {
        crop = newC;
    }

    private void SpawnCrop(Vector3 pos)
    {
        if (0 < invManager.GetNumHeld(crop))
        {
            invManager.AddCrop(crop, -1);
            menus.UpdateCropCount();
            Vector3 spawnPos = new Vector3(pos.x, pos.y + 2, pos.z);
            var instCrop = Instantiate(crop.cropObj, spawnPos, Quaternion.identity);
            instCrop.GetComponent<CropObj>().Setup(crop);
        }
    }
}
