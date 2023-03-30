using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoPannelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI iTitle;
    [SerializeField] private TextMeshProUGUI iPrice;
    [SerializeField] private TextMeshProUGUI iDescription;
    [SerializeField] private GameObject DescriptionPannel;
    [SerializeField] private Image previewImage;

    public void Setup(string title, int price, string description)
    {
        previewImage.gameObject.SetActive(false);

        iTitle.text = title;
        iPrice.text = price.ToString();

        if(description != "" && description != null)
        {
            DescriptionPannel.SetActive(true);
            iDescription.text = description;
        }
        else
            DescriptionPannel.SetActive(false);
    }

    public void DisplayImage(Sprite sprite)
    {
        previewImage.gameObject.SetActive(true);
        previewImage.sprite = sprite;
    }
}
