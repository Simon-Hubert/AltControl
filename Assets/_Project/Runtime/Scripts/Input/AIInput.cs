using System;
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

    [Header("AI Settings")]
    [SerializeField] private float _speed = 10f;
    [SerializeField, Range(0.1f, 5f)] private float _steerSensitivity = 1f; 
    [SerializeField, Range(0.1f, 45f)] private float _maxSteerAngle = 30f;
    [SerializeField] private float _minAxisPower = 0.5f;    
    [SerializeField] private float _steerStrength = 0.4f; 
    [SerializeField] private float _errorInterval = 2f; 
    [SerializeField] private float _maxErrorAngle = 5f; 
    [SerializeField] private float _lookAhead = 2f;     

    [SerializeField] private float _prog = 0f;
    [SerializeField] private Racer _racer;

    //private float _splineLength;
    private float _progress;
    private float _errorTimer;
    private float _errorOffset;
    private bool _initialized = false;
    private float _tDelta, _tAI, _offset = 0.005f;

    private Vector3 _target, /*_debugPos*/ _current, _targetDebug;
    //private float3 _worldTargetTan, _worldTargetPoint, _worldTargetUp, _worldNearestPoint;

    public void Init(/*SplineContainer track*/)
    {
        _controllable = GetComponentInParent<IControllable>();
        //_track = track;
        //_splineLength = _track.CalculateLength();
        _progress = 0f;
        _errorTimer = 0f;
        _initialized = true;
    }

    private void Update()
    {
        if (!_initialized /*|| _track == null*/)
            return;
        
        //Vector3 splinePos = _track.transform.InverseTransformPoint(transform.position);
        //SplineUtility.GetNearestPoint(_track.Spline, splinePos, out float3 nearest, out float t);
       //if(t >= _tAI && t < _tAI + 0.010)
       //{
       //    _tAI = t;
       //    _worldNearestPoint = _track.EvaluatePosition(t);
       //}
       //else if(Mathf.Abs(t - _tAI) > 0.75f)
       //{
       //    _tAI = t;
       //    _worldNearestPoint = _track.EvaluatePosition(t);
       //}
       //else
       //{
       //    _worldNearestPoint = _track.EvaluatePosition(_tAI);
       //}
       //
       //if (t > _tDelta && t < t + _offset)
       //{
       //    _tDelta = t + _offset;
       //    _track.Evaluate(_tDelta, out _worldTargetPoint, out _worldTargetTan, out _worldTargetUp);
       //}
       //else if (Mathf.Abs(t - _tDelta) > 0.75f)
       //{
       //    _tDelta = t + _offset;
       //    _track.Evaluate(_tDelta, out _worldTargetPoint, out _worldTargetTan, out _worldTargetUp);
       //}
       //else
       //{
       //    _tDelta = _tAI + _offset;
       //    _track.Evaluate(_tDelta, out _worldTargetPoint, out _worldTargetTan, out _worldTargetUp);
       //}

       _current = _racer.CurrentCheckpoint.transform.position;
       _target = _racer.NextCheckpoint.transform.position;
       
       //Debug.Log($"current : {_racer.CurrentCheckpoint.Index} next : {_racer.NextCheckpoint.Index}");
       Vector3 pos = transform.position;
       
       float segmentLength = Vector3.Distance(_current, _target);
       float distFromLastChecekPoint = Vector3.Dot((pos - _current), (_target - _current).normalized);
       
       float t = Mathf.Clamp01(distFromLastChecekPoint / segmentLength);
       
       float lookT = Mathf.Clamp01(t + _lookAhead / segmentLength);
       Vector3 lookPoint = Vector3.Lerp(_current, _target, lookT);
       
        //Vector3 normale = Vector3.Cross(_worldTargetTan, _worldTargetUp);

        _errorTimer += Time.deltaTime;
        if (_errorTimer >= _errorInterval)
        {
            //_offset = UnityEngine.Random.Range(_lookAheadOnSpline.x, _lookAheadOnSpline.y);
            _errorOffset = UnityEngine.Random.Range(-_maxErrorAngle, _maxErrorAngle);
            _errorTimer = 0f;
        }

        //_debugPos = new Vector3(_worldNearestPoint.x, _worldNearestPoint.y, _worldNearestPoint.z);
        //_target = new Vector3(_worldTargetPoint.x, _worldTargetPoint.y, _worldTargetPoint.z);
        
        //_target += normale * _errorOffset;
        
        //Vector3 targetDir = ((_target - _debugPos)).normalized;
        Vector3 targetDir = (lookPoint - pos).normalized;
        targetDir = Quaternion.Euler(0, _errorOffset, 0) * targetDir;
        _targetDebug = pos + targetDir * 10;
        
       //targetDir = Quaternion.Euler(0, _errorOffset, 0) * targetDir;
        
        Vector3 forward = transform.forward;
        float angleDelta = Vector3.SignedAngle(forward, targetDir, Vector3.up);
        
        float steerFactor = Mathf.Clamp(angleDelta / _maxSteerAngle, -1f, 1f);
        steerFactor *= _steerSensitivity;
        
        float left = (_minAxisPower - steerFactor * _steerStrength);
        float right = (_minAxisPower + steerFactor * _steerStrength);
        
        _controllable.OnLeftAxis(left);
        _controllable.OnRightAxis(right);
        
        /*if (Mathf.Abs(angleDelta) > _correctionAngle)
        {
            float steerFactor = Mathf.Clamp(angleDelta / 45f, -1f, 1f);
            
            float left = (_minAxisPower - steerFactor * _steerStrength);
            float right = (_minAxisPower + steerFactor * _steerStrength);
            
            _controllable.OnLeftAxis(Mathf.Clamp01(left));
            _controllable.OnRightAxis(Mathf.Clamp01(right));
        }
        else
        {
            _controllable.OnLeftAxis(0f);
            _controllable.OnRightAxis(0f);
        }*/
        
    }

    private void OnDrawGizmos()
    {
        if (_racer == null || _racer.Checkpoints.Count <= 0 || _racer.CurrentCheckpoint == null || _racer.NextCheckpoint == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_targetDebug, 0.5f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, _targetDebug);
    }
}
