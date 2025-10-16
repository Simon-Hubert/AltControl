using System;
using System.Collections;
using NaughtyAttributes;
using Unity.Cinemachine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;
using Random = System.Random;

public class AIInput : Input
{
    private IControllable _controllable;
    private SplineContainer _track;
    [SerializeField] private IAConfig _config;

    [SerializeField] private float _prog = 0f;
    [SerializeField] private Racer _racer;
    [SerializeField] private Rigidbody _rb;
    
    private float _progress;
    private float _errorTimer;
    private float _errorOffset;
    private bool _initialized = false;
    private float _tDelta, _tAI, _offset = 0.005f;

    private Vector3 _target, _current, _targetDebug;

    private Coroutine _accelCoolDownRoutine;

    public void Init(IAConfig config)
    {
        _controllable = GetComponentInParent<IControllable>();
        _progress = 0f;
        _errorTimer = 0f;
        _config = config;
        _controllable.Enable(false);
    }

    public void StartUp()
    {
        transform.position += new Vector3(0, .25f, 0);
        _controllable.Enable(true);
        _initialized = true;
    }

    private void Update()
    {
        if (!_initialized)
            return;

       _current = _racer.CurrentCheckpoint.transform.position;
       _target = _racer.NextCheckpoint.transform.position;
       
       Vector3 pos = transform.position;
       
       float segmentLength = Vector3.Distance(_current, _target);
       float distFromLastChecekPoint = Vector3.Dot((pos - _current), (_target - _current).normalized);
       
       float t = Mathf.Clamp01(distFromLastChecekPoint / segmentLength);
       
       float lookT = Mathf.Clamp01(t + _config.LookAhead / segmentLength);
       Vector3 lookPoint = Vector3.Lerp(_current, _target, lookT);

        _errorTimer += Time.deltaTime;
        if (_errorTimer >= _config.ErrorInterval)
        {
            _errorOffset = UnityEngine.Random.Range(-_config.MaxErrorAngle, _config.MaxErrorAngle);
            _errorTimer = 0f;
        }
        
        Vector3 targetDir = (lookPoint - pos).normalized;
        targetDir = Quaternion.Euler(0, _errorOffset, 0) * targetDir;
        _targetDebug = lookPoint;
        
        Vector3 forward = transform.forward;
        float angleDelta = Vector3.SignedAngle(forward, targetDir, Vector3.up);
        
        float steerFactor = Mathf.Clamp(angleDelta / _config.MaxSteerAngle, -1f, 1f);
        steerFactor *= _config.SteerSensitivity;
        
        float left = (_config.MinAxisPower - steerFactor * _config.SteerStrength);
        float right = (_config.MinAxisPower + steerFactor * _config.SteerStrength);
        float triggerBoost = Vector3.Dot(_racer.CurrentCheckpoint.transform.forward, _racer.NextCheckpoint.transform.forward);
        
        _rb.AddForce(transform.forward * _config.Boost, ForceMode.Acceleration);
        
        if (_config.TriggerBoostLimit < triggerBoost && _accelCoolDownRoutine == null)
        {
            _controllable.OnLeftButton();
            _controllable.OnRightButton();
            _accelCoolDownRoutine = StartCoroutine(CoolDownAcceleration());
        }
        _controllable.OnLeftAxis(left);
        _controllable.OnRightAxis(right);
    }

    private void OnDrawGizmos()
    {/*
        if (_racer == null || _racer.Checkpoints.Count <= 0 || _racer.CurrentCheckpoint == null || _racer.NextCheckpoint == null ||!Application.isPlaying)
            return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_targetDebug, 0.5f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, _targetDebug);*/
    }

    IEnumerator CoolDownAcceleration()
    {
        yield return new WaitForSeconds(_config.CoolDownAccel);
        _accelCoolDownRoutine = null;
    }
}
