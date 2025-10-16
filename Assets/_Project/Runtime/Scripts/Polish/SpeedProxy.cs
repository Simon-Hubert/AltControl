using System;
using UnityEngine;

public class SpeedProxy : MonoBehaviour
{
    private Rigidbody _rb;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
    }


    public float ForwardSpeed => Vector3.Dot(_rb.linearVelocity, transform.forward);
    public float SideSpeed => Vector3.Dot(_rb.linearVelocity, transform.right);
}
