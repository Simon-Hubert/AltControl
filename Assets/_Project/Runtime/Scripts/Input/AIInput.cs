using System;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

public class AIInput : MonoBehaviour
{
    private IControllable _controllable;
    private SplineContainer _track;

    [Header("AI Settings")]
    [SerializeField] private float _speed = 10f;            
    [SerializeField] private float _lookAheadOnSpline = 0.05f;      
    [SerializeField] private float _correctionAngle = 1f;      
    [SerializeField] private float _minAxisPower = 0.5f;        
    [SerializeField] private float _steerStrength = 0.4f;       
    [SerializeField] private float _errorInterval = 2f;      
    [SerializeField] private float _maxErrorAngle = 5f;    

    private float _splineLength;
    private float _progress;
    private float _errorTimer;
    private float _errorOffset;
    private bool _initialized = false;

    private Vector3 _target, _debugPos;

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
        
        float3 worldnearestpoint = _track.EvaluatePosition(t);
        
        _track.Evaluate(t + _lookAheadOnSpline, out float3 worldtargetPoint, out float3 worldTargetTan, out float3 worldTargetUp);
        Vector3 normale = Vector3.Cross(worldTargetTan, worldTargetUp);
        
        _errorTimer += Time.deltaTime;
        if (_errorTimer >= _errorInterval)
        {
            _errorOffset = UnityEngine.Random.Range(-_maxErrorAngle, _maxErrorAngle);
            _errorTimer = 0f;
        }
        
        _debugPos = new Vector3(worldnearestpoint.x, worldnearestpoint.y, worldnearestpoint.z);
        _target = new Vector3(worldtargetPoint.x, worldtargetPoint.y, worldtargetPoint.z);
        
        _target += normale * _errorOffset;
        
        Vector3 targetDir = ((Vector3)(_target - _debugPos)).normalized;
        
       targetDir = Quaternion.Euler(0, _errorOffset, 0) * targetDir;
        
        Vector3 forward = transform.forward;
        float angleDelta = Vector3.SignedAngle(forward, targetDir, Vector3.up);
        
        if (Mathf.Abs(angleDelta) > _correctionAngle)
        {
            float steerFactor = Mathf.Clamp(angleDelta / 45f, -1f, 1f);
            
            float left = _minAxisPower - steerFactor * _steerStrength;
            float right = _minAxisPower + steerFactor * _steerStrength;
            
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
