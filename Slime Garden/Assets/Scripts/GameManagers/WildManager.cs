using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildManager : MonoBehaviour
{
    // Variables
    [SerializeField] private GameObject wildSlimePref;
    [SerializeField] private Vector2 zoneBorders;
    [SerializeField] private float spawnInterval;
    [SerializeField] private float spawnCoolDown;

    // Refrences
    private RandomSlime randSlime;

    void Start()
    {
        randSlime = this.GetComponent<RandomSlime>();
    }

    void Update()
    {
        if (spawnCoolDown > 0) spawnCoolDown -= Time.deltaTime;
        else
        {
            spawnCoolDown = spawnInterval;
            SpawnSlime();
        }
    }

    private void SpawnSlime()
    {
        Vector3 randPos = new Vector3(Random.Range(-zoneBorders.x, zoneBorders.x), 0, Random.Range(-zoneBorders.y, zoneBorders.y));
        GameObject newSLime = randSlime.CreateSlime(randPos, true);
        newSLime.GetComponent<SlimeController>().ChangeState(SlimeController.State.jump);

        Debug.Log("Spawned slime " + newSLime.GetComponent<SlimeData>().speciesName);
    }
}
