using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalViewVolume : AViewVolume
{
    [SerializeField, Range(0,1)] private float _weight;
    
    private void Start()
    {
        SetActive(true);
    }

    public override float ComputeSelfWeight() => _weight;
}
