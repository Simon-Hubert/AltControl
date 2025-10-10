using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class AIInput : MonoBehaviour
{
    private IControllable _controllable;
    private SplineContainer _track;

    [Header("AI Settings")]
    [SerializeField] private float _speed = 10f;            
    [SerializeField] private float _lookAheadDistance = .5f;      
    [SerializeField] private float _correctionAngle = 10f;      
    [SerializeField] private float _minAxisPower = 0.5f;        
    [SerializeField] private float _steerStrength = 0.4f;       
    [SerializeField] private float _errorInterval = 2f;      
    [SerializeField] private float _maxErrorAngle = 5f;    

    private float _splineLength;
    private float _progress;
    private float _errorTimer;
    private float _errorOffset;
    private bool _initialized = false;

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
        
        _progress += (Time.deltaTime * _speed) / _splineLength;
        if (_progress > 1f)
            _progress -= 1f;
        
        _track.Evaluate(_progress, out float3 pos, out _, out _);
        
        float lookAheadT = Mathf.Clamp01(_progress + (_lookAheadDistance / _splineLength));
        _track.Evaluate(lookAheadT, out float3 aheadPos, out _, out _);
        
        Vector3 targetDir = ((Vector3)(aheadPos - pos)).normalized;
        
        _errorTimer += Time.deltaTime;
        if (_errorTimer >= _errorInterval)
        {
            _errorOffset = UnityEngine.Random.Range(-_maxErrorAngle, _maxErrorAngle);
            _errorTimer = 0f;
        }
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
}
