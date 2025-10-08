using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredViewVolume : AViewVolume
{
    [SerializeField] string _target;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(_target)) SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag(_target)) SetActive(false);
    }
}
