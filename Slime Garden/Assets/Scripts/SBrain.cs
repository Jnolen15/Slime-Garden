using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBrain : MonoBehaviour
{
    // SLIME DICTIONARIES ===============
    public SBaseDictionary sBaseDictionary;         //The SO of the dictionary that holds all slime Base SOs.
    public SPatternDictionary sPatternDictionary;   //The SO of the dictionary that holds all slime Pattern SOs.

    // STATE STUFF ===============
    public SlimeFaceSO slimeFaceDefault;            //The SO with Default slime facial expressions. Info is taken from here.
    public SlimeFaceSO slimeFaceHappy;              //The SO with Happy slime facial expressions. Info is taken from here.
    public SlimeFaceSO slimeFaceSleep;              //The SO with Sleep slime facial expressions. Info is taken from here.

    public GameObject sleepParticles;               //The particles used for sleeping slimes
    public GameObject heartParticles;               //The particles used for loving slimes


    // COMPONENTS ===============
    public List<GameObject> activeSlimes;           //List of all active slimes

    private float stateCD = 2f;

    private void Start()
    {
        foreach (GameObject ob in GameObject.FindGameObjectsWithTag("Slime"))
        {
            activeSlimes.Add(ob);
        }
    }

    private void Update()
    {
        // After a random ammount of time the brain picks a few random slimes and changes what state they are in.
        stateCD -= Time.deltaTime;
        if (stateCD <= 0)
        {
            int numSlimes = Random.Range(0, activeSlimes.Count); // How many slimes will be effected by the state change
            List<int> prevSlimes = new List<int>(); // List of effected slimes (This is so the same slime isnt effected multiple times in the same state change)
            int slimeIndex = 0; // Which slime in the list of active slimes is effected

            for (int i = 0; i < numSlimes; i++) // For each number of effected slime
            {
                slimeIndex = Random.Range(0, activeSlimes.Count); // Pick random slime
                if (!prevSlimes.Contains(slimeIndex)) // As long as that slime wasn't already effected
                {
                    prevSlimes.Add(slimeIndex); // Add slime to list of effected slimes

                    GameObject currslime = activeSlimes[slimeIndex]; // Get slime game object
                    if (currslime.gameObject.GetComponent<SlimeController>().state != "held" && 
                        currslime.gameObject.GetComponent<SlimeController>().state != "splice" &&
                        currslime.gameObject.GetComponent<SlimeController>().state != "jump") 
                        // As long as the slime isn't being held, in the splicer, or jumping
                    {
                        int randInt = Random.Range(0, 25); // Random number 0-19
                        if (randInt >= 0 && randInt < 16) // If 0-15 Jump
                        {
                            if (currslime.transform.GetChild(4).gameObject.transform.childCount > 0) // Remove particle FX from previous state if they were still there.
                                Destroy(currslime.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject);

                            currslime.gameObject.GetComponent<SlimeController>().queuedState = "jump";
                        }
                        else if (randInt >= 16 && randInt <= 17) // If 16-17 Sleep
                        {
                            if (currslime.transform.GetChild(4).gameObject.transform.childCount > 0)
                                Destroy(currslime.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject);

                            currslime.gameObject.GetComponent<SlimeController>().queuedState = "sleep";
                            currslime.gameObject.GetComponent<SlimeController>().stateParticles = sleepParticles;
                        }
                        else if (randInt >= 18 && randInt <= 19) // If 18-19 Love
                        {
                            if (currslime.transform.GetChild(4).gameObject.transform.childCount > 0)
                                Destroy(currslime.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject);

                            currslime.gameObject.GetComponent<SlimeController>().queuedState = "love";
                            currslime.gameObject.GetComponent<SlimeController>().stateParticles = heartParticles;
                        }
                        else if (randInt >= 20) // If 20 or above Play
                        {
                            if (currslime.transform.GetChild(4).gameObject.transform.childCount > 0)
                                Destroy(currslime.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject);

                            currslime.gameObject.GetComponent<SlimeController>().queuedState = "play";
                        }
                    }
                }
            }

            stateCD += 0.2f + Random.Range(0f, 3f);
        }
    }

    public void isHeld(GameObject slime) // This gets called from the drag drop script. if a slime is being held.
    {
        //slime.gameObject.GetComponent<SlimeController>().state = "held";
        if (slime.transform.GetChild(4).gameObject.transform.childCount > 0)
            Destroy(slime.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject);
    }

}
