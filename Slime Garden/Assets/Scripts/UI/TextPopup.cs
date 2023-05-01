using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TextPopup : MonoBehaviour
{
    public void setup(string str, Color color)
    {
        var tmp = this.GetComponent<TextMeshProUGUI>();
        tmp.text = str;
        tmp.color = color;

        tmp.DOFade(0, 1.2f);
        this.transform.DOMoveY(transform.position.y + 30, 1.2f).OnComplete(() => { Remove(); });
    }

    public void Remove()
    {
        this.transform.DOKill();
        Destroy(this.gameObject);
    }
}
