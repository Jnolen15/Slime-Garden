using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseControl : MonoBehaviour
{
    public bool holding;
    public GameObject heldSlime;
    private DragDrop heldSlimeDragDrop;
    private GameObject potentialSlime;

    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask slimeLayerMask;
    [SerializeField] private GameObject mousePoint;

    [SerializeField] private SlimeDropCheck slimeDropcheck;
    private PlayerController pc;
    private CursorVisual cursorVis;

    private void Start()
    {
        pc = this.GetComponent<PlayerController>();
        cursorVis = this.GetComponent<CursorVisual>();
    }

    void Update()
    {
        CursorStyle();

        // Mouse position in world space (Hitbox)
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit mousePos, 999f, groundLayerMask))
        {
            mousePoint.transform.position = mousePos.point;
        }
    }

    private void SlimePickUp(GameObject slime)
    {
        heldSlime = slime;
        holding = true;
        heldSlimeDragDrop = heldSlime.GetComponent<DragDrop>();
        heldSlimeDragDrop.PickedUp();
        slimeDropcheck.Activate();
    }

    private void SlimeDropped()
    {
        if (heldSlime != null)
        {
            // TP slime to mouse location if check passes
            if (!slimeDropcheck.collision)
            {
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(ray, out RaycastHit mousePos, 999f, groundLayerMask))
                    heldSlimeDragDrop.LetGo(mousePos.point);
            }
            else
                heldSlimeDragDrop.LetGo();
        }

        holding = false;
        heldSlime = null;
        heldSlimeDragDrop = null;
        potentialSlime = null;
        slimeDropcheck.Deactivate();
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


    // ========== CURSOR VISUAL SWAPPING ==========
    private void CursorStyle()
    {
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
    }
}
