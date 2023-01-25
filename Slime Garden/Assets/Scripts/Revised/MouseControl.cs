using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseControl : MonoBehaviour
{
    public bool holding;
    public GameObject heldSlime;
    [SerializeField] private GameObject potentialSlime;

    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask slimeLayerMask;
    [SerializeField] private LayerMask borderLayerMask;
    [SerializeField] private GameObject mouseVisual;

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
                heldSlime.GetComponent<DragDrop>().SlimeHeld(mousePos.point);
        }
    }

    private void SlimePickUp(GameObject slime)
    {
        heldSlime = slime;
        holding = true;
        heldSlime.GetComponent<DragDrop>().PickedUp();
    }

    private void SlimeDropped()
    {
        if(heldSlime != null)
            heldSlime.GetComponent<DragDrop>().LetGo();
        
        holding = false;
        heldSlime = null;
        potentialSlime = null;
    }


    // ========== CONTROLS ==========
    public void OnPrimaryHold(InputAction.CallbackContext context)
    {
        // Start hold on slime
        if (context.started)
        {
            if (pc.state == PlayerController.State.Default)
            {
                Ray clickray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(clickray, out RaycastHit clickraycastHit, 999f, slimeLayerMask))
                {
                    // capture potential slime
                    if (clickraycastHit.collider.gameObject.tag == "Slime")
                        potentialSlime = clickraycastHit.collider.gameObject;
                }
            }
        }
        // Execute hold (Start pickup)
        else if (context.performed)
        {
            if(potentialSlime != null)
                SlimePickUp(potentialSlime);
        }
        // Cancel hold (let go)
        else if (context.canceled)
        {
            SlimeDropped();
        }
    }
}
