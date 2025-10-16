using System;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    private Transform _cam;

    private void Awake() {
        _cam = Camera.main.transform;
    }

    private void Update() {
        transform.LookAt(_cam,Vector3.up);
    }
}
