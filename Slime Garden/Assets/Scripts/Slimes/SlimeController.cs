﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using DG.Tweening;

public class SlimeController : MonoBehaviour
{
    //=============== COMPONENTS ===============
    public SlimeFaceSO slimeFace;               //The SO with slime facial expressions. Info is taken from here.
    private GameObject sprites;                 //The child gameobject that holds sprite objects
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
    private SlimeData sData;                    //Info about the slime
    private AudioSource audioSrc;
    [SerializeField] private AudioClip[] jumpSounds;
    [SerializeField] private AudioClip[] eatSounds;

    //=============== HELP VARIABLES ===============
    private bool stateChanged;                  // Bool to make sure initial state changes only happen once
    [SerializeField]
    private bool grounded;                      // Bool to make sure slime is grounded
    private float groundedBuffer;               // Timer to prevent odd behavior
    [SerializeField]
    private bool jumped;                        // Bool to see if slime has already jumped
    [SerializeField]    
    private float stateTimer;                   // Used to add pauses to certain states
    private GameObject stateParticles;          // Used to keep track of state particles so they can be disabled later
    private CropObj curFood;                 // Used to keep track or crop that is currently being eaten

    //=============== SLIME STATES ===============
    public enum State                           // All possible slime states
    {
        idle,
        jump,
        held,
        splice,
        sleep,
        love,
        play,
        crystalize,
        pet,
        eat,
        tamed
    }
    [SerializeField] private State state;       // This slimes current state
    public State GetState() { return state; }   // Get State function so it can stay private


    void Start()
    {
        // Find Brain and Data
        brain = this.GetComponent<SBrain>();
        sData = this.GetComponent<SlimeData>();

        // ========== Sprite stuff ==========
        sprites = this.transform.GetChild(0).gameObject;
        baseSlime = sprites.transform.GetChild(0).gameObject;
        pattern = sprites.transform.GetChild(1).gameObject;
        face = sprites.transform.GetChild(2).gameObject;
        // Sprite renderers
        Basesr = baseSlime.GetComponent<SpriteRenderer>();
        patternsr = pattern.GetComponent<SpriteRenderer>();
        facesr = face.GetComponent<SpriteRenderer>();
        // Sprite animators
        baseAnimator = baseSlime.GetComponent<Animator>();
        patternAnimator = pattern.GetComponent<Animator>();
        faceAnimator = face.GetComponent<Animator>();

        // Auido
        audioSrc = this.GetComponent<AudioSource>();
    }

