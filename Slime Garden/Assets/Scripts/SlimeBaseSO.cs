﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SlimeBase")]
public class SlimeBaseSO : ScriptableObject
{
    public Sprite sSprite;                  // The slime's base sprite
    public Sprite sIdle;                    // The slimes Idle sprite (Switch between this and the besic sprite)
    public Sprite sJump;                    // The slimes Jump sprite
    public GameObject sParticles;           // The Particle system used by the slime
    public string sName = "Null";           // The species name of the slime
    public string sBaseColor = "Null";      // The base color of the slime
    public float sRarity;                   // How rare a slime is
    public bool sSpecial = false;           // Special is true if the slime isnt one of the 12 base colors. Like Diamond
}
