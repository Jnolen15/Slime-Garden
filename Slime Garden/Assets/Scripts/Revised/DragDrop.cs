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

}