    void Update()
    {
        // Grounded buffer
        if (groundedBuffer > 0) groundedBuffer -= Time.deltaTime;

        // State machine switch
        switch (state)
        {
            case State.idle:
                if (stateChanged)
                {
                    StartState("Idle", brain.slimeFaceDefault);
                }
                // Blink
                if (stateTimer > 0) stateTimer -= Time.deltaTime;
                if (stateTimer <= 0.2f && stateTimer > 0)
                    StartState("none", brain.slimeFaceSleep);
                else if (stateTimer <= 0)
                {
                    StartState("none", brain.slimeFaceDefault);
                    stateTimer = Random.Range(0.5f, 4f);
                }
                break;
            case State.jump:
                if (stateChanged)
                {
                    StartState("Held", brain.slimeFaceDefault);
                    JumpState();
                    stateTimer = 2f;
                }
                // If in jump state for too long, assume stuck and set idle
                if (stateTimer > 0) stateTimer -= Time.deltaTime;
                if (jumped && grounded || (stateTimer <= 0))
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
                    StartState("Sleep", brain.slimeFaceSleep, "Sleep");
                }
                break;
            case State.love:
                if (stateChanged)
                {
                    StartState("Hop", brain.slimeFaceHappy, "Hearts");
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
                    JumpState();
                    StartState("Held", brain.slimeFaceHappy);
                }
                if (jumped && grounded)
                {
                    StartState("Idle", brain.slimeFaceHappy);
                    jumped = false;
                    stateTimer = 0.4f;
                }
                break;
            case State.crystalize:
                if (stateChanged)
                {
                    StartState("Idle", brain.slimeFaceFocused);
                    stateTimer = 2f;
                }
                if (stateTimer > 0) stateTimer -= Time.deltaTime;
                if (stateTimer <= 0)
                {
                    brain.SpawnCS();
                    ChangeState(State.jump);
                }
                break;
            case State.pet:
                if (stateChanged)
                {
                    StartState("Hop", brain.slimeFaceHappy, "Hearts");
                    stateParticles.GetComponent<ParticleSystem>().Emit(8);
                    stateTimer = 1f;
                }
                if (stateTimer > 0) stateTimer -= Time.deltaTime;
                if (stateTimer <= 0)
                    ChangeState(State.idle);
                break;
            case State.eat:
                if (stateChanged)
                {
                    StartState("Hop", brain.slimeFaceDefault);
                    stateTimer = 1f;
                }
                if (stateTimer > 0) stateTimer -= Time.deltaTime;
                if (stateTimer <= 0)
                {
                    ChangeState(State.idle);

                    if (curFood != null)
                    {
                        // Attempt tame if wild slime Or feed if not
                        if (sData.isWild)
                            this.GetComponent<WildSlime>().AttemptTame(curFood);
                        else
                            sData.Feed(curFood);

                        curFood.DestroySelf();
                        audioSrc.PlayOneShot(eatSounds[Random.Range(0, eatSounds.Length)]);
                    }
                }
                break;
            case State.tamed:
                if (stateChanged)
                {
                    StartState("Hop", brain.slimeFaceHappy, "Hearts");
                    Basesr.DOFade(0, 2f);
                    patternsr.DOFade(0, 2f);
                    facesr.DOFade(0, 2f);
                }
                break;
        }
    }

    public void ChangeState(State newState)
    {
        state = newState;
        stateChanged = true;
        stateTimer = 0;
    }

    private void StartState(string sName, SlimeFaceSO sface, string particles = null)
    {
        // Changes the slime face, sets animations, and re-sets stateChanged bool
        // Be sure to set stateChanged to false if changing state without this function

        if (stateParticles != null)
            Destroy(stateParticles);

        this.slimeFace = sface;
        if(sData.isMature)
            face.GetComponent<SpriteLibrary>().spriteLibraryAsset = slimeFace.libraryAsset;
        else
            face.GetComponent<SpriteLibrary>().spriteLibraryAsset = slimeFace.babyLibraryAsset;

        if (sName != "none")
        {
            baseAnimator.SetTrigger(sName);
            patternAnimator.SetTrigger(sName);
            faceAnimator.SetTrigger(sName);
        }

        if(particles != null)
        {
            var par = Resources.Load<GameObject>("Particles/" + particles);
            var pos = new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z);
            stateParticles = Instantiate(par, pos, transform.rotation, transform);
        }

        stateChanged = false;
    }

    // ========================== JUMPING STATE ==========================

    public void JumpState()
    {
        Vector3 newPos = new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f));

        // TODO:
        // Here test if within bounds
        // Idea: raycast, if hits a bordeer fence, jump in opposite direction.

        // Flipping the sprite in the direction it jumps
        if ((newPos.x + transform.position.x) < transform.position.x)
            FlipSprite(false);
        else
            FlipSprite(true);

        jumped = true;
        SetGrounded(false);

        // Jump by force
        this.GetComponent<Rigidbody>().AddForce(newPos * 4, ForceMode.Impulse);

        audioSrc.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)]);
        Stretch();
    }


    // ========================== SPRITE STUFF ==========================
    private void FlipSprite(bool flip)
    {
        Basesr.flipX = flip;
        patternsr.flipX = flip;
        facesr.flipX = flip;
    }

    private void Squish()
    {
        ResetScale(sprites);

        sprites.transform.DOPunchScale(new Vector3(0.1f, -0.05f, 0), 0.3f, 5, 0.5f);
    }

    private void Stretch()
    {
        ResetScale(sprites);

        sprites.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.3f, 5, 0.5f);
    }

    private void ResetScale(GameObject obj)
    {
        obj.transform.DOKill();

        obj.transform.localScale = new Vector3(1, 1, 1);
    }


    // ========================== MISC ==========================

    private Vector3 validDirection = Vector3.down;
    private Vector2Int contactThresholds = new Vector2Int(160, 200);

    void OnCollisionStay(Collision col)
    {
        if (grounded || groundedBuffer > 0)
            return;

        // If surface is ground or ground object
        if (col.gameObject.tag == "Ground" || col.gameObject.tag == "GroundObj")
        {
            SetGrounded(true);
            return;
        }

        // if surface is mostly below
        for (int k = 0; k < col.contacts.Length; k++)
        {
            //Debug.Log("Hit angle " + Vector3.Angle(col.contacts[k].normal, validDirection));
            if (Vector3.Angle(col.contacts[k].normal, validDirection) >= contactThresholds.x &&
                Vector3.Angle(col.contacts[k].normal, validDirection) <= contactThresholds.y)
            {
                SetGrounded(true);
                return;
            }
        }
    }

    public void SetGrounded(bool isGrounded)
    {
        groundedBuffer = 0.2f;
        grounded = isGrounded;
        Squish();
    }

    public bool FeedSlime(CropObj food)
    {
        if (!InNonInteruptableState())
        {
            ChangeState(State.eat);
            curFood = food;
            return true;
        }

        return false;
    }

    public void ReleaseSlime()
    {
        var player = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
        if(sData.isMature)
            player.GainMoney((int)sData.sRarity * 4);
        else
            player.GainMoney((int)sData.sRarity);

        ChangeState(State.tamed);
        Destroy(gameObject, 2f);
        sData.RemoveFromHabitat();
    }

    public bool InNonInteruptableState()
    {
        if (state == State.held)
            return true;
        else if (state == State.splice)
            return true;
        else if (state == State.jump)
            return true;
        else if (state == State.crystalize)
            return true;
        else if (state == State.eat)
            return true;
        else if (state == State.tamed)
            return true;

        return false;
    }

    private void OnDestroy()
    {
        Basesr.DOKill();
        patternsr.DOKill();
        facesr.DOKill();

        sprites.transform.DOKill();
    }
}