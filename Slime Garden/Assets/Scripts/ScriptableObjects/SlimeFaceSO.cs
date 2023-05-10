using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[CreateAssetMenu(menuName = "SlimeSOs/SlimeFace")]

public class SlimeFaceSO : ScriptableObject
{
    public SpriteLibraryAsset libraryAsset; // The slimes library of sprites
    public SpriteLibraryAsset babyLibraryAsset; // The slimes library of baby sprites
}
