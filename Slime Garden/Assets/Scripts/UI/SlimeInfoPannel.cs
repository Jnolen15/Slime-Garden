using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SlimeInfoPannel : MonoBehaviour
{
    [SerializeField] private GameObject nameTextObj;
    [SerializeField] private GameObject nameInputObj;
    [SerializeField] private TMP_InputField inputText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI baseColorText;
    [SerializeField] private TextMeshProUGUI patternColorText;
    [SerializeField] private TextMeshProUGUI patternText;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private SlimeData curslime;

    public void Setup(SlimeData slime)
    {
        curslime = slime;
        nameTextObj.SetActive(true);
        nameInputObj.SetActive(false);
        nameText.text = curslime.displayName;
        inputText.text = "New Name";
        baseColorText.text = "Base Color: " + curslime.sBaseColor;
        patternColorText.text = "Pattern Color: " + curslime.sPatternColor;
        patternText.text = "Pattern: " + curslime.slimeSpeciesPattern.sPattern;
        rarityText.text = "Rarity: " + curslime.sRarity;
    }

    public void OpenRename()
    {
        nameTextObj.SetActive(false);
        nameInputObj.SetActive(true);
    }

    public void SetRename(string newName)
    {
        curslime.SetName(newName);

        nameText.text = curslime.displayName;
        nameTextObj.SetActive(true);
        nameInputObj.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }
}
