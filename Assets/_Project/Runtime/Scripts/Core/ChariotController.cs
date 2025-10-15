using System;
using System.Collections;
using UnityEngine;

public class ChariotController : MonoBehaviour, IControllable
{
    [SerializeField] private WheelCollider _frontLeft;
    [SerializeField] private WheelCollider _frontRight;
    [SerializeField] private WheelCollider _backLeft;
    [SerializeField] private WheelCollider _backRight;

    private float a;
    private float brakeForce;
    private float backAccel;
    private float maxTurnAngle;
    private float rushForce;
    private AnimationCurve brakeCurve;
    
    private float sideForce;
    private float sideForceMaxTime;
    private AnimationCurve sideForceCurve;
    private float sfT;

    private float currentAcc;
    private float currentBrake;
    private float currentTurnAngle;
    private float currentSideForce;
    
    private float _rightAxisInput;
    private float _leftAxisInput;
    private bool _rightRushInput;
    private bool _leftRushInput;
    
    private bool RushInput => _rightRushInput && _leftRushInput;
    
    private float BrakeInput => (_rightAxisInput + _leftAxisInput) / 2f;
    private float RotationInput => _rightAxisInput - _leftAxisInput;

    private ChariotConfig config;

    [SerializeField] private Rigidbody _rb;

    private void Awake() {
        config = Resources.Load<ChariotConfig>("ChariotConfig");
        UpdateConfig();
        currentAcc = a;
    }

    private void FixedUpdate() {
        currentBrake = brakeCurve.Evaluate(BrakeInput) * brakeForce;

        if (RushInput) {
            _rb.AddForce(_rb.transform.forward * rushForce, ForceMode.VelocityChange);
            _leftRushInput = false;
            _rightRushInput = false;
        }
        

        if (Vector3.Dot(_rb.linearVelocity, _rb.transform.forward) < 0 && BrakeInput > 0.7) {
            _rb.AddForce(_rb.transform.forward * backAccel, ForceMode.Acceleration);
        }
        else {
            _rb.AddForce(_rb.transform.forward * (currentAcc - currentBrake), ForceMode.Acceleration);
        }

        if (Mathf.Abs(RotationInput) > 0.3) {
            sfT += Time.fixedDeltaTime;
            float p = Mathf.Min(sfT / sideForceMaxTime, 1);
            p = sideForceCurve.Evaluate(p);
            currentSideForce = (Mathf.Sign(RotationInput) * sideForce * p);
            _rb.AddForce(_rb.transform.right * (Mathf.Sign(RotationInput) * sideForce * p), ForceMode.Acceleration);
        }
        else {
            sfT = 0f;
        }

        currentTurnAngle = maxTurnAngle * RotationInput;
        _frontLeft.steerAngle = currentTurnAngle;
        _frontRight.steerAngle = currentTurnAngle;
    }


    public void OnRightAxis(float value) {
        _rightAxisInput = value;
    }
    
    public void OnLeftAxis(float value) {
        _leftAxisInput = value;
    }
    
    public void OnRightButton() {
        StartCoroutine(ResetRightBoolCoroutine());
    }
    
    public void OnLeftButton() {
        StartCoroutine(ResetLeftBoolCoroutine());
    }

    private void UpdateConfig() {
        brakeCurve = config.BrakeCurve;
        maxTurnAngle = config.SteerAngle;
        rushForce = config.RushForce;
        _rb.linearDamping = config.Friction;

        sideForce = config.SideForce;
        sideForceMaxTime = config.SideForceTime;
        sideForceCurve = config.SideForceCurve;
        
        a = config.MaxSpeed * config.Friction;
        backAccel = -config.MaxBackSpeed * config.Friction;
        brakeForce = config.BrakeFactor * a;
        
        WheelFrictionCurve curve = _backLeft.sidewaysFriction;
        curve.stiffness = config.DriftFactor;
        _backLeft.sidewaysFriction = curve;
        
        WheelFrictionCurve friction = _backRight.sidewaysFriction;
        friction.stiffness = config.DriftFactor;
        _backRight.sidewaysFriction = friction;
    }
    
    private IEnumerator ResetRightBoolCoroutine() {
        _rightRushInput = true;
        yield return new WaitForSeconds(0.2f);
        _rightRushInput = false;
    }
    
    private IEnumerator ResetLeftBoolCoroutine() {
        _leftRushInput = true;
        yield return new WaitForSeconds(0.2f);
        _leftRushInput = false;
    }
    
#if UNITY_EDITOR
    private void OnGUI() {
        GUIStyle style = new GUIStyle(GUIStyle.none) {
            fontSize = 24,
            normal = {
                textColor = Color.green
            },
            
        };
        GUI.Label(new Rect(10, 10, 300, 20), $"Linear speed : {_rb.linearVelocity.magnitude:F1}", style);
        GUI.Label(new Rect(10, 40, 300, 20), $"Time : {Time.time - 0.8f:F}", style);
        GUI.Label(new Rect(10, 70, 300, 20), $"SideForce : {currentSideForce:F}", style);
    }

    private void OnDrawGizmos() {
        if (Application.isPlaying) {
            Gizmos.DrawLine(_rb.transform.position, _rb.transform.position + _rb.linearVelocity * 2.0f);
        }
    }
#endif
}
