using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplicerScript : MonoBehaviour
{
    // FOR TESTING ===============
    public bool TestOdds = true;

    // COMPONENTS ===============
    private PlayerData pData;
    private StatTracker stats;
    public GameObject slimePrefab;
    public GameObject InputLeft;
    public GameObject InputRight;
    public GameObject Button;
    public GameObject Output;
    public Transform outputPos;
    private SplicerButton sb;
    private AudioSource audioSrc;
    private bool splicing = false;
    private string newSlimeBase;
    private string newSlimePatternColor;
    private string newSlimePatternStyle;
    private string newSlimeName;
    private string newSlimePattern;
    //private bool newSlimeSpecial;
    private int price = 0;
    [SerializeField] private Material outputMat;
    private Color currentColor;

    [SerializeField] private AudioClip spliceSound;

    // SLIME PARTS ===============
    //LEFT
    private SplicerInput ls;
    private SplicerInput rs;

    void Start()
    {
        pData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        stats = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<StatTracker>();
        ls = InputLeft.GetComponent<SplicerInput>();
        rs = InputRight.GetComponent<SplicerInput>();
        sb = Button.GetComponent<SplicerButton>();
        audioSrc = this.GetComponent<AudioSource>();

        outputMat = Output.GetComponentInChildren<MeshRenderer>().material;

        currentColor = InputRight.GetComponent<SplicerInput>().defaultColor;
    }

    void Update()
    {
        //If Left and right inputs are full
        if (InputLeft.GetComponent<SplicerInput>().currentSlime != null && InputRight.GetComponent<SplicerInput>().currentSlime != null && !splicing)
        {
            // Calculate Price
            if (price == 0)
            {
                CalculatePrice();
                sb.priceText.enabled = true;
                sb.priceText.text = price.ToString();
            }
            // If the player can afford it or not
            if (pData.CanAfford(price))
            {
                sb.UpdateColor(Color.green);
                sb.priceText.color = Color.green;
                sb.canBePressed = true;
                if (sb.isPressed)
                {
                    pData.MakePurchase(price);
                    sb.priceText.enabled = false;
                    splicing = true;
                    Splice();
                }
            } else
            {
                sb.UpdateColor(Color.red);
                sb.priceText.color = Color.red;
            }
        } else
        {
            Removed();
            sb.UpdateColor(currentColor);
        }

        // Update Lights
        outputMat.SetColor("_HighlightColor", Color.Lerp(outputMat.GetColor("_HighlightColor"), currentColor, 0.01f));
    }

    private void CalculatePrice()
    {
        price = (int)InputLeft.GetComponent<SplicerInput>().sRarity + (int)InputRight.GetComponent<SplicerInput>().sRarity + 2;
    }

    // Called when a slime is taken out of one of the inputs
    public void Removed()
    {
        price = 0;
        sb.priceText.enabled = false;
        sb.canBePressed = false;
    }

    private void Splice()
    {
        Debug.Log("Left input: " + ls.sBaseColor + " " + ls.sPatternColor + " " + ls.sPattern);
        Debug.Log("Right input: " + rs.sBaseColor + " " + rs.sPatternColor + " " + rs.sPattern);

        // Test Odds is a function that generates 1000 slimes 
        // and displays all the combos of slimes generated, 
        // and how many of each were generated
        if (TestOdds)
        {
            RunTestOdds();
        }
        else
        {
            CreateSlime();
            StartCoroutine(spawnSlime());
        }

    }

    public void CreateSlime()
    {
        // Randomly choose components
        bool compatableName = false;
        while (!compatableName)
        {
            // ======== Base color ========
            if (Random.Range(0, 2) == 0)
                newSlimeBase += ls.sBaseColor;
            else
                newSlimeBase += rs.sBaseColor;

            Debug.Log("Base color: " + newSlimeBase);

            // ======== Pattern Style ========
            if (Random.Range(0, 2) == 0)
                newSlimePatternStyle += ls.sPattern;
            else
                newSlimePatternStyle += rs.sPattern;

            Debug.Log("Pattern Style: " + newSlimePatternStyle);

            // If Pattern is null, make basic slime
            if (newSlimePatternStyle == "Null")
            {
                Debug.Log("Pattern Style was Null, Making a basic slime");

                newSlimePatternColor = newSlimeBase;
                compatableName = true;
            }

            if (!compatableName)
            {
                // ======== Pattern Color ========
                if (Random.Range(0, 2) == 0)
                    newSlimePatternColor += ls.sPatternColor;
                else
                    newSlimePatternColor += rs.sPatternColor;

                Debug.Log("Pattern Color: " + newSlimePatternColor);

                if (newSlimePatternColor != "Null") // Test to see if pattern color is Null
                {
                    // Test to see if pattern color is the same as the base color
                    if (newSlimePatternColor != newSlimeBase)
                    {
                        compatableName = true;
                    }
                    else
                    {
                        Debug.Log("Pattern Color was same as Base Color. Re-rolling");
                    }
                } else
                {
                    Debug.LogError("pattern Color was Null, Re-rolling");
                }
            }

            if (!compatableName) //If slime still does not work, reset values and try again
            {
                newSlimeBase = "";
                newSlimePatternColor = "";
                newSlimePatternStyle = "";
            }
        }

        newSlimePattern += (newSlimePatternColor + " " + newSlimePatternStyle);

        newSlimeName += (newSlimeBase + " " + newSlimePatternColor + " " + newSlimePatternStyle);
        Debug.Log("New Slime: " + newSlimeName);
    }

    IEnumerator spawnSlime()
    {
        yield return new WaitForSeconds(0.5f);

        // Change Lights color to Base color of Created Slime
        if (newSlimeBase == ls.sBaseColor) currentColor = InputLeft.GetComponent<SplicerInput>().currentColor;
        else currentColor = InputRight.GetComponent<SplicerInput>().currentColor;

        yield return new WaitForSeconds(1);

        // Spawn new slime apply selected SO
        // Instantates the basic slime prefab
        // Gives prefab the correct SO from the dictionary via GetSlime
        GameObject newSlime = Instantiate(slimePrefab, outputPos.position, Quaternion.identity);
        SlimeData newSD = newSlime.GetComponent<SlimeData>();
        var newBase = SBaseDictionary.GetSlime(newSlimeBase);
        var newpattern = SPatternDictionary.GetSlime(newSlimePattern);

        newSD.slimeSpeciesBase = newBase.slimeBaseSO;
        newSD.sBaseColor = newBase.slimeBaseName;
        newSD.slimeSpeciesBaseColor = newBase.slimeBaseColor;
        newSD.slimeSpeciesPattern = newpattern.slimePatternSO;
        newSD.sPatternColor = newpattern.slimePatternColorName;
        newSD.slimeSpeciesPatternColor = newpattern.slimePatternColor;
        newSD.sCrystal = newBase.slimeCrystal;
        newSD.Setup(true, true);

        // Award EXP and increment stat
        pData.GainExperience(newSD.sRarity);
        stats.IncrementStat("slimesCreated", 1);
        audioSrc.PlayOneShot(spliceSound);

        yield return new WaitForSeconds(0.5f);

        // Eject New Slime
        //newSlime.GetComponent<SlimeController>().ChangeState(SlimeController.State.jump);

        // Reset Strings
        newSlimeBase = "";
        newSlimePatternColor = "";
        newSlimePatternStyle = "";
        newSlimeName = "";
        newSlimePattern = "";

        splicing = false;
        Button.GetComponent<SplicerButton>().isPressed = false;
        Button.GetComponent<SplicerButton>().canBePressed = false;
        price = 0;

        currentColor = InputRight.GetComponent<SplicerInput>().defaultColor;
    }



    // FOR TESTING ODDS OF EACH SLIME TYPE
    private void RunTestOdds()
    {
        // Get all possible Combos
        Dictionary<string, int> generatedSlimes = new Dictionary<string, int>();

        // Generate Slimes X Times, Keeping track of how many times each slime is made
        for (int i = 0; i < 1000; i++)
        {
            bool compatableName = false;
            while (!compatableName)
            {
                // ======== Base color ========
                if (Random.Range(0, 2) == 0)
                    newSlimeBase += ls.sBaseColor;
                else
                    newSlimeBase += rs.sBaseColor;

                Debug.Log("Base color: " + newSlimeBase);

                // ======== Pattern Style ========
                if (Random.Range(0, 2) == 0)
                    newSlimePatternStyle += ls.sPattern;
                else
                    newSlimePatternStyle += rs.sPattern;

                Debug.Log("Pattern Style: " + newSlimePatternStyle);

                // If Pattern is null, make basic slime
                if (newSlimePatternStyle == "Null")
                {
                    Debug.Log("Pattern Style was Null, Making a basic slime");

                    newSlimePatternColor = newSlimeBase;
                    compatableName = true;
                }

                if (!compatableName)
                {
                    // ======== Pattern Color ========
                    if (Random.Range(0, 2) == 0)
                        newSlimePatternColor += ls.sPatternColor;
                    else
                        newSlimePatternColor += rs.sPatternColor;

                    Debug.Log("Pattern Color: " + newSlimePatternColor);

                    if (newSlimePatternColor != "Null") // Test to see if pattern color is Null
                    {
                        // Test to see if pattern color is the same as the base color
                        if (newSlimePatternColor != newSlimeBase)
                        {
                            compatableName = true;
                        }
                        else
                        {
                            Debug.Log("Pattern Color was same as Base Color. Re-rolling");
                        }
                    }
                    else
                    {
                        Debug.LogError("pattern Color was Null, Re-rolling");
                    }
                }

                if (!compatableName) //If slime still does not work, reset values and try again
                {
                    newSlimeBase = "";
                    newSlimePatternColor = "";
                    newSlimePatternStyle = "";
                }
            }
            newSlimeName += (newSlimeBase + " " + newSlimePatternColor + " " + newSlimePatternStyle);

            // If slime is in the dictionary incremet count, if not add it
            int count;
            if (generatedSlimes.TryGetValue(newSlimeName, out count))
            {
                count += 1;
                generatedSlimes[newSlimeName] = count;
            }
            else
            {
                generatedSlimes.Add(newSlimeName, 0);
            }

            newSlimeBase = "";
            newSlimePatternColor = "";
            newSlimePatternStyle = "";
            newSlimeName = "";
            newSlimePattern = "";
        }

        // Print Report
        Debug.Log(generatedSlimes);
        foreach (KeyValuePair<string, int> kvp in generatedSlimes)
            Debug.Log(kvp.Key + " generated " + kvp.Value + " times.");

        splicing = false;
    }
}
