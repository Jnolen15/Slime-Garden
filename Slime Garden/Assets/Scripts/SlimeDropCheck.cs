using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlimeDropCheck : MonoBehaviour
{
    public bool collision;
    public Renderer arrow;
    [SerializeField] Material blueGhost;
    [SerializeField] Material redGhost;

    public void Activate()
    {
        arrow.gameObject.SetActive(true);
        arrow.transform.DOLocalMove(new Vector3(0, 0.25f, 0), 2f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        arrow.transform.DOLocalRotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

    public void Deactivate()
    {
        arrow.transform.DOKill();
        arrow.transform.localPosition = new Vector3(0, -2, 0);
        arrow.transform.localRotation = Quaternion.identity;
        arrow.gameObject.SetActive(false);
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
            return;

        collision = true;
        arrow.material = redGhost;
    }

    private void OnTriggerExit(Collider other)
    {
        collision = false;
        arrow.material = blueGhost;
    }
}
