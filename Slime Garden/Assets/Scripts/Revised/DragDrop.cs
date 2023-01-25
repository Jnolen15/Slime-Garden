using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public SlimeController sc;
    public bool isHeld = false;

    public void PickedUp()
    {
        isHeld = true;
        sc.ChangeState(SlimeController.State.held);
    }

    public void LetGo()
    {
        isHeld = false;
        sc.ChangeState(SlimeController.State.idle);
    }

    public void SlimeHeld(Vector3 mousePos)
    {
        this.transform.position = new Vector3(mousePos.x, 0.2f, mousePos.z);
    }
}
