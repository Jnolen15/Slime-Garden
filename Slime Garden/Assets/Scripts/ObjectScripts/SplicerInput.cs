﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplicerInput : MonoBehaviour
{
    private SplicerScript splicer;
    private SlimeController slimeControll;
    private SlimeData slimeData;
    public Transform slimePos;
    public GameObject currentSlime;
    public bool occupied = false;
    public string sBaseColor;
    public string sPatternColor;
    public string sPattern;
    public float sRarity;
    public bool sSpecial = false;

    // STUFF FOR COLOR CHANGING
    [SerializeField] private Material mat;
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

    private ParticleSystem particles;

    void Start()
    {
        splicer = this.transform.parent.gameObject.GetComponent<SplicerScript>();

        particles = GetComponent<ParticleSystem>();

        mat = this.GetComponentInChildren<MeshRenderer>().material;
    }

    private void Update()
    {
        mat.SetColor("_HighlightColor", Color.Lerp(mat.GetColor("_HighlightColor"), currentColor, 0.01f));
    }

    // When a slime enters this inputs trigger, it gets its info.
    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag != "Slime" || occupied)
            return;

        if (!col.gameObject.GetComponent<DragDrop>().isHeld && col.gameObject.GetComponent<SlimeData>().isMature)
        {
            // Set up
            occupied = true;
            currentSlime = col.gameObject;
            slimeControll = currentSlime.GetComponent<SlimeController>();
            slimeControll.ChangeState(SlimeController.State.splice);
            slimeData = currentSlime.GetComponent<SlimeData>();
            currentSlime.transform.localPosition = slimePos.position;

            // Get slime attributes
            sBaseColor = slimeData.sBaseColor;
            sPatternColor = slimeData.sPatternColor;
            sPattern = slimeData.slimeSpeciesPattern.sPattern;
            sRarity = slimeData.sRarity;
            sSpecial = slimeData.slimeSpeciesBase.sSpecial;

            // Change color of lights to base color of input slime
            SetColor(sBaseColor);

            // Start Particles
            particles.Play();
            ParticleSystem.MainModule settings = particles.GetComponent<ParticleSystem>().main;
            settings.startColor = new ParticleSystem.MinMaxGradient(currentColor);
        }
    }

    // When a slime is taken out or ejected it resets its data feilds
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject == currentSlime)
        {
            Eject();
        }
    }

    // Resets fields
    public void Eject()
    {
        if (currentSlime != null)
        {
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
        particles.Stop();
    }

    private void OnDestroy()
    {
        Eject();
    }

    private void SetColor(string changeTo)
    {
        switch (changeTo)
        {
            default:
                Debug.LogError("BASE COLOR NOT RECOGNIZED" + changeTo);
                break;
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
