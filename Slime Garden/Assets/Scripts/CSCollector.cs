using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CSCollector : MonoBehaviour, IInteractable
{
    public int maxValue;
    [SerializeField] private int storedValue;
    [SerializeField] private CSCollectionRadious collectionRadious;
    [SerializeField] private Transform collector;
    [SerializeField] private Transform ejectionPoint;
    [SerializeField] private GameObject crystal;
    [SerializeField] private Material[] mats;
    private bool animatingCollection;
    private PlayerData pData;

    void Start()
    {
        pData = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<PlayerData>();
    }

    public void Interact()
    {
        if (storedValue > 0 && !animatingCollection)
        {
            StartCoroutine(AnimateCollection());
        }
    }

    public IEnumerator StoreCS(CongealedSlime cs)
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        if (cs != null)
        {
            Squish();
            storedValue += cs.value;
            cs.Store();
        }
    }

    public IEnumerator AnimateCollection()
    {
        animatingCollection = true;

        pData.GainMoney(storedValue);

        var animValue = storedValue;
        storedValue = 0;

        for (int i = 0; i < animValue; i+=5)
        {
            var cs = Instantiate(crystal, ejectionPoint.position, Random.rotation);

            Squish();

            cs.GetComponent<MeshRenderer>().material = mats[Random.Range(0, mats.Length)];

            // Add some force to crystal
            Vector3 newPos = new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f));
            cs.GetComponent<Rigidbody>().AddForce(newPos * 4, ForceMode.Impulse);

            yield return new WaitForSeconds(0.15f);
        }

        animatingCollection = false;
    }

    private void Squish()
    {
        ResetScale();

        collector.DOPunchScale(new Vector3(0.1f, 0.1f, 0), 0.1f, 5, 0.5f);
    }

    private void ResetScale()
    {
        collector.DOKill();

        collector.localScale = new Vector3(1, 1, 1);
    }
}
