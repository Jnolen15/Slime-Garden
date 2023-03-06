using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlimeInfoPannel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI baseColorText;
    [SerializeField] private TextMeshProUGUI patternColorText;
    [SerializeField] private TextMeshProUGUI patternText;
    [SerializeField] private TextMeshProUGUI rarityText;

    public void Setup(SlimeController slime)
    {
        nameText.text = slime.speciesName;
        baseColorText.text = "Base Color: " + slime.sBaseColor;
        patternColorText.text = "Pattern Color: " + slime.sPatternColor;
        patternText.text = "Pattern: " + slime.slimeSpeciesPattern.sPattern;
        rarityText.text = "Rarity: " + slime.sRarity;
    }
}
