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

    [Header("Maturity")]
    public bool isMature;
    public float infancyTimer;
    [SerializeField]private float maturityTime;
    [SerializeField]private float infancyHungerReduction;

    [Header("Hunger")]
    public int hungerLevel;

    [Header("Mark slime as wild")]
    public bool isWild;       // Marks it as a wild slime wich changes some behavior

    //=============== COMPONENTS ===============
    private HabitatControl hControl;
    private GameObject sprites;                 //The child gameobject that holds sprite objects
    private GameObject baseSlime;               //The child gameobject containing the base SR
    private GameObject pattern;                 //The child gameobject containing the pattern SR
    private GameObject face;                    //The face rendered above the slime
    private SpriteRenderer Basesr;              //The SpriteRenderer of the base child object
    private SpriteRenderer patternsr;           //The SpriteRenderer of the pattern child object

    public void Setup(bool isInfant, bool newInfant)
    {
        // Create name and calculate rarity
        sPattern = slimeSpeciesPattern.sPattern;
        speciesName = sBaseColor + " " + sPatternColor + " " + sPattern;
        displayName = sBaseColor + " Slime";
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
        if (isInfant)
        {
            baseSlime.GetComponent<SpriteLibrary>().spriteLibraryAsset = slimeSpeciesBase.babyLibraryAsset;
            pattern.GetComponent<SpriteLibrary>().spriteLibraryAsset = slimeSpeciesPattern.babyLibraryAsset;
            face.GetComponent<SpriteLibrary>().spriteLibraryAsset = slimeFace.babyLibraryAsset;
        } else
        {
            isMature = true;
            baseSlime.GetComponent<SpriteLibrary>().spriteLibraryAsset = slimeSpeciesBase.libraryAsset;
            pattern.GetComponent<SpriteLibrary>().spriteLibraryAsset = slimeSpeciesPattern.libraryAsset;
            face.GetComponent<SpriteLibrary>().spriteLibraryAsset = slimeFace.libraryAsset;
        }
        // Assign Colors
        Basesr.color = slimeSpeciesBaseColor;
        patternsr.color = slimeSpeciesPatternColor;

        // Set infancy Timer
        if (newInfant)
            infancyTimer = maturityTime;

        if (!isMature)
            infancyHungerReduction = maturityTime * (hungerLevel * 0.0025f);

        // Add slime to habitat list if not wild
        if (!isWild)
        {
            hControl = GameObject.FindGameObjectWithTag("GameManager").GetComponent<HabitatControl>();
            hControl.activeSlimes.Add(gameObject);
        }
    }

    // Slime Aging
    private void Update()
    {
        if (isMature)
            return;

        if (infancyTimer >= infancyHungerReduction)
            infancyTimer -= Time.deltaTime;
        else
            Mature();
    }

    public void Mature()
    {
        isMature = true;
        infancyTimer = 0;
        LooseHunger(50);

        // Set mature sprites
        baseSlime.GetComponent<SpriteLibrary>().spriteLibraryAsset = slimeSpeciesBase.libraryAsset;
        pattern.GetComponent<SpriteLibrary>().spriteLibraryAsset = slimeSpeciesPattern.libraryAsset;
        face.GetComponent<SpriteLibrary>().spriteLibraryAsset = slimeFace.libraryAsset;
    }

    public void Feed(CropObj food)
    {
        hungerLevel += food.cropData.feedValue;

        // Clamp Hunger Value
        if (hungerLevel >= 100) hungerLevel = 100;
        else if (hungerLevel <= 0) hungerLevel = 0;

        if (!isMature)
        {
            infancyHungerReduction = maturityTime * (hungerLevel * 0.0025f);
        }
    }

    // Loose hunger when producing CS or Maturing
    public void LooseHunger(int val)
    {
        hungerLevel -= val;

        // Clamp Hunger Value
        if (hungerLevel >= 100) hungerLevel = 100;
        else if (hungerLevel <= 0) hungerLevel = 0;

        //Debug.Log($"Slime lost {val} hunger, now at {hungerLevel}");
    }

    public void SetName(string newName)
    {
        displayName = newName;
        //Debug.Log("Display name changed: " + displayName);
    }

    public void RemoveFromHabitat()
    {
        hControl.activeSlimes.Remove(gameObject);
    }
}
