using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeathParticles : MonoBehaviour
{
    [SerializeField] private float deathTimer;
    [SerializeField] private GameObject deathParticles;

    private void Start()
    {
        StartCoroutine(DestroySelf());
    }

    private IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(deathTimer);
        
        Instantiate(deathParticles, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
