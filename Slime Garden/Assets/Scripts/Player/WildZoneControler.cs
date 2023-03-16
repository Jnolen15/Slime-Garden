using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class WildZoneControler : MonoBehaviour
{
    // Variables
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask slimeLayerMask;
    private bool isOverUI;
    private bool inspectingSlime;

    // Script/Object Refrences
    private PlayerInput pInput;
    private MenuManager menus;
    private CameraControl camcontrol;


    void Start()
    {
        pInput = this.GetComponentInParent<PlayerInput>();
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
            // DO SOMETHING?
        }

        // Click Interactable
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
            if (clickraycastHit.collider.gameObject.tag == "Slime")
            {
                var clickedSlime = clickraycastHit.collider.gameObject.GetComponent<SlimeData>();
                InspectSlime(clickedSlime);
            }
        }
    }


    // ========== ACTIONS ==========
    private void InspectSlime(SlimeData slime)
    {
        //state = State.Inspect;
        inspectingSlime = true;
        menus.ShowSlimeStats(slime);
        camcontrol.FollowSlime(slime.gameObject.transform);
    }

    public void StopInspectSlime()
    {
        //state = State.Default;
        inspectingSlime = false;
        menus.CloseSlimeStats();
    }
}
