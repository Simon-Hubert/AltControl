using System;
using UnityEngine;

public class WheelAnimator : MonoBehaviour
{
    [SerializeField] private WheelCollider _collider;
    [SerializeField] private Transform _wheel;

    private void Update()
    {
        _wheel.transform.Rotate(0,0, _collider.rotationSpeed / 60 * 360 * Time.deltaTime);
    }
}
