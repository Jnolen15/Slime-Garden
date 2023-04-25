using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildSlime : MonoBehaviour
{
    // Variables
    [SerializeField] private bool isTamed;
    [SerializeField] private float despawnTime;
    [SerializeField] private int tameThreshold;
    [SerializeField] private int tameAmmount;
    [SerializeField] private float expMod;
    [SerializeField] private GameObject tameFX;

    // Refrences
    private SlimeData sData;
    private SlimeController sControl;
    private SBrain sBrain;
    private WildManager wildManager;

    void Start()
    {
        sData = this.GetComponent<SlimeData>();
        sControl = this.GetComponent<SlimeController>();
        sBrain = this.GetComponent<SBrain>();

        wildManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<WildManager>();

        tameThreshold = (int)((sData.sRarity/2) * 10);
    }

    private void Update()
    {
        if (despawnTime > 0) despawnTime -= Time.deltaTime;
        else
        {
            // TODO - Make this better
            Debug.Log("Slime Despawning");
            Destroy(gameObject);
        }
    }

    public void AttemptTame(CropObj currentFood)
    {
        // Dont attempt tame if already tamed
        if (isTamed)
            return;

        // Increase despawn time on each tame attempt
        despawnTime += 10f;

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
        Debug.Log("Slime Tamed!");
        isTamed = true;

        // Turn off brain so it won't go to other states
        sBrain.ToggleBrain(false);

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        Instantiate(tameFX, transform.position, transform.rotation);
        sControl.ChangeState(SlimeController.State.tamed);

        // Award EXP
        GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>().GainExperience(sData.sRarity * expMod);

        // Save to tamed slime list
        wildManager.AddTamedSlime(sData.sPattern, sData.sBaseColor, sData.sPatternColor);
        Destroy(gameObject, 3f);
    }
}
