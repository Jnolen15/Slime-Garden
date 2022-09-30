using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[CreateAssetMenu(menuName = "SlimePattern")]
public class SlimePatternSO : ScriptableObject
{
    public Sprite sPatternSprite;           // The slime's base sprite
    public Sprite sPatternIdle;             // The slimes Idle sprite (Switch between this and the besic sprite)
    public Sprite sPatternJump;             // The slimes Jump sprite
    public SpriteLibraryAsset libraryAsset; // The slimes library of sprites
    public string sPattern = "Null";        // The base color of the slime
    public float sRarity;                   // How rare a slime is
}
