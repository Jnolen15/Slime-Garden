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
    
    private bool stateChanged;                  // Bool to make sure initial state changes only happen once
    private bool isJumping;                     // Bool to make sure jumping isn't called when alredy in progress


    void Start()
    {
        // Find Brain
        brain = GameObject.FindGameObjectWithTag("Brain").GetComponent<SBrain>();

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
                    stateChanged = false;
                    JumpState();
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
                    StartState("Hop", brain.slimeFaceHappy);
                }
                // JUMP AROUND
                break;
        }
    }

    public void ChangeState(State newState)
    {
        //Debug.Log("Changed to " + newState + " state.");
        state = newState;
        stateChanged = true;
    }

    private void StartState(string sName, SlimeFaceSO sface)
    {
        //Debug.Log(sName + " state is being started");

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
        if (!isJumping)
        {
            isJumping = true;

            //Makes a random position to jump too
            Vector3 newPos = new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
            newPos = newPos + transform.position;

            // TODO:
            // Here test if within bounds
            // Idea: raycast, if hits a bordeer fence, jump in opposite direction.

            // Flipping the sprite in the direction it jumps
            if (newPos.x < transform.position.x)
                FlipSprite(false);
            else
                FlipSprite(true);

            float jumpDuration = Vector3.Distance(transform.position, newPos) / 2f;

            StartCoroutine(Jump(newPos, jumpDuration));
        }
    }

    // Used by other scripts to make a slime jump to a specific location
    /*public void JumpToo(Vector3 targetPosition)
    {
        isJumping = true;

        float hypotenuse = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(transform.position.x - targetPosition.x), 2)    //Uses Pythagorean theorem to get hypotenuse
                                    + Mathf.Pow(Mathf.Abs(transform.position.y - targetPosition.y), 2));
        float jumpDuration = hypotenuse / 1.5f;            // Calculating jump speed.
        float maxHeight = 1f + (hypotenuse / 3);  //hypotenuse / 3 to get the jump height because this makes for a larger height the larger the jump.
        Vector3 height = new Vector3(maxHeight, maxHeight, 0f);

        if (targetPosition.x < transform.position.x) // Flipping the sprite in the direction it jumps
        {
            height.x = height.x * 1;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            height.x = height.x * -1;
            transform.localScale = new Vector3(-1, 1, 1);
        }

        StartCoroutine(Jump(targetPosition, height, jumpDuration));
    }*/

    IEnumerator Jump(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            var x = Mathf.Lerp(startPosition.x, targetPosition.x, time / duration);
            var y = 0.0f;
            var z = Mathf.Lerp(startPosition.z, targetPosition.z, time / duration);

            if(time < duration / 2)
            {
                y = Mathf.Lerp(startPosition.y, duration, time / duration);
            }
            else if(time < duration)
            {
                y = Mathf.Lerp(duration, 0, time / duration);
            }
            
            transform.position = new Vector3(x, y, z);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        //spawnlandingparticles();

        yield return new WaitForSeconds(0.2f);

        ChangeState(State.idle);
        isJumping = false;
    }

    // ========================== MISC ==========================

    private void FlipSprite(bool flip)
    {
        Basesr.flipX = flip;
        patternsr.flipX = flip;
        facesr.flipX = flip;
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