using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDropCheck : MonoBehaviour
{
    public bool collision;
    public SpriteRenderer groundSprite;

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
            return;

        collision = true;
        groundSprite.color = Color.red;
    }

    private void OnTriggerExit(Collider other)
    {
        collision = false;
        groundSprite.color = Color.green;
    }
}
