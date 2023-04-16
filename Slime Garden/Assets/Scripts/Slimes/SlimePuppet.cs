using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class SlimePuppet : MonoBehaviour
{
    //=============== COMPONENTS ===============
    [SerializeField] private SlimeFaceSO[] slimeFaces;
    [SerializeField] private GameObject face;
    [SerializeField] private Animator baseAnimator;
    [SerializeField] private Animator patternAnimator;
    [SerializeField] private Animator faceAnimator;
    private GameObject stateParticles;

    void Start()
    {
        TriggerAnim("Idle", slimeFaces[0]);
    }

    public void TriggerAnim(string sName, SlimeFaceSO sface, string particles = null)
    {
        // Changes the slime face, sets animations, and re-sets stateChanged bool
        // Be sure to set stateChanged to false if changing state without this function

        if (stateParticles != null)
            Destroy(stateParticles);

        face.GetComponent<SpriteLibrary>().spriteLibraryAsset = sface.libraryAsset;

        if (sName != "none")
        {
            baseAnimator.SetTrigger(sName);
            patternAnimator.SetTrigger(sName);
            faceAnimator.SetTrigger(sName);
        }

        if (particles != null)
        {
            var par = Resources.Load<GameObject>("Particles/" + particles);
            var pos = new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z);
            stateParticles = Instantiate(par, pos, transform.rotation, transform);
        }
    }
}
