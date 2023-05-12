using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseControl : MonoBehaviour
{
    public bool holding;
    public GameObject heldSlime;
    public DragDrop heldSlimeDragDrop;
    private GameObject potentialSlime;

    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask slimeLayerMask;
    [SerializeField] private GameObject mouseVisual;

    private PlayerController pc;
    private CursorVisual cursorVis;

    private void Start()
    {
        pc = this.GetComponent<PlayerController>();
        cursorVis = this.GetComponent<CursorVisual>();
    }

    void Update()
    {
        // If slime held becomes false drop it
        if (holding)
        {
            if (!heldSlimeDragDrop.isHeld)
            {
                SlimeDropped();
            }
        }

        // CURSOR VISUAL SWAPPING
        // DEFAULT
        if (pc.state == PlayerController.State.Default)
        {
            if (holding)
            {
                cursorVis.UpdateCursor("close");
            }
            else
            {
                // Cursor swapping
                Ray clickray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(clickray, out RaycastHit clickraycastHit, 999f, slimeLayerMask))
                {
                    if (clickraycastHit.collider.gameObject.tag == "Slime")
                        cursorVis.UpdateCursor("open");
                }
                else
                    cursorVis.UpdateCursor("point");
            }
        }
        // BUILD
        if (pc.state == PlayerController.State.Build)
        {
            cursorVis.UpdateCursor("point");
        }
        // PLANT
        if (pc.state == PlayerController.State.Plant)
        {
            cursorVis.UpdateCursor("point");
        }
        // PAINT
        if (pc.state == PlayerController.State.Paint)
        {
            cursorVis.UpdateCursor("paint");
        }


        // Mouse position in world space
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit mousePos, 999f, groundLayerMask))
        {
            mouseVisual.transform.position = mousePos.point;

            if (holding)
                heldSlimeDragDrop.SlimeHeld(mousePos.point);
        }
    }

    private void SlimePickUp(GameObject slime)
    {
        heldSlime = slime;
        holding = true;
        heldSlimeDragDrop = heldSlime.GetComponent<DragDrop>();
        heldSlimeDragDrop.PickedUp();
    }

    private void SlimeDropped()
    {
        if(heldSlime != null)
            heldSlimeDragDrop.LetGo();
        
        holding = false;
        heldSlime = null;
        heldSlimeDragDrop = null;
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
