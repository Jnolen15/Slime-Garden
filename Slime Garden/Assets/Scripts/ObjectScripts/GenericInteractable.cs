using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent intereactEvent;

    public void Interact()
    {
        intereactEvent.Invoke();
    }
}
