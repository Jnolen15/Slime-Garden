using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    public bool holding;
    public GameObject heldSlime;

    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask slimeLayerMask;
    [SerializeField] private GameObject mouseVisual;
    private float pickupOffsetX;
    private float pickupOffsetZ;

    void Update()
    {
        // Mouse position in world space
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit mousePos, 999f, groundLayerMask))
        {
            mouseVisual.transform.position = mousePos.point;

            if (holding)
                SlimeHeld(mousePos.point);
        }

        // Mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Ray clickray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(clickray, out RaycastHit clickraycastHit, 999f, slimeLayerMask))
            {
                // Pick up slime of clicked
                if (clickraycastHit.collider.gameObject.tag == "Slime")
                {
                    SlimePickUp(clickraycastHit.collider.gameObject, clickraycastHit.point);
                }

            }
        }

        // Let go of a slime if player was holding one
        if (Input.GetMouseButtonUp(0) && holding)
        {
            SlimeDropped();
        }
    }

    private void SlimePickUp(GameObject slime, Vector3 mousePos)
    {
        heldSlime = slime;
        holding = true;
        heldSlime.GetComponent<DragDrop>().PickedUp();

        pickupOffsetX = mousePos.x - heldSlime.transform.position.x;
        pickupOffsetZ = mousePos.z - heldSlime.transform.position.z;
    }

    private void SlimeHeld(Vector3 mousePos)
    {
        heldSlime.transform.position = new Vector3(mousePos.x - pickupOffsetX, 0, mousePos.z - pickupOffsetZ);
    }

    private void SlimeDropped()
    {
        heldSlime.GetComponent<DragDrop>().LetGo();
        holding = false;
        heldSlime = null;
    }
}
