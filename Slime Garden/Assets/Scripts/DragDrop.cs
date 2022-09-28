using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public SlimeController sc;
    public SBrain sb;
    private float startPosx;
    private float startPosy;
    public bool isHeld = false;

    private void Start()
    {
        sb = GameObject.FindGameObjectWithTag("Brain").GetComponent<SBrain>();
    }

    private void Update()
    {
        if (isHeld)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPosx, mousePos.y - startPosy, 0);
        }
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && !sc.isJumping)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            startPosx = mousePos.x - this.transform.localPosition.x;
            startPosy = mousePos.y - this.transform.localPosition.y;

            isHeld = true;
            sb.isHeld(this.transform.gameObject);
        }
    }

    private void OnMouseUp()
    {
        if (isHeld)
        {
            isHeld = false;
            sc.Basesr.sprite = sc.slimeSpeciesBase.sIdle;
            sc.patternsr.sprite = sc.slimeSpeciesPattern.sPatternIdle;
            sc.facesr.sprite = sc.slimeFace.faceIdle;
            sc.spawnlandingparticles();
            if (sc.state != "splice")
                sc.state = "idle";
        }
    }
}
