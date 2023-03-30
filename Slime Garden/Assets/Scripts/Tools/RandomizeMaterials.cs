using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RandomizeMaterials : MonoBehaviour
{
    [Button("Assign Random Materials")]
    private void AssignRandomMaterials()
    {
        var objectsToRandomize = GameObject.FindObjectsOfType<RandomMaterial>();

        foreach (RandomMaterial obj in objectsToRandomize)
        {
            obj.AssignRandomMat();
        }
    }
}
