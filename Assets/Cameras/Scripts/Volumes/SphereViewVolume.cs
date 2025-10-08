using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereViewVolume : AViewVolume
{
    [field : SerializeField] public GameObject Target { get; private set; }
    [field : SerializeField] public float OuterRadius { get; private set; }
    [field : SerializeField] public float InnerRadius { get; private set; }

    private float _distance;

    private void Update()
    {
        _distance = Vector3.Distance(Target.transform.position, transform.position);
        if (_distance <= OuterRadius && !isActive)
        {
            SetActive(true);
        }
        else if (_distance > OuterRadius && isActive)
        {
            SetActive(false);
        }
    }

    public override float ComputeSelfWeight() => Mathf.InverseLerp(OuterRadius, InnerRadius, _distance);
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.5f, 0, 1, 1f);
        Gizmos.DrawWireSphere(transform.position, InnerRadius);
        Gizmos.color = new Color(0f, 0, 1, 1f);
        Gizmos.DrawWireSphere(transform.position, OuterRadius);
    }
    
}
