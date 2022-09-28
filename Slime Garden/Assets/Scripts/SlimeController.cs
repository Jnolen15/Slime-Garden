using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    // COMPONENTS ===============
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
    public DragDrop dragDrop;                   //The Drag and Drop script
    private SBrain brain;                       //The Slime Brain Script

    // SCRIPTABLE OBJECT ===============
    //public SlimeSO slimeSpecies;              //The SO of this slime species. Info is taken from here.
    public SlimeBaseSO slimeSpeciesBase;        //The SO of this slime species Base color. Info is taken from here.
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

    // OTHER ===============
    private bool inspliceState = false;         //Bool for splice state
    private bool inidleState = false;           //Bool for idle state
    private bool insleepState = false;          //Bool for sleep state
    private bool inloveState = false;           //Bool for love state
    private bool inplayState = false;           //Bool for play state
    public bool isJumping = false;              //Bool for jumping (used to switch to jump sprite)

    public string state = "idle";               //The string that denotes which state the slime should be in
    public string queuedState = "idle";         //The string that denotes which state the slime should be in next

    private float spliceCD = 0.4f;              //Cooldown between splice animation cycles
    private float idleCD = 0.2f;                //Cooldown between idle animation cycles
    private float sleepCD = 2f;                 //Cooldown between sleep animation cycles
    private float loveCD = 0.2f;                //Cooldown between love animation cycles

    public float habitatX = 10;                 //X postiton of habitat bounds
    public float habitatY = 10;                 //Y postiton of habitat bounds

    public Sprite shadow1;
    public Sprite shadow2;

    // Start is called before the first frame update
    void Start()
    {
        pattern = this.transform.GetChild(0).gameObject;
        landingParticleSpawnPos = this.transform.GetChild(1).gameObject;
        shadow = this.transform.GetChild(2).gameObject;
        face = this.transform.GetChild(3).gameObject;
        stateParticleSpawnPos = this.transform.GetChild(4).gameObject;
        Basesr = gameObject.GetComponent<SpriteRenderer>();
        patternsr = pattern.GetComponent<SpriteRenderer>();
        shadowsr = shadow.GetComponent<SpriteRenderer>();
        facesr = face.GetComponent<SpriteRenderer>();
        dragDrop = gameObject.GetComponent<DragDrop>();
        brain = GameObject.FindGameObjectWithTag("Brain").GetComponent<SBrain>();

        speciesName = slimeSpeciesBase.sName + " " + sPatternColor + " " + slimeSpeciesPattern.sPattern;
        sRarity = slimeSpeciesBase.sRarity + slimeSpeciesPattern.sRarity;
        sBaseColor = slimeSpeciesBase.sBaseColor;
        //sPatternColor = slimeSpeciesPattern.sPatternColor;
        sPattern = slimeSpeciesPattern.sPattern;
        sSpecial = slimeSpeciesBase.sSpecial;
        Basesr.sprite = slimeSpeciesBase.sSprite;
        patternsr.sprite = slimeSpeciesPattern.sPatternSprite;
        patternsr.color = slimeSpeciesPatternColor;
        particles = slimeSpeciesBase.sParticles;
    }

    // Update is called once per frame
    /* In here i add (int)(100 * -transform.position.y) to the sorting layer.
     * I do this so slimes that are closer to the bottom of the screen
     * appear above ones that are further back.
     * THIS IS A BANDAID SOLUTION
     * A better solution to this should be found
     * RN the most promising idea would just be to make the game 3D
     * I can still have 2d assets and colliders ect.
     * 
     * Acctualy its kinda OK. so maybe fine now
    */
    void Update()
    {
        // Update sorting layer
        Basesr.sortingOrder = 0 + (int)(100 * -transform.position.y);
        patternsr.sortingOrder = 1 + (int)(100 * -transform.position.y);
        facesr.sortingOrder = 2 + (int)(100 * -transform.position.y);

        if (dragDrop.isHeld)
        {
            StopAllCoroutines();
            state = "held";
            HeldState();
        }

        //if(state != "jump") isJumping = false;

        // Splice State
        if (state == "splice" && !inspliceState)
            SpliceState();

        if (inspliceState && state != "splice")
            inspliceState = false;

        // Idle State
        if (state == "idle" && !inidleState)
            IdleState();

        if (inidleState && state != "idle")
            inidleState = false;

        // Sleep State
        if (state == "sleep" && !insleepState)
            SleepState();

        if (insleepState && state != "sleep")
            insleepState = false;

        // Love state
        if (state == "love" && !inloveState)
            LoveState();

        if (inloveState && state != "love")
            inloveState = false;

        // Play state
        if (state == "play" && !isJumping)
            PlayState();

        if (inplayState && state != "play")
        {
            state = queuedState;
            inplayState = false;
        }

        if (state == "jump")
        {
            JumpState();
        }
    }

    void FixedUpdate()
    {

        // this is where the if (state == "jump") thing used to be. I dont remeber why

    }

    // ========================== HELD STATE ==========================

    public void HeldState()
    {
        isJumping = false;
        Basesr.sprite = slimeSpeciesBase.sJump;
        patternsr.sprite = slimeSpeciesPattern.sPatternJump;
        facesr.sprite = slimeFace.faceJump;
        shadowsr.sprite = shadow1;
        this.slimeFace = brain.slimeFaceDefault;
    }

    // ========================== SPLICE STATE ==========================

    public void SpliceState()
    {
        inspliceState = true;
        this.slimeFace = brain.slimeFaceDefault;
        StartCoroutine(spliceAnimation());
    }

    IEnumerator spliceAnimation()
    {
        while (state == "splice")
        {
            // switches between sprites each half second
            Basesr.sprite = slimeSpeciesBase.sSprite;
            patternsr.sprite = slimeSpeciesPattern.sPatternSprite;
            facesr.sprite = slimeFace.faceSprite;
            shadowsr.sprite = shadow1;
            if (state == "splice")
                yield return new WaitForSeconds(spliceCD);
            else
                yield break;
            Basesr.sprite = slimeSpeciesBase.sIdle;
            patternsr.sprite = slimeSpeciesPattern.sPatternIdle;
            facesr.sprite = slimeFace.faceIdle;
            shadowsr.sprite = shadow2;
            if (state == "splice")
                yield return new WaitForSeconds(spliceCD);
            else
                yield break;
            Basesr.sprite = slimeSpeciesBase.sSprite;
            patternsr.sprite = slimeSpeciesPattern.sPatternSprite;
            facesr.sprite = slimeFace.faceSprite;
            shadowsr.sprite = shadow1;
            if (state == "splice")
                yield return new WaitForSeconds(spliceCD);
            else
                yield break;
        }
    }

    // ========================== IDLE STATE ==========================

    public void IdleState()
    {
        inidleState = true;
        this.slimeFace = brain.slimeFaceDefault;
        StartCoroutine(IdleAnimation());
    }

    IEnumerator IdleAnimation()
    {
        while (state == "idle" && state == queuedState)
        {
            // switches between sprites each half second
            Basesr.sprite = slimeSpeciesBase.sSprite;
            patternsr.sprite = slimeSpeciesPattern.sPatternSprite;
            facesr.sprite = slimeFace.faceSprite;
            shadowsr.sprite = shadow1;
            if (state == "idle")
                yield return new WaitForSeconds(idleCD);
            else
                yield break;
            Basesr.sprite = slimeSpeciesBase.sIdle;
            patternsr.sprite = slimeSpeciesPattern.sPatternIdle;
            facesr.sprite = slimeFace.faceIdle;
            shadowsr.sprite = shadow2;
            if (state == "idle")
                yield return new WaitForSeconds(idleCD);
            else
                yield break;
            Basesr.sprite = slimeSpeciesBase.sSprite;
            patternsr.sprite = slimeSpeciesPattern.sPatternSprite;
            facesr.sprite = slimeFace.faceSprite;
            shadowsr.sprite = shadow1;
            if (state == "idle")
                yield return new WaitForSeconds(idleCD);
            else
                yield break;
            idleCD = 0.3f + Random.Range(0f, 0.4f);
        }
        state = queuedState;
        yield break;
    }

    // ========================== JUMPING STATE ==========================

    public void JumpState()
    {
        this.slimeFace = brain.slimeFaceDefault;
        if (isJumping == false && state == "jump" && state == queuedState)
        {
            isJumping = true;

            Basesr.sprite = slimeSpeciesBase.sJump;
            patternsr.sprite = slimeSpeciesPattern.sPatternJump;
            facesr.sprite = slimeFace.faceJump;
            shadowsr.sprite = shadow1;

            Vector3 newPos = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);             //Makes a random position ro jump to
            newPos = newPos + transform.position;
            int count = 0;
            while (newPos.x > habitatX || newPos.y > habitatY || newPos.x < -habitatX || newPos.y < -habitatY) // Testing to make sure newPos is in habitat bounds
            {
                if (count > 20)
                {
                    //Debug.Log("Slime out of bounds. Repositioning");
                    transform.position = new Vector3(0f, 0f, 0f);
                }
                //Debug.Log("NewPos was not within bounds. Rerolling.");
                newPos = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f); // If not, get a new pos
                newPos = newPos + transform.position;
                count += 1;
            }
            float hypotenuse = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(transform.position.x - newPos.x), 2)    //Uses Pythagorean theorem to get hypotenuse
                                    + Mathf.Pow(Mathf.Abs(transform.position.y - newPos.y), 2));
            float jumpDuration = hypotenuse / 1.5f;            // Calculating jump speed.
            float maxHeight = 1f + (hypotenuse / 3);  //hypotenuse / 3 to get the jump height because this makes for a larger height the larger the jump.
            Vector3 height = new Vector3(maxHeight, maxHeight, 0f);

            if (newPos.x < transform.position.x) // Flipping the sprite in the direction it jumps
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
            state = "idle";
            queuedState = "idle";
        }
    }

    // Used by other scripts to make a slime jump to a specific location
    public void JumpToo(Vector3 targetPosition)
    {
        isJumping = true;

        Basesr.sprite = slimeSpeciesBase.sJump;
        patternsr.sprite = slimeSpeciesPattern.sPatternJump;
        facesr.sprite = slimeFace.faceJump;
        shadowsr.sprite = shadow1;

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

    IEnumerator Jump(Vector3 targetPosition, Vector3 targetScale, float duration)
    {
        /* Jump Code
         * targetPosition - Landing spot
         * targetScale - Max height it will reach in jump
         * duration - length of jump
         * This uses Lerp to smoothly move the slime to its target position.
         * It will also lerp up to targetScale in the first half
         * then back to normal scale duing the second half.
         * This makes it seem like the slime is moving upwards. 
         */

        float time = 0;
        Vector3 startPosition = transform.position;
        Vector3 startScale = transform.localScale;
        Vector3 shadowstartPosition = shadow.transform.position;
        Vector3 sadowStartScale = shadow.transform.localScale;
        Vector3 sadowtargetScale = new Vector3(0.5f - (Mathf.Abs(targetScale.x) - 1), 0.5f - (Mathf.Abs(targetScale.y) - 1), 0f);
        Vector3 sadowtargetDist = new Vector3(0f, -(Mathf.Abs(targetScale.y) - 1), 0f);

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);

            if(time < duration / 2)
            {
                transform.localScale = Vector3.Lerp(startScale, targetScale, time / (duration / 2));
                shadow.transform.localScale = Vector3.Lerp(sadowStartScale, sadowtargetScale, time / (duration / 2));
                shadow.transform.position = Vector3.Lerp(transform.position, transform.position + sadowtargetDist, time / (duration / 2));
            }
            else if(time < duration)
            {
                transform.localScale = Vector3.Lerp(targetScale, startScale, time / duration);
                shadow.transform.localScale = Vector3.Lerp(sadowtargetScale, sadowStartScale, time / duration);
                shadow.transform.position = Vector3.Lerp(transform.position + sadowtargetDist, transform.position, time / duration);
            }

            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        transform.localScale = startScale;
        Basesr.sprite = slimeSpeciesBase.sIdle;
        patternsr.sprite = slimeSpeciesPattern.sPatternIdle;
        facesr.sprite = slimeFace.faceIdle;
        spawnlandingparticles();

        yield return new WaitForSeconds(0.2f);

        isJumping = false;
    }

    // ========================== SLEEP STATE ==========================

    public void SleepState()
    {
        insleepState = true;
        spawnstateparticles(stateParticles);
        this.slimeFace = brain.slimeFaceSleep;
        StartCoroutine(SleepAnimation());
    }

    IEnumerator SleepAnimation()
    {
        while (state == "sleep" && state == queuedState)
        {
            // switches between sprites each half second
            Basesr.sprite = slimeSpeciesBase.sSprite;
            patternsr.sprite = slimeSpeciesPattern.sPatternSprite;
            facesr.sprite = slimeFace.faceSprite;
            shadowsr.sprite = shadow1;
            if (state == "sleep")
                yield return new WaitForSeconds(sleepCD);
            else
                yield break;
            Basesr.sprite = slimeSpeciesBase.sIdle;
            patternsr.sprite = slimeSpeciesPattern.sPatternIdle;
            facesr.sprite = slimeFace.faceIdle;
            shadowsr.sprite = shadow2;
            if (state == "sleep")
                yield return new WaitForSeconds(sleepCD);
            else
                yield break;
            Basesr.sprite = slimeSpeciesBase.sSprite;
            patternsr.sprite = slimeSpeciesPattern.sPatternSprite;
            facesr.sprite = slimeFace.faceSprite;
            shadowsr.sprite = shadow1;
            if (state == "sleep")
                yield return new WaitForSeconds(sleepCD);
            else
                yield break;
        }
        state = queuedState;
        yield break;
    }

    // ========================== LOVE STATE ==========================

    public void LoveState()
    {
        inloveState = true;
        spawnstateparticles(stateParticles);
        this.slimeFace = brain.slimeFaceHappy;
        StartCoroutine(LoveAnimation());
    }

    IEnumerator LoveAnimation()
    {
        while (state == "love" && state == queuedState)
        {
            Basesr.sprite = slimeSpeciesBase.sSprite;
            patternsr.sprite = slimeSpeciesPattern.sPatternSprite;
            facesr.sprite = slimeFace.faceSprite;
            shadowsr.sprite = shadow1;
            if (state == "love")
                yield return new WaitForSeconds(loveCD);
            else
                yield break;
            Basesr.sprite = slimeSpeciesBase.sIdle;
            patternsr.sprite = slimeSpeciesPattern.sPatternIdle;
            facesr.sprite = slimeFace.faceIdle;
            shadowsr.sprite = shadow2;
            if (state == "love")
                yield return new WaitForSeconds(loveCD);
            else
                yield break;
            Basesr.sprite = slimeSpeciesBase.sSprite;
            patternsr.sprite = slimeSpeciesPattern.sPatternSprite;
            facesr.sprite = slimeFace.faceSprite;
            shadowsr.sprite = shadow1;
            if (state == "love")
                yield return new WaitForSeconds(loveCD);
            else
                yield break;
            Basesr.sprite = slimeSpeciesBase.sJump;
            patternsr.sprite = slimeSpeciesPattern.sPatternJump;
            facesr.sprite = slimeFace.faceJump;
            shadowsr.sprite = shadow1;
            if (state == "love")
                yield return new WaitForSeconds(loveCD);
            else
                yield break;
        }
        state = queuedState;
        yield break;
    }

    // ========================== PLAY STATE ========================== THIS STATE IS CURRENTLY EMPTY. REMOVE OR ADD TOO

    public void PlayState()
    {
        if (state == "play" && state == queuedState)
        {
            isJumping = true;
            inplayState = true;
            //Make a random position ro jump to
            Vector3 newPos = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
            newPos += transform.position;
            int count = 0;
            while (newPos.x > habitatX || newPos.y > habitatY || newPos.x < -habitatX || newPos.y < -habitatY)
            {
                if (count > 20)
                {
                    Debug.Log("Slime out of bounds. Repositioning");
                    transform.position = new Vector3(0f, 0f, 0f);
                }
                newPos = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f) + transform.position;
                count++;
            }
            JumpToo(newPos);
        } else
        {
            state = queuedState;
        }
    }

    // ========================== MISC ==========================

    public void spawnlandingparticles()
    {
        Instantiate(particles, landingParticleSpawnPos.transform.position, Quaternion.identity, landingParticleSpawnPos.transform);
    }

    public void spawnstateparticles(GameObject ptype)
    {
        Instantiate(ptype, stateParticleSpawnPos.transform.position, Quaternion.identity, stateParticleSpawnPos.transform);
    }
}
