using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class SlimeController : MonoBehaviour
{
    // COMPONENTS ===============
    private GameObject baseSlime;               //The child gameobject containing the base SR
    private GameObject pattern;                 //The child gameobject containing the pattern SR
    private GameObject particles;               //The landing particle system
    public GameObject stateParticles;           //The state particle system
    private GameObject landingParticleSpawnPos; //The spawn position of the landing particles
    private GameObject stateParticleSpawnPos;   //The spawn position of the state particles
    private GameObject shadow;                  //The shadow rendered under the slime
    private GameObject face;                    //The face rendered above the slime
    public SpriteRenderer Basesr;               //The SpriteRenderer. Needed to change between sprites.
    public SpriteRenderer patternsr;            //The SpriteRenderer of the pattern child object.
    public SpriteRenderer shadowsr;             //The SpriteRenderer for the shadows of the slimes.
    public SpriteRenderer facesr;               //The SpriteRenderer for the faces of the slimes.
    public Animator baseAnimator;               //The animator for the base
    public Animator patternAnimator;            //The animator for the pattern
    public Animator faceAnimator;               //The animator for the face
    public DragDrop dragDrop;                   //The Drag and Drop script
    private SBrain brain;                       //The Slime Brain Script

    // SCRIPTABLE OBJECT ===============
    //public SlimeSO slimeSpecies;              //The SO of this slime species. Info is taken from here.
    public SlimeBaseSO slimeSpeciesBase;        //The SO of this slime species Base color. Info is taken from here.
    public Color slimeSpeciesBaseColor;         //The Color that will be applied to the Base
    public SlimePatternSO slimeSpeciesPattern;  //The SO of this slime species. Info is taken from here.
    public Color slimeSpeciesPatternColor;      //The Color that will be applied to the Pattern
    public SlimeFaceSO slimeFace;               //The SO with slime facial expressions. Info is taken from here.

    // SLIME ATTRIBUTES ===============
    public string speciesName;                  //Name of slime species
    public string sBaseColor;                   //Base color of species
    public string sPatternColor;                //Pattern color of species
    public string sPattern;                     //Pattern of species
    public float sRarity;                       //Rarity of slime
    public bool sSpecial;                       //Special is true if the base color isn't one of the basic 12

    public float habitatX = 10;                 //X postiton of habitat bounds
    public float habitatZ = 10;                 //Y postiton of habitat bounds

    public bool isJumping = false;

    public Sprite shadow1;
    public Sprite shadow2;

    // State stuff
    public enum State
    {
        idle,
        jump,
        held,
        splice,
        sleep,
        love,
        play
    }
    [SerializeField] private State state;
    public State GetState() { return state; }

    public bool stateChanged; // Bool to make sure initial state changes only happen once

    void Start()
    {
        baseSlime = this.transform.GetChild(0).gameObject;
        pattern = this.transform.GetChild(1).gameObject;
        landingParticleSpawnPos = this.transform.GetChild(4).gameObject;
        shadow = this.transform.GetChild(3).gameObject;
        face = this.transform.GetChild(2).gameObject;
        stateParticleSpawnPos = this.transform.GetChild(5).gameObject;
        Basesr = baseSlime.GetComponent<SpriteRenderer>();
        patternsr = pattern.GetComponent<SpriteRenderer>();
        shadowsr = shadow.GetComponent<SpriteRenderer>();
        facesr = face.GetComponent<SpriteRenderer>();
        dragDrop = gameObject.GetComponent<DragDrop>();
        brain = GameObject.FindGameObjectWithTag("Brain").GetComponent<SBrain>();

        baseSlime.GetComponent<SpriteLibrary>().spriteLibraryAsset = slimeSpeciesBase.libraryAsset;
        pattern.GetComponent<SpriteLibrary>().spriteLibraryAsset = slimeSpeciesPattern.libraryAsset;
        face.GetComponent<SpriteLibrary>().spriteLibraryAsset = slimeFace.libraryAsset;

        speciesName = sBaseColor + " " + sPatternColor + " " + slimeSpeciesPattern.sPattern;
        sRarity = slimeSpeciesBase.sRarity + slimeSpeciesPattern.sRarity;
        //sBaseColor = slimeSpeciesBase.sName;
        //sPatternColor = slimeSpeciesPattern.sPatternColor;
        sPattern = slimeSpeciesPattern.sPattern;
        sSpecial = slimeSpeciesBase.sSpecial;
        //Basesr.sprite = slimeSpeciesBase.sSprite;
        Basesr.color = slimeSpeciesBaseColor;
        //patternsr.sprite = slimeSpeciesPattern.sPatternSprite;
        patternsr.color = slimeSpeciesPatternColor;
        particles = slimeSpeciesBase.sParticles;
    }

    void Update()
    {
        //FOR TESTING
        if (Input.GetKeyDown(KeyCode.Y))
        {
            ChangeState(State.play);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            ChangeState(State.sleep);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            ChangeState(State.love);
        }

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
                    Debug.Log("State has been changed to JUMP");
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
        Debug.Log("Changed to " + newState + " state.");
        state = newState;
        stateChanged = true;
    }

    private void StartState(string sName, SlimeFaceSO sface)
    {
        Debug.Log(sName + " state is being started");

        this.slimeFace = sface;
        face.GetComponent<SpriteLibrary>().spriteLibraryAsset = slimeFace.libraryAsset;

        baseAnimator.SetTrigger(sName);
        patternAnimator.SetTrigger(sName);
        faceAnimator.SetTrigger(sName);
        //shadowsr.sprite = shadow1;
        stateChanged = false;
    }

    // ========================== JUMPING STATE ==========================

    public void JumpState()
    {
        if (!isJumping)
        {
            isJumping = true;

            //Makes a random position ro jump too
            Vector3 newPos = new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
            newPos = newPos + transform.position;

            // Testing to make sure newPos is in habitat bounds (Will try 20 times then reposition at center)
            int count = 0;
            while (newPos.x > habitatX || newPos.y > habitatZ || newPos.x < -habitatX || newPos.y < -habitatZ) 
            {
                if (count > 20)
                {
                    //Debug.Log("Slime out of bounds. Repositioning");
                    transform.position = new Vector3(0f, 0f, 0f);
                }
                //Debug.Log("NewPos was not within bounds. Rerolling.");
                newPos = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)); // If not, get a new pos
                newPos = newPos + transform.position;
                count += 1;
            }

            // Uses Pythagorean theorem to get hypotenuse, which is used to calculate jump height and speed
            float hypotenuse = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(transform.position.x - newPos.x), 2)
                                    + Mathf.Pow(Mathf.Abs(transform.position.y - newPos.y), 2));
            float jumpDuration = hypotenuse / 1.5f;
            float maxHeight = 1f + (hypotenuse / 3);
            Vector3 height = new Vector3(maxHeight, maxHeight, 0f);

            // Flipping the sprite in the direction it jumps
            if (newPos.x < transform.position.x)
            {
                height.x = height.x * 1;
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                height.x = height.x * -1;
                transform.localScale = new Vector3(-1, 1, 1);
            }

            StartCoroutine(Jump(newPos, height, jumpDuration)); // Call to jump
        }
    }

    // Used by other scripts to make a slime jump to a specific location
    public void JumpToo(Vector3 targetPosition)
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
    }

    IEnumerator Jump(Vector3 targetPosition, Vector3 targetHeight, float duration)
    {
        /* Jump Code
         * targetPosition - Landing spot
         * targetScale - Max height it will reach in jump
         * duration - length of jump
         * This uses Lerp to smoothly move the slime to its target position.
         * It will also lerp up to targetHeight in the first half
         * then back to normal scale duing the second half. 
         */
        float time = 0;
        Vector3 startPosition = transform.position;
        Vector3 startScale = transform.localScale;
        Vector3 shadowstartPosition = shadow.transform.position;
        Vector3 sadowStartScale = shadow.transform.localScale;
        Vector3 sadowtargetScale = new Vector3(0.5f - (Mathf.Abs(targetHeight.x) - 1), 0.5f - (Mathf.Abs(targetHeight.y) - 1), 0f);
        Vector3 sadowtargetDist = new Vector3(0f, -(Mathf.Abs(targetHeight.y) - 1), 0f);

        Debug.Log("Jumping from " + startPosition + " to " + targetPosition);

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);

            if(time < duration / 2)
            {
                transform.localScale = Vector3.Lerp(startScale, targetHeight, time / (duration / 2));
                shadow.transform.localScale = Vector3.Lerp(sadowStartScale, sadowtargetScale, time / (duration / 2));
                shadow.transform.position = Vector3.Lerp(transform.position, transform.position + sadowtargetDist, time / (duration / 2));
            }
            else if(time < duration)
            {
                transform.localScale = Vector3.Lerp(targetHeight, startScale, time / duration);
                shadow.transform.localScale = Vector3.Lerp(sadowtargetScale, sadowStartScale, time / duration);
                shadow.transform.position = Vector3.Lerp(transform.position + sadowtargetDist, transform.position, time / duration);
            }

            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        transform.localScale = startScale;
        spawnlandingparticles();

        yield return new WaitForSeconds(0.2f);

        ChangeState(State.idle);
        isJumping = false;
    }

    // ========================== MISC ==========================

    // RESOURCES.LOAD THIS PARTICAL STUFF

    public void spawnlandingparticles()
    {
        Instantiate(particles, landingParticleSpawnPos.transform.position, Quaternion.identity, landingParticleSpawnPos.transform);
    }

    public void spawnstateparticles(GameObject ptype)
    {
        Instantiate(ptype, stateParticleSpawnPos.transform.position, Quaternion.identity, stateParticleSpawnPos.transform);
    }
}
