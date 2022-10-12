using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSDestroy : MonoBehaviour
{
    public float destroyTime = 3f;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().sortingLayerName = "Particles";

        Destroy(gameObject, destroyTime);
    }
}
