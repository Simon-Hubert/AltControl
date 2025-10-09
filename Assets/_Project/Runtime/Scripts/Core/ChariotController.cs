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
    private float maxTurnAngle;
    private float rushForce;
    /*private float rushDuration;
    private AnimationCurve rushForceCurve;*/

    private float currentAcc;
    private float currentBrake;
    private float currentTurnAngle;
    
    private float _rightAxisInput;
    private float _leftAxisInput;
    private bool _rightRushInput;
    private bool _leftRushInput;
    
    private bool RushInput => _rightRushInput && _leftRushInput;
    
    private float BrakeInput => (_rightAxisInput + _leftAxisInput) / 2f;
    private float RotationInput => _rightAxisInput - _leftAxisInput;

    private Coroutine RushRoutine;

    [SerializeField] private Rigidbody _rb;

    private void Awake() {
        ChariotConfig config = Resources.Load<ChariotConfig>("ChariotConfig");
        a = config.Acceleration;
        brakeForce = config.BrakeForce;
        maxTurnAngle = config.SteerAngle;
        rushForce = config.RushForce;
        //rushDuration = config.RushDuration;
        //rushForceCurve = config.RushForceCurve;
        
        _frontLeft.forwardFriction = config.ForwardFriction;
        _frontRight.forwardFriction = config.ForwardFriction;
        _backLeft.forwardFriction = config.ForwardFriction;
        _backRight.forwardFriction = config.ForwardFriction;
        
        _frontLeft.sidewaysFriction = config.SidewaysFriction;
        _frontRight.sidewaysFriction = config.SidewaysFriction;
        _backLeft.sidewaysFriction = config.SidewaysFriction;
        _backRight.sidewaysFriction = config.SidewaysFriction;
        
        currentAcc = a;
        
    }

    private void FixedUpdate() {
        currentBrake = BrakeInput * brakeForce;

        if (RushInput) {
            _rb.AddForce(transform.forward * rushForce, ForceMode.Impulse);
            _leftRushInput = false;
            _rightRushInput = false;
        }
        
        _frontRight.motorTorque = currentAcc;
        _frontLeft.motorTorque = currentAcc;
        
        _frontRight.brakeTorque = currentBrake;
        _frontLeft.brakeTorque = currentBrake;
        _backRight.brakeTorque = currentBrake;
        _backLeft.brakeTorque = currentBrake;

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

    /*private IEnumerator Rush() {
        currentAcc = a + rushForce;
        float t = 0;
        
        while (t < rushDuration) {
            t += Time.fixedDeltaTime;
            float p = t / rushDuration;
            p = rushForceCurve.Evaluate(p);
            currentAcc = Mathf.Lerp(a + rushForce, a, p);
            yield return new WaitForFixedUpdate();
        }

        currentAcc = a;
    }*/
    
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
}
