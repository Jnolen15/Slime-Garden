using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SlimeFace")]

public class SlimeFaceSO : ScriptableObject
{
    public Sprite faceSprite;                  // The slime's base face sprite
    public Sprite faceIdle;                    // The slimes Idle face sprite (Switch between this and the besic sprite)
    public Sprite faceJump;                    // The slimes Jump face sprite
}
