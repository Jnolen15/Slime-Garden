using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseControl : MonoBehaviour
{
    public bool holding;
    public GameObject heldSlime;

    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask slimeLayerMask;
    [SerializeField] private LayerMask borderLayerMask;
    [SerializeField] private GameObject mouseVisual;
    private float pickupOffsetX;

    private PlayerController pc;

    private void Start()
    {
        pc = this.GetComponent<PlayerController>();
    }

    void Update()
    {
        // border detection
        if (holding)
        {
            Ray borderRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(borderRay, out RaycastHit borderRaycastHit, 999f, borderLayerMask))
            {
                SlimeDropped();
            }
        }

        // Mouse position in world space
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit mousePos, 999f, groundLayerMask))
        {
            mouseVisual.transform.position = mousePos.point;

            if (holding)
                heldSlime.GetComponent<DragDrop>().SlimeHeld(mousePos.point, pickupOffsetX);
        }
    }

    private void SlimePickUp(GameObject slime, Vector3 mousePos)
    {
        heldSlime = slime;
        holding = true;
        heldSlime.GetComponent<DragDrop>().PickedUp();

        pickupOffsetX = mousePos.x - heldSlime.transform.position.x;
    }

    private void SlimeDropped()
    {
        heldSlime.GetComponent<DragDrop>().LetGo();
        holding = false;
        heldSlime = null;
    }

    // ========== CONTROLS ==========

    public void OnPrimaryHold(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (pc.state == PlayerController.State.Default)
            {
                Ray clickray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(clickray, out RaycastHit clickraycastHit, 999f, slimeLayerMask))
                {
                    // Pick up slime of clicked
                    if (clickraycastHit.collider.gameObject.tag == "Slime")
                        SlimePickUp(clickraycastHit.collider.gameObject, clickraycastHit.point);
                }
            }
        }
        else if (context.canceled)
        {
            if (Input.GetMouseButtonUp(0) && holding)
            {
                SlimeDropped();
            }
        }
    }
}
