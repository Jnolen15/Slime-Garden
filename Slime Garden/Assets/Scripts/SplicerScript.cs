using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplicerScript : MonoBehaviour
{
    // FOR TESTING ===============
    public bool TestOdds = true;

    // COMPONENTS ===============
    private SBrain brain;
    private PlayerController pc;
    public GameObject slimePrefab;
    public GameObject InputLeft;
    public GameObject InputRight;
    public GameObject Output;
    public GameObject Button;
    private SplicerButton sb;
    private bool splicing = false;
    private string newSlimeBase;
    private string newSlimePatternColor;
    private string newSlimePatternStyle;
    private string newSlimeName;
    private string newSlimePattern;
    private bool newSlimeSpecial;
    private int price = 0;
    private SpriteRenderer OutputlightsSR;
    private SpriteRenderer ButtonlightsSR;
    private Color currentColor;

    // SLIME PARTS ===============
    //LEFT
    public string lsBaseColor;
    public string lsPatternColor;
    public string lsPattern;
    public bool lsSpecial = false;
    //RIGHT
    public string rsBaseColor;
    public string rsPatternColor;
    public string rsPattern;
    public bool rsSpecial = false;

    // Start is called before the first frame update
    void Start()
    {
        brain = GameObject.FindGameObjectWithTag("Brain").GetComponent<SBrain>();
        pc = GameObject.Find("PlayerController").GetComponent<PlayerController>();

        InputLeft = this.transform.GetChild(0).gameObject;
        InputRight = this.transform.GetChild(1).gameObject;
        Output = this.transform.GetChild(2).gameObject;
        Button = this.transform.GetChild(3).gameObject;
        sb = Button.GetComponent<SplicerButton>();
        // Get access to the SR of the Ligths object so we can change its color
        OutputlightsSR = Output.transform.GetChild(0).GetComponent<SpriteRenderer>();
        ButtonlightsSR = Button.transform.GetChild(0).GetComponent<SpriteRenderer>();
        currentColor = InputRight.GetComponent<SplicerInput>().defaultColor;
    }

    void Update()
    {
        //If Left and right inputs are full
        if (InputLeft.GetComponent<SplicerInput>().currentSlime != null && InputRight.GetComponent<SplicerInput>().currentSlime != null && !splicing)
        {
            // If the player can afford it or not
            if (price == 0)
            {
                CalculatePrice();
                sb.priceText.enabled = true;
                sb.priceText.text = price.ToString();
            }
            if (pc.cs >= price)
            {
                ButtonlightsSR.color = Color.green;
                sb.priceText.color = Color.green;
                sb.canBePressed = true;
                if (sb.isPressed)
                {
                    sb.priceText.enabled = false;
                    splicing = true;
                    pc.cs -= price;
                    Splice();
                }
            } else
            {
                ButtonlightsSR.color = Color.red;
                sb.priceText.color = Color.red;
            }
        }

        // Update Lights
        OutputlightsSR.color = Color.Lerp(OutputlightsSR.color, currentColor, 0.01f);
        ButtonlightsSR.color = Color.Lerp(ButtonlightsSR.color, currentColor, 0.01f);
    }

    private void CalculatePrice()
    {
        price = (int)InputLeft.GetComponent<SplicerInput>().sRarity + (int)InputRight.GetComponent<SplicerInput>().sRarity;
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
        // Get slime components from each input
        lsBaseColor = InputLeft.GetComponent<SplicerInput>().sBaseColor;
        lsPatternColor = InputLeft.GetComponent<SplicerInput>().sPatternColor;
        lsPattern = InputLeft.GetComponent<SplicerInput>().sPattern;
        lsSpecial = InputLeft.GetComponent<SplicerInput>().sSpecial;
        Debug.Log("Left input: " + lsBaseColor + " " + lsPatternColor + " " + lsPattern);
        rsBaseColor = InputRight.GetComponent<SplicerInput>().sBaseColor;
        rsPatternColor = InputRight.GetComponent<SplicerInput>().sPatternColor;
        rsPattern = InputRight.GetComponent<SplicerInput>().sPattern;
        rsSpecial = InputRight.GetComponent<SplicerInput>().sSpecial;
        Debug.Log("Right input: " + rsBaseColor + " " + rsPatternColor + " " + rsPattern);

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
            // Base color: 50/50 choose left or right base
            if (Random.Range(0, 2) == 0)
            {
                newSlimeBase += lsBaseColor;
                newSlimeSpecial = lsSpecial;
            }
            else
            {
                newSlimeBase += rsBaseColor;
                newSlimeSpecial = rsSpecial;
            }

            Debug.Log("Base color: " + newSlimeBase);

            // Pattern Style: 50/50 choose left or right pattern style
            if (Random.Range(0, 2) == 0)
                newSlimePatternStyle += lsPattern;
            else
                newSlimePatternStyle += rsPattern;

            Debug.Log("Pattern Style: " + newSlimePatternStyle);

            // Test to see if pattern is Null
            if (newSlimePatternStyle == "Null")
            {
                Debug.Log("Pattern Style was Null, Making a basic slime");

                if (newSlimeSpecial) //If Base is special then pattern Color is Null (There are no special patterns)
                {
                    newSlimePatternColor = "Null";
                    compatableName = true;
                }
                else //Otherwise The pattern color is = to the Base color
                {
                    newSlimePatternColor = newSlimeBase;
                    compatableName = true;
                }
            }

            if (!compatableName)
            {
                // Pattern Color: 50/50 choose left or right pattern color
                if (Random.Range(0, 2) == 0)
                    newSlimePatternColor += lsPatternColor;
                else
                    newSlimePatternColor += rsPatternColor;

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
        if (newSlimeBase == lsBaseColor) currentColor = InputLeft.GetComponent<SplicerInput>().currentColor;
        else currentColor = InputRight.GetComponent<SplicerInput>().currentColor;

        yield return new WaitForSeconds(1);

        // Spawn new slime apply selected SO
        // Instantates the basic slime prefab
        // Gives prefab the correct SO from the dictionary via GetSlime
        Vector3 offset = new Vector3(0, 0.2f, 0);
        GameObject newSlime = Instantiate(slimePrefab, Output.transform.position + offset, Quaternion.identity);
        newSlime.GetComponent<SlimeController>().slimeSpeciesBase = SBaseDictionary.GetSlime(newSlimeBase);
        newSlime.GetComponent<SlimeController>().slimeSpeciesPattern = SPatternDictionary.GetSlime(newSlimePattern).slimePatternSO;
        newSlime.GetComponent<SlimeController>().sPatternColor = SPatternDictionary.GetSlime(newSlimePattern).slimePatternColorName;
        newSlime.GetComponent<SlimeController>().slimeSpeciesPatternColor = SPatternDictionary.GetSlime(newSlimePattern).slimePatternColor;
        // ^ Scriptable object scripts can be directly called without them needing to be in a scene


        brain.activeSlimes.Add(newSlime);

        yield return new WaitForSeconds(0.5f);

        // Eject Inputs
        InputLeft.GetComponent<SplicerInput>().Eject();
        InputRight.GetComponent<SplicerInput>().Eject();

        yield return new WaitForSeconds(0.5f);

        // Eject New Slime
        Vector3 targetPosition = new Vector3(Random.Range(-2f, 2f), -1.5f, 0f);
        targetPosition += newSlime.GetComponent<SlimeController>().transform.position;
        newSlime.GetComponent<SlimeController>().JumpToo(targetPosition);
        newSlime.GetComponent<SlimeController>().state = "idle";
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
                // Base color
                if (Random.Range(0, 2) == 0)
                    newSlimeBase += lsBaseColor;
                else
                    newSlimeBase += rsBaseColor;

                // Pattern Style
                if (Random.Range(0, 2) == 0)
                    newSlimePatternStyle += lsPattern;
                else
                    newSlimePatternStyle += rsPattern;

                // Test to see if pattern is Null
                if (newSlimePatternStyle == "Null")
                {
                    // Basic slimes seem to be twice as likely as pattern slimes when its a Basic + a pattern
                    // So here I just do another odds to keep it as a basic or generate again
                    // This will make getting a pattern one more likely. (Which is fine cuz getteing a pattern is more fun)
                    if (Random.Range(0, 3) == 0)
                    {
                        if (newSlimeBase == "Diamond") //If Base is Diamond then pattern Color is Null (There is no Diamond pattern)
                        {
                            newSlimePatternColor = "Null";
                            compatableName = true;
                        }
                        else //Otherwise The pattern color is = to the Base color
                        {
                            newSlimePatternColor = newSlimeBase;
                            compatableName = true;
                        }
                    }
                    else
                    {
                        if (lsPattern == "Null") newSlimePatternStyle = rsPattern;
                        else if (rsPattern == "Null") newSlimePatternStyle = lsPattern;
                    }

                }

                if (!compatableName)
                {
                    // Pattern Color
                    if (Random.Range(0, 2) == 0)
                        newSlimePatternColor += lsPatternColor;
                    else
                        newSlimePatternColor += rsPatternColor;

                    if (newSlimePatternColor != "Null") // Test to see if pattern color is Null
                    {
                        // Test to see if pattern color is the same as the base color
                        if (newSlimePatternColor != newSlimeBase)
                        {
                            compatableName = true;
                        }
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

        // Eject Inputs
        InputLeft.GetComponent<SplicerInput>().Eject();
        InputRight.GetComponent<SplicerInput>().Eject();

        splicing = false;
    }
}
