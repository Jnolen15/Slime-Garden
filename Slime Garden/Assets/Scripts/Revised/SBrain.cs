using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBrain : MonoBehaviour
{
    // STATE STUFF ============
    public SlimeFaceSO slimeFaceDefault;            //The SO with Default slime facial expressions. Info is taken from here.
    public SlimeFaceSO slimeFaceHappy;              //The SO with Happy slime facial expressions. Info is taken from here.
    public SlimeFaceSO slimeFaceSleep;              //The SO with Sleep slime facial expressions. Info is taken from here.

    public float stateCD = 5f;

    private void Update()
    {
        // After a random ammount of time the brain picks a few random slimes and changes what state they are in.
        stateCD -= Time.deltaTime;
        if (stateCD <= 0)
        {
            SlimeController currslimeControler = this.GetComponent<SlimeController>(); // Get slime controler
            if (currslimeControler.GetState() != SlimeController.State.held &&
                currslimeControler.GetState() != SlimeController.State.splice &&
                currslimeControler.GetState() != SlimeController.State.jump)
            // As long as the slime isn't being held, in the splicer, or jumping
            {
                int randInt = Random.Range(0, 25); // Random number 0-19
                if (randInt >= 0 && randInt < 16) // If 0-15 Jump
                {
                    currslimeControler.ChangeState(SlimeController.State.jump);
                }
                else if (randInt >= 16 && randInt <= 17) // If 16-17 Sleep
                {
                    currslimeControler.ChangeState(SlimeController.State.sleep);
                }
                else if (randInt >= 18 && randInt <= 19) // If 18-19 Love
                {
                    currslimeControler.ChangeState(SlimeController.State.love);
                }
                else if (randInt >= 20) // If 20 or above Play
                {
                    currslimeControler.ChangeState(SlimeController.State.play);
                }
            }

            stateCD += 10f + Random.Range(0f, 20f);
        }
    }
}
