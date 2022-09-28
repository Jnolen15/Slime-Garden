using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField]
    private string[] sBaseColors;
    [SerializeField]
    private string[] sPatterns;
    [SerializeField]
    private string[] sPatternColors;
    [SerializeField]
    private GameObject slimePrefab;

    private string sBaseColor = "";
    private string sPattern = "";
    private string sPatternColor = "";
    private string sPatterntype = "";
    private bool sSpecial = false;

    private SBrain brain;

    void Start()
    {
        brain = GameObject.FindGameObjectWithTag("Brain").GetComponent<SBrain>();
    }

    public void CreateSlime(Vector3 pos)
    {
        bool compatableName = false;
        while (!compatableName)
        {
            // Base color
            sBaseColor = sBaseColors[Random.Range(0, sBaseColors.Length)];

            // Pattern Style: 50/50 choose left or right pattern style
            sPattern = sPatterns[Random.Range(0, sPatterns.Length)];

            // Test to see if pattern is Null
            if (sPattern == "Null")
            {
                sSpecial = SBaseDictionary.GetSlime(sBaseColor).sSpecial;
                if (sSpecial) //If Base is Diamond then pattern Color is Null (There is no Diamond pattern)
                {
                    sPatternColor = "Null";
                    compatableName = true;
                }
                else //Otherwise The pattern color is = to the Base color
                {
                    sPatternColor = sBaseColor;
                    compatableName = true;
                }
            }

            if (!compatableName)
            {
                // Pattern Color
                sPatternColor = sPatternColors[Random.Range(0, sPatternColors.Length)];

                if (sPatternColor != "Null") // Test to see if pattern color is Null
                {
                    // Test to see if pattern color is the same as the base color
                    if (sPatternColor != sBaseColor)
                        compatableName = true;
                }

            }

            if (!compatableName) //If slime still does not work, reset values and try again
            {
                sBaseColor = "";
                sPattern = "";
                sPatternColor = "";
            }
        }

        sPatterntype = sPatternColor + " " + sPattern;

        GameObject newSlime = Instantiate(slimePrefab, pos, Quaternion.identity);
        newSlime.GetComponent<SlimeController>().slimeSpeciesBase = SBaseDictionary.GetSlime(sBaseColor);
        newSlime.GetComponent<SlimeController>().slimeSpeciesPattern = SPatternDictionary.GetSlime(sPatterntype).slimePatternSO;
        newSlime.GetComponent<SlimeController>().sPatternColor = SPatternDictionary.GetSlime(sPatterntype).slimePatternColorName;
        newSlime.GetComponent<SlimeController>().slimeSpeciesPatternColor = SPatternDictionary.GetSlime(sPatterntype).slimePatternColor;

        brain.activeSlimes.Add(newSlime);

        sBaseColor = "";
        sPattern = "";
        sPatternColor = "";
        sPatterntype = "";
    }
}
