using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinishZone : MonoBehaviour
{
    public event UnityAction levelFinnished;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerBody>())
        {
            levelFinnished.Invoke();
        }
    }
}
