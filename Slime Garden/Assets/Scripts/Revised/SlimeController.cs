using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class SlimeController : MonoBehaviour
{
    //=============== SLIME ATTRIBUTES ===============
    public string speciesName;                  //Name of slime species
    public float sRarity;                       //Rarity of slime

    //=============== SCRIPTABLE OBJECTS ===============
    public SlimeBaseSO slimeSpeciesBase;        //The SO of this slime species Base color. Info is taken from here.
    public string sBaseColor;                   //Base color of species
    public Color slimeSpeciesBaseColor;         //The Color that will be applied to the Base
    public SlimePatternSO slimeSpeciesPattern;  //The SO of this slime species. Info is taken from here.
    public string sPatternColor;                //Pattern color of species
    public Color slimeSpeciesPatternColor;      //The Color that will be applied to the Pattern
    public SlimeFaceSO slimeFace;               //The SO with slime facial expressions. Info is taken from here.

    //=============== COMPONENTS ===============
    private GameObject baseSlime;               //The child gameobject containing the base SR
    private GameObject pattern;                 //The child gameobject containing the pattern SR
    private GameObject face;                    //The face rendered above the slime
    private SpriteRenderer Basesr;              //The SpriteRenderer of the base child object
    private SpriteRenderer patternsr;           //The SpriteRenderer of the pattern child object
    private SpriteRenderer facesr;              //The SpriteRenderer of the face child object
    private Animator baseAnimator;              //The animator for the base
    private Animator patternAnimator;           //The animator for the pattern
    private Animator faceAnimator;              //The animator for the face
    private SBrain brain;                       //The Slime Brain Script

    //=============== HELP VARIABLES ===============
    private bool stateChanged;                  // Bool to make sure initial state changes only happen once
    [SerializeField]
    private bool grounded;                      // Bool to make sure slime is grounded
    [SerializeField]
    private bool jumped;                        // Bool to see if slime has already jumped
    [SerializeField]    
    private float stateTimer;                   // Used to add pauses to certain states

    //=============== SLIME STATES ===============
    public enum State                           // All possible slime states
    {
        idle,
        jump,
        held,
        splice,
        sleep,
        love,
        play
    }
    [SerializeField] private State state;       // This slimes current state
    public State GetState() { return state; }   // Get State function so it can stay private


    void Start()
    {
        // Find Brain
        brain = this.GetComponent<SBrain>();

        // Create name and calculate rarity
        speciesName = sBaseColor + " " + sPatternColor + " " + slimeSpeciesPattern.sPattern;
        sRarity = slimeSpeciesBase.sRarity + slimeSpeciesPattern.sRarity;

        // ========== Sprite stuff ==========
        baseSlime = this.transform.GetChild(0).gameObject;
        pattern = this.transform.GetChild(1).gameObject;
        face = this.transform.GetChild(2).gameObject;
        // Sprite renderers
        Basesr = baseSlime.GetComponent<SpriteRenderer>();
        patternsr = pattern.GetComponent<SpriteRenderer>();
        facesr = face.GetComponent<SpriteRenderer>();
        // Sprite animators
        baseAnimator = baseSlime.GetComponent<Animator>();
        patternAnimator = pattern.GetComponent<Animator>();
        faceAnimator = face.GetComponent<Animator>();
        // Sprite library assets (Used to swap out sprites in animations)
        baseSlime.GetComponent<SpriteLibrary>().spriteLibraryAsset = slimeSpeciesBase.libraryAsset;
        pattern.GetComponent<SpriteLibrary>().spriteLibraryAsset = slimeSpeciesPattern.libraryAsset;
        face.GetComponent<SpriteLibrary>().spriteLibraryAsset = slimeFace.libraryAsset;
        // Assign Colors
        Basesr.color = slimeSpeciesBaseColor;
        patternsr.color = slimeSpeciesPatternColor;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            JumpState();

        // State machine switch
        switch (state)
        {
            case State.idle:
                if (stateChanged)
                {
                    StartState("Idle", brain.slimeFaceDefault);
                }
                break;
            case State.jump:
                if (stateChanged)
                {
                    StartState("Held", brain.slimeFaceDefault);
                    JumpState();
                }
                if (jumped && grounded)
                {
                    jumped = false;
                    ChangeState(State.idle);
                }
                break;
            case State.held:
                if (stateChanged)
                {
                    StartState("Held", brain.slimeFaceDefault);
                }
                break;
            case State.splice:
                if (stateChanged)
                {
                    StartState("Stationary", brain.slimeFaceDefault);
                }
                break;
            case State.sleep:
                if (stateChanged)
                {
                    StartState("Sleep", brain.slimeFaceSleep);
                }
                break;
            case State.love:
                if (stateChanged)
                {
                    StartState("Hop", brain.slimeFaceHappy);
                }
                break;
            case State.play:
                if (stateChanged)
                {
                    StartState("Held", brain.slimeFaceHappy);
                }
                // JUMP AROUND (WIP)
                if (stateTimer > 0) stateTimer -= Time.deltaTime;
                if (!jumped && grounded && stateTimer <= 0)
                {
                    StartState("Held", brain.slimeFaceHappy);
                    JumpState();
                }
                if (jumped && grounded)
                {
                    StartState("Idle", brain.slimeFaceHappy);
                    jumped = false;
                    stateTimer = 0.2f;
                }
                break;
        }
    }

    public void ChangeState(State newState)
    {
        state = newState;
        stateChanged = true;
    }

    private void StartState(string sName, SlimeFaceSO sface)
    {
        // Changes the slime face, sets animations, and re-sets stateChanged bool
        // Be sure to set stateChanged to false if changing state without this function

        this.slimeFace = sface;
        face.GetComponent<SpriteLibrary>().spriteLibraryAsset = slimeFace.libraryAsset;

        baseAnimator.SetTrigger(sName);
        patternAnimator.SetTrigger(sName);
        faceAnimator.SetTrigger(sName);
        stateChanged = false;
    }

    // ========================== JUMPING STATE ==========================

    public void JumpState()
    {
        Debug.Log("Jumping");
        Vector3 newPos = new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f));

        // TODO:
        // Here test if within bounds
        // Idea: raycast, if hits a bordeer fence, jump in opposite direction.

        // Flipping the sprite in the direction it jumps
        Debug.Log("newPosx: " + newPos.x + " transformpositionx: " + transform.position.x);
        if ((newPos.x + transform.position.x) < transform.position.x)
            FlipSprite(false);
        else
            FlipSprite(true);

        // Jump by force
        this.GetComponent<Rigidbody>().AddForce(newPos * 4, ForceMode.Impulse);

        jumped = true;
        grounded = false;
    }

    // ========================== MISC ==========================

    private void FlipSprite(bool flip)
    {
        Basesr.flipX = flip;
        patternsr.flipX = flip;
        facesr.flipX = flip;
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }

    // RESOURCES.LOAD THIS PARTICAL STUFF
    public void spawnlandingparticles()
    {
        //Instantiate(particles, landingParticleSpawnPos.transform.position, Quaternion.identity, landingParticleSpawnPos.transform);
    }

    public void spawnstateparticles(GameObject ptype)
    {
        //Instantiate(ptype, stateParticleSpawnPos.transform.position, Quaternion.identity, stateParticleSpawnPos.transform);
    }
}