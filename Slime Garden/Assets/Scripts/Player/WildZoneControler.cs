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
    private bool isOverUI;
    private bool inspectingSlime;

    // Script/Object Refrences
    private PlayerInput pInput;
    private MenuManager menus;
    private CameraControl camcontrol;
    private InventoryManager invManager;
    [SerializeField] private SlimeController curInspectingSlime;

    // Wild Player State enum
    public enum State
    {
        Default,
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


        if (inspectingSlime)
        {
            // If inspect state is changed
            if (state != State.Inspect)
            {
                StopInspectSlime();
            }

            // If inspecting slime becomes tamed
            if (curInspectingSlime.GetState() == SlimeController.State.tamed)
            {
                StopInspectSlime();
            }
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
        }
    }

    // ========== CONTROLS ==========
    public void OnPrimary(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (isOverUI)
            return;

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
                var clickedSlime = clickraycastHit.collider.gameObject;
                InspectSlime(clickedSlime);
            }
        }
    }


    // ========== ACTIONS ==========
    private void InspectSlime(GameObject slime)
    {
        state = State.Inspect;
        inspectingSlime = true;
        curInspectingSlime = slime.GetComponent<SlimeController>();
        menus.ShowSlimeStats(slime);
        camcontrol.FollowSlime(slime.gameObject.transform);
    }

    public void StopInspectSlime()
    {
        if (!inspectingSlime)
            return;

        Debug.Log("Stop inspecting slime");
        state = State.Default;
        inspectingSlime = false;
        curInspectingSlime = null;
        menus.CloseSlimeStats();
        camcontrol.EndFollowSlime();
    }
}
