using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    public static UnityEvent finishEvents;
    private void Awake()
    {
        if(finishEvents == null)
        {
            finishEvents = new UnityEvent();
        }
    }
    private void LateUpdate()
    {
        finishEvents.Invoke();
    }
}
