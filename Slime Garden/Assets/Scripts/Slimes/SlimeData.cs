using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class SlimeData : MonoBehaviour
{
    //=============== SLIME ATTRIBUTES ===============
    public string speciesName;                  //Name of slime species
    public string displayName;                  //Name of slime species
    public float sRarity;                       //Rarity of slime
    public SlimeBaseSO slimeSpeciesBase;        //The SO of this slime species Base color. Info is taken from here.
    public string sBaseColor;                   //Base color of species
    public Color slimeSpeciesBaseColor;         //The Color that will be applied to the Base
    public string sPattern;                     //The Pattern
    public SlimePatternSO slimeSpeciesPattern;  //The SO of this slime species. Info is taken from here.
    public string sPatternColor;                //Pattern color of species
    public Color slimeSpeciesPatternColor;      //The Color that will be applied to the Pattern
    public SlimeFaceSO slimeFace;               //The SO with slime facial expressions. Info is taken from here.
    public GameObject sCrystal;                 //The crystal Game object this slime produces
    [Header("Mark slime as wild")]
    public bool isWild;       // Marks it as a wild slime wich changes some behavior

    //=============== COMPONENTS ===============
    private HabitatControl hControl;
    private GameObject sprites;               //The child gameobject that holds sprite objects
    private GameObject baseSlime;               //The child gameobject containing the base SR
    private GameObject pattern;                 //The child gameobject containing the pattern SR
    private GameObject face;                    //The face rendered above the slime
    private SpriteRenderer Basesr;              //The SpriteRenderer of the base child object
    private SpriteRenderer patternsr;           //The SpriteRenderer of the pattern child object

    public void Setup()
    {
        // Create name and calculate rarity
        sPattern = slimeSpeciesPattern.sPattern;
        speciesName = sBaseColor + " " + sPatternColor + " " + sPattern;
        displayName = speciesName;
        sRarity = slimeSpeciesBase.sRarity + slimeSpeciesPattern.sRarity;
        // ========== Sprite stuff ==========
        sprites = this.transform.GetChild(0).gameObject;
        baseSlime = sprites.transform.GetChild(0).gameObject;
        pattern = sprites.transform.GetChild(1).gameObject;
        face = sprites.transform.GetChild(2).gameObject;
        // Sprite renderers
        Basesr = baseSlime.GetComponent<SpriteRenderer>();
        patternsr = pattern.GetComponent<SpriteRenderer>();
        // Sprite library assets (Used to swap out sprites in animations)
        baseSlime.GetComponent<SpriteLibrary>().spriteLibraryAsset = slimeSpeciesBase.libraryAsset;
        pattern.GetComponent<SpriteLibrary>().spriteLibraryAsset = slimeSpeciesPattern.libraryAsset;
        face.GetComponent<SpriteLibrary>().spriteLibraryAsset = slimeFace.libraryAsset;
        // Assign Colors
        Basesr.color = slimeSpeciesBaseColor;
        patternsr.color = slimeSpeciesPatternColor;

        // Add slime to habitat list if not wild
        if (!isWild)
        {
            hControl = GameObject.FindGameObjectWithTag("GameManager").GetComponent<HabitatControl>();
            hControl.activeSlimes.Add(gameObject);
        }
    }

    public void SetName(string newName)
    {
        displayName = newName;
        Debug.Log("Display name changed: " + displayName);
    }
}
