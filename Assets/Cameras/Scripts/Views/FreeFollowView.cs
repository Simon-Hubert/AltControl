using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FreeFollowView : AView
{
    [SerializeField] private float[] _pitch = new float[3];
    [SerializeField] private float[] _roll= new float[3];
    [SerializeField] private float[] _fov= new float[3];
    
    [SerializeField] private float _yaw;
    [SerializeField] private float _yawSpeed;
    [SerializeField] private Transform _target;
    [SerializeField] private float _curvePosition;
    [SerializeField] private float _curveSpeed;
    
    [SerializeField] private Curve _curve;

    [SerializeField] private InputActionReference _axis;

    private Vector2 _axisInput;
    
    
    private void OnEnable() {
       _axis.action.performed += HandleAxis;
       _axis.action.canceled += HandleAxis;
    }

    private void OnDisable() {
       _axis.action.performed -= HandleAxis;
       _axis.action.canceled -= HandleAxis;
    }

    private void HandleAxis(InputAction.CallbackContext context) {
        _axisInput = context.ReadValue<Vector2>();
    }

    private void Update() {
        _yaw += _yawSpeed * _axisInput.x * Time.deltaTime;
        _curvePosition += _curveSpeed * _axisInput.y * Time.deltaTime;
        _curvePosition = Mathf.Clamp(_curvePosition, 0, 1);
    }

    public override CameraConfiguration GetConfiguration() {

        float pitch = _curvePosition < .5f ? Mathf.Lerp(_pitch[0], _pitch[1], _curvePosition * 2) : Mathf.Lerp(_pitch[1], _pitch[2], _curvePosition * 2 - 1);
        float roll = _curvePosition < .5f ? Mathf.Lerp(_roll[0], _roll[1], _curvePosition * 2) : Mathf.Lerp(_roll[1], _roll[2], _curvePosition * 2 - 1);
        float fov = _curvePosition < .5f ? Mathf.Lerp(_fov[0], _fov[1], _curvePosition * 2) : Mathf.Lerp(_fov[1], _fov[2], _curvePosition * 2 - 1);
        
        return new CameraConfiguration
        {
            Yaw = _yaw,
            Pitch = pitch,
            Roll = roll,
            Fov = fov,
            Pivot = _target.position + Quaternion.Euler(0,_yaw,0)* _curve.GetPosition(_curvePosition, transform.localToWorldMatrix),
            Distance = 0
        };
    }

    private void OnDrawGizmos() {
        _curve.DrawGizmo(Color.yellow, _target.localToWorldMatrix);
    }
}
