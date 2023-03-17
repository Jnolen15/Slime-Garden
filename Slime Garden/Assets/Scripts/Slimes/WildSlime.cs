using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildSlime : MonoBehaviour
{
    // Variables
    [SerializeField] private bool isTamed;
    [SerializeField] private int tameThreshold;
    [SerializeField] private int tameAmmount;

    // Refrences
    private SlimeData sData;
    private WildManager wildManager;

    void Start()
    {
        sData = this.GetComponent<SlimeData>();

        wildManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<WildManager>();

        tameThreshold = (int)((sData.sRarity/2) * 10);
    }

    public void AttemptTame(CropObj currentFood)
    {
        Debug.Log(sData.speciesName + " was fed and is being tamed");
        tameAmmount += currentFood.cropData.tameValue;
        Debug.Log("Tame Progress: " + tameAmmount + " Tame Threshold: " + tameThreshold);
        // If tame ammount has passed threshold, auto tame
        if (tameAmmount >= tameThreshold)
        {
            Debug.Log("Progress met or surpassed threshold. Taming slime " + sData.speciesName);
            Tame();
            return;
        }

        var roll = Random.Range(0, tameThreshold);
        Debug.Log("Roll: " + roll);
        if (roll < tameAmmount)
        {
            Debug.Log("Roll Success. Taming slime " + sData.speciesName);
            Tame();
            return;
        }
    }

    public void Tame()
    {
        // Save to tamed slime list
        wildManager.AddTamedSlime(sData.sPattern, sData.sBaseColor, sData.sPatternColor);
        Destroy(gameObject);
    }
}
