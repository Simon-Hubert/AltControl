using UnityEngine;
using UnityEngine.Splines;

public class AIInput : MonoBehaviour
{
    private IControllable _controllable;
    private SplineContainer _track;

    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _errorInterval = 2f;
    [SerializeField] private float _maxErrorDist = 1f;

    private float _splineLenght;
    private float _progress;
    private float _currentErrorDist;
    private float _targetErrorDist;
    private float _errorTimer;
    private bool _initialized = false;

    void Init(SplineContainer track)
    {
        _controllable = GetComponent<IControllable>();
        _track = track;
        _splineLenght = _track.CalculateLength();
        _progress = 0f;
        _errorTimer = 0f;
        _initialized = true;
    }

    void GetDirection()
    {
        _progress += (Time.deltaTime * _speed) / _splineLenght;
        //_track.Evaluate(_progress, out float3 pos, out float);
    }
    
}
