using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SlimePattern")]
public class SlimePatternSO : ScriptableObject
{
    public Sprite sPatternSprite;           // The slime's base sprite
    public Sprite sPatternIdle;             // The slimes Idle sprite (Switch between this and the besic sprite)
    public Sprite sPatternJump;             // The slimes Jump sprite
    public string sPattern = "Null";        // The base color of the slime
    public float sRarity;                   // How rare a slime is
}
