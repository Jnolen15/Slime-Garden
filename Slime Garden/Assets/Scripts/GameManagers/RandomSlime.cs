using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSlime : MonoBehaviour
{
    [SerializeField]
    private string[] sBaseColors;
    [SerializeField]
    private List<string> sPatterns;
    [SerializeField]
    private List<string> sAllPatterns;
    [SerializeField]
    private string[] sPatternColors;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private GameObject wildSlimePrefab;

    private string sBaseColor = "";
    private string sPattern = "";
    private string sPatternColor = "";
    private string sPatterntype = "";
    private bool sSpecial = false;

    public void SetUnlockedSlimeList(List<string> patList)
    {
        sPatterns = patList;
    }

    public GameObject CreateSlime(Vector3 pos, bool isWild)
    {
        bool compatableName = false;
        while (!compatableName)
        {
            // Base color
            sBaseColor = sBaseColors[Random.Range(0, sBaseColors.Length)];

            // Pattern Style: 50/50 choose left or right pattern style
            sPattern = sPatterns[Random.Range(0, sPatterns.Count)];

            // Test to see if pattern is Null
            if (sPattern == "Null")
            {
                sSpecial = SBaseDictionary.GetSlime(sBaseColor).isSpecial;
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

        GameObject newSlime;
        if (!isWild)
        {
            newSlime = Instantiate(slimePrefab, pos, Quaternion.identity);
        } else
        {
            newSlime = Instantiate(wildSlimePrefab, pos, Quaternion.identity);
        }

        SlimeData newSD = newSlime.GetComponent<SlimeData>();
        var newBase = SBaseDictionary.GetSlime(sBaseColor);
        var newpattern = SPatternDictionary.GetSlime(sPatterntype);

        newSD.slimeSpeciesBase = newBase.slimeBaseSO;
        newSD.sBaseColor = newBase.slimeBaseName;
        newSD.slimeSpeciesBaseColor = newBase.slimeBaseColor;
        newSD.slimeSpeciesPattern = newpattern.slimePatternSO;
        newSD.sPatternColor = newpattern.slimePatternColorName;
        newSD.slimeSpeciesPatternColor = newpattern.slimePatternColor;
        newSD.sCrystal = newBase.slimeCrystal;
        newSD.Setup();

        sBaseColor = "";
        sPattern = "";
        sPatternColor = "";
        sPatterntype = "";

        return newSlime;
    }
}
