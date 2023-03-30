using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewStageManager : MonoBehaviour
{
    [SerializeField] private float[] zoomLevels;
    [SerializeField] private Transform stagePos;
    [SerializeField] private Transform camPos;
    private Transform visual;

    public void Setup(PlaceableObjectSO placeableData)
    {
        // Instantiate and center visual prefab
        if (visual != null)
            Destroy(visual.gameObject);

        visual = Instantiate(placeableData.visual, stagePos.position, stagePos.rotation, stagePos);
        visual.transform.localPosition = new Vector3((placeableData.width * -0.5f), 0, (placeableData.height * -0.5f));

        // Adjust zoom levels
        int size = placeableData.width;
        if (size < placeableData.height)
            size = placeableData.height;

        if (size <= 1)
            camPos.localPosition = new Vector3(0, 2, -2);
        else if (size == 2)
            camPos.localPosition = new Vector3(0, 3, -3);
        else if (size == 3)
            camPos.localPosition = new Vector3(0, 4, -4);
        else if (size == 4)
            camPos.localPosition = new Vector3(0, 5, -5);
        else if (size >= 5)
            camPos.localPosition = new Vector3(0, 6, -6);
    }
}
