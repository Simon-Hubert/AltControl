using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AView : MonoBehaviour
{
    [field: SerializeField] public float Weight { get; set; }
    [field: SerializeField] public bool IsActiveOnStart { get; set; }
    
    public abstract CameraConfiguration GetConfiguration();

    private void Start() {
        if(IsActiveOnStart) SetActive(true);
    }

    public void SetActive(bool isActive) {
        if (isActive) CameraController.instance.AddView(this);
        else CameraController.instance.RemoveView(this);
    }

    private void OnDrawGizmosSelected() {
        GetConfiguration().DrawGizmo(Color.green);
    }
}
