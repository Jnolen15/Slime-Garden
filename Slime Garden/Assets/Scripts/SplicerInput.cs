using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplicerInput : MonoBehaviour
{
    private SplicerScript splicer;
    private SlimeController slimeControll;

    public GameObject currentSlime;

    public bool occupied = false;
    public string sBaseColor;
    public string sPatternColor;
    public string sPattern;
    public float sRarity;
    public bool sSpecial = false;

    // STUFF FOR COLOR CHANGING
    private SpriteRenderer lightsSR;
    public Color defaultColor;
    public Color currentColor;
    public Color Amethyst;
    public Color Aquamarine;
    public Color Bixbite;
    public Color Citrine;
    public Color Emerald;
    public Color Jade;
    public Color Obsidian;
    public Color Peridot;
    public Color Quartz;
    public Color Ruby;
    public Color Sapphire;
    public Color Topaz;
    public Color Diamond;

    public ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        // Get access to the main splicerr script which does the generation
        splicer = this.transform.parent.gameObject.GetComponent<SplicerScript>();

        // Get access to the SR of the Ligths object so we can change its color
        lightsSR = this.transform.GetChild(0).GetComponent<SpriteRenderer>();

        particles = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        lightsSR.color = Color.Lerp(lightsSR.color, currentColor, 0.01f);
    }

    // When a slime enters this inputs trigger, it gets its info.
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!occupied)
        {
            // Get slime attributes
            currentSlime = col.gameObject;
            slimeControll = col.gameObject.GetComponent<SlimeController>();
            slimeControll.ChangeState(SlimeController.State.splice);
            slimeControll.gameObject.GetComponent<DragDrop>().isHeld = false;
            Vector3 offset = new Vector3(0, 0.1f, 0);
            slimeControll.transform.position = this.transform.position + offset;
            occupied = true;
            sBaseColor = slimeControll.sBaseColor;
            sPatternColor = slimeControll.sPatternColor;
            sPattern = slimeControll.slimeSpeciesPattern.sPattern;
            sRarity = slimeControll.sRarity;
            sSpecial = slimeControll.slimeSpeciesBase.sSpecial;

            // Change color of lights to base color of input slime
            SetColor(sBaseColor);

            // Start Particles
            particles.Play();
            ParticleSystem.MainModule settings = particles.GetComponent<ParticleSystem>().main;
            settings.startColor = new ParticleSystem.MinMaxGradient(currentColor);

        } else // If a slime is already in the input it ejects slime to default pos
        {
            slimeControll = col.gameObject.GetComponent<SlimeController>();
            slimeControll.gameObject.GetComponent<DragDrop>().isHeld = false;
            slimeControll.transform.position = new Vector3(0f, 0f, 0f);
        }
    }

    // When a slime is taken out or ejected it resets its data feilds
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == currentSlime)
        {
            splicer.Removed();

            slimeControll = col.gameObject.GetComponent<SlimeController>();
            slimeControll.ChangeState(SlimeController.State.idle);
            occupied = false;

            currentSlime = null;
            sBaseColor = null;
            sPatternColor = null;
            sPattern = null;
            sSpecial = false;

            currentColor = defaultColor;

            particles.Stop();
        }
        
    }

    // Sends slime out to default pos
    public void Eject()
    {
        if (currentSlime != null)
        {
            Vector3 targetPosition = new Vector3(Random.Range(-1f, 1f), -1.5f, 0f);
            targetPosition += currentSlime.GetComponent<SlimeController>().transform.position;
            //currentSlime.GetComponent<SlimeController>().JumpToo(targetPosition);
            currentSlime.GetComponent<SlimeController>().ChangeState(SlimeController.State.idle);
            particles.Stop();
            occupied = false;
            currentSlime = null;
            sBaseColor = null;
            sPatternColor = null;
            sPattern = null;
            sSpecial = false;
        }

        currentColor = defaultColor;
    }

    private void SetColor(string changeTo)
    {
        switch (changeTo)
        {
            case "Amethyst":
                currentColor = Amethyst;
                break;
            case "Aquamarine":
                currentColor = Aquamarine;
                break;
            case "Bixbite":
                currentColor = Bixbite;
                break;
            case "Citrine":
                currentColor = Citrine;
                break;
            case "Emerald":
                currentColor = Emerald;
                break;
            case "Jade":
                currentColor = Jade;
                break;
            case "Obsidian":
                currentColor = Obsidian;
                break;
            case "Peridot":
                currentColor = Peridot;
                break;
            case "Quartz":
                currentColor = Quartz;
                break;
            case "Ruby":
                currentColor = Ruby;
                break;
            case "Sapphire":
                currentColor = Sapphire;
                break;
            case "Topaz":
                currentColor = Topaz;
                break;
            case "Diamond":
                currentColor = Diamond;
                break;
        }
    }
}
