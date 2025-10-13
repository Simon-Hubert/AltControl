using System;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;
using Random = System.Random;

public class AIInput : MonoBehaviour
{
    private IControllable _controllable;
    private SplineContainer _track;

    [Header("AI Settings")]
    [SerializeField] private float _speed = 10f;
    
    [SerializeField, Range(0.1f, 5f)] private float _steerSensitivity = 1f;
    [SerializeField, MinMaxSlider(0.001f, 0.02f)] private Vector2 _lookAheadOnSpline;  
    [SerializeField] private float _correctionAngle = 1f;      
    [SerializeField] private float _minAxisPower = 0.5f;        
    [SerializeField] private float _steerStrength = 0.4f;       
    [SerializeField] private float _errorInterval = 2f;      
    [SerializeField] private float _maxErrorAngle = 5f;
    [SerializeField] private float _MAXRange = 0.005f;

    [SerializeField] private float _prog = 0f;

    private float _splineLength;
    private float _progress;
    private float _errorTimer;
    private float _errorOffset;
    private bool _initialized = false;
    private float _tDelta, _tAI, _offset = 0.005f;

    private Vector3 _target, _debugPos;
    private float3 _worldTargetTan, _worldTargetPoint, _worldTargetUp, _worldNearestPoint;

    public void Init(SplineContainer track)
    {
        _controllable = GetComponentInParent<IControllable>();
        _track = track;
        _splineLength = _track.CalculateLength();
        _progress = 0f;
        _errorTimer = 0f;
        _initialized = true;
    }

    private void Update()
    {
        if (!_initialized || _track == null)
            return;
        
        Vector3 splinePos = _track.transform.InverseTransformPoint(transform.position);
        SplineUtility.GetNearestPoint(_track.Spline, splinePos, out float3 nearest, out float t);
        if(t >= _tAI && t < _tAI + 0.010)
        {
            _tAI = t;
            _worldNearestPoint = _track.EvaluatePosition(t);
        }
        else if(Mathf.Abs(t - _tAI) > 0.75f)
        {
            _tAI = t;
            _worldNearestPoint = _track.EvaluatePosition(t);
        }
        else
        {
            _worldNearestPoint = _track.EvaluatePosition(_tAI);
        }
        
        if (t > _tDelta && t < t + _offset)
        {
            _tDelta = t + _offset;
            _track.Evaluate(_tDelta, out _worldTargetPoint, out _worldTargetTan, out _worldTargetUp);
        }
        else if (Mathf.Abs(t - _tDelta) > 0.75f)
        {
            _tDelta = t + _offset;
            _track.Evaluate(_tDelta, out _worldTargetPoint, out _worldTargetTan, out _worldTargetUp);
        }
        else
        {
            _tDelta = _tAI + _offset;
            _track.Evaluate(_tDelta, out _worldTargetPoint, out _worldTargetTan, out _worldTargetUp);
        }
        
       
        Vector3 normale = Vector3.Cross(_worldTargetTan, _worldTargetUp);
        
        _errorTimer += Time.deltaTime;
        if (_errorTimer >= _errorInterval)
        {
            _offset = UnityEngine.Random.Range(_lookAheadOnSpline.x, _lookAheadOnSpline.y);
            _errorOffset = UnityEngine.Random.Range(-_maxErrorAngle, _maxErrorAngle);
            _errorTimer = 0f;
        }
        
        _debugPos = new Vector3(_worldNearestPoint.x, _worldNearestPoint.y, _worldNearestPoint.z);
        _target = new Vector3(_worldTargetPoint.x, _worldTargetPoint.y, _worldTargetPoint.z);
        
        _target += normale * _errorOffset;
        
        Vector3 targetDir = ((_target - _debugPos)).normalized;
        
       targetDir = Quaternion.Euler(0, _errorOffset, 0) * targetDir;
        
        Vector3 forward = transform.forward;
        float angleDelta = Vector3.SignedAngle(forward, targetDir, Vector3.up);
        
        if (Mathf.Abs(angleDelta) > _correctionAngle)
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
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(new Vector3(_debugPos.x, _debugPos.y, _debugPos.z), 1);
        Gizmos.DrawWireSphere(new Vector3(_target.x, _target.y, _target.z), 1);
    }
}
