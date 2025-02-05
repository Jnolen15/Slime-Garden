﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "SlimeSOs/SlimePattern")]
[InlineEditor]
public class SlimePatternSO : ScriptableObject
{
    public SpriteLibraryAsset libraryAsset; // The slimes library of sprites
    public SpriteLibraryAsset babyLibraryAsset; // The slimes library of baby sprites
    public string sPattern = "Null";        // The base color of the slime
    public float sRarity;                   // How rare a slime is
}
