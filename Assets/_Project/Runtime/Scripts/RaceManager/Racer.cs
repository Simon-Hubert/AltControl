using System.Collections.Generic;
using UnityEngine;

public class Racer : MonoBehaviour
{
    [SerializeField] string _racerName;
    
    bool _hasFinished = false;
    bool _isAlive = true;
    int _currentLap; 
    int _lastCheckpointIndex;
    private int _lapsToWin;
    float _totalTime;
    private float _distToNextCheckpoint = float.MaxValue;
    private float _speed; //debug
    private Vector3 _targetPos; //debug
    
    private CheckPoint _nextCheckpoint;
    private List<CheckPoint> _checkpoints;

    public bool HasFinished => _hasFinished;
    public bool IsAlive => _isAlive;
    public int CurrentLap => _currentLap;
    public int LastCheckpointIndex => _lastCheckpointIndex;
    public int LapsToWin => _lapsToWin;
    public float TotalTime => _totalTime;
    public float DistToNextCheckpoint => _distToNextCheckpoint;
    public string RacerName => _racerName;

    public void Init(List<CheckPoint> checkPoints, int lapsToWin)
    {
        _checkpoints = checkPoints;
        _nextCheckpoint = _checkpoints[0];
        _currentLap = 0;
        _lastCheckpointIndex = -1;
        _hasFinished = false;
        _isAlive = true;
        _totalTime = 0;
        _lapsToWin = lapsToWin;
    }
    
    public void SetRacerName(string racerName) => _racerName = racerName;

    public void UpdateProgress(List<CheckPoint> checkPoints)
    {
        if (_hasFinished) return;

        _totalTime += Time.deltaTime;
        if (_nextCheckpoint != null)
        {
            _distToNextCheckpoint = Vector3.Distance(transform.position, _nextCheckpoint.transform.position);
        }
    }

    public void OnCheckPointPassed(CheckPoint checkPoint)
    {
        if(_hasFinished) return;
        
        int expectedIndex = (_lastCheckpointIndex + 1) % _checkpoints.Count;
        if (checkPoint.Index != expectedIndex)
            return;
        
        _lastCheckpointIndex = checkPoint.Index;
        
        _lastCheckpointIndex = checkPoint.Index;

        if (checkPoint.IsFinishLine)
        {
            _currentLap++;

            if (_currentLap >= _lapsToWin)
            {
                _hasFinished = true;
                Debug.Log($"{_racerName} finished the race in {_totalTime:F2}s !");
                return;
            }
        }

        int nextIndex = (_lastCheckpointIndex + 1) % _checkpoints.Count;
        _nextCheckpoint = _checkpoints[nextIndex];
    }

    public float GetRaceProgress()
    {
        if(_checkpoints == null || _checkpoints.Count == 0) return 0;
        
        float checkPointProgress = ((float)(_lastCheckpointIndex + 1)) / _checkpoints.Count;
        float segmentLength = 0f;
        if (_nextCheckpoint != null)
        {
            int prevIndex = _lastCheckpointIndex >= 0 ? _lastCheckpointIndex : _checkpoints.Count - 1;
            Vector3 prevPos = _checkpoints[prevIndex].transform.position;
            segmentLength = Vector3.Distance(prevPos, _nextCheckpoint.transform.position);
        }
        float distanceRatio = segmentLength > 0f ? 1f - Mathf.Clamp01(_distToNextCheckpoint / segmentLength) : 0f;
        float progress = _currentLap + checkPointProgress + distanceRatio * (1f / _checkpoints.Count);
        return progress;
    }
    
    public void InitSimulation(List<CheckPoint> checkPoints, int lapsToWin, float speed)
    {
        Init(checkPoints, lapsToWin);
        _speed = speed;
        _targetPos = _checkpoints[0].transform.position;
    }

    public void SimulateMovement(float deltaTime)
    {
        if (_hasFinished) return;

        // Avancer vers le prochain checkpoint
        if (_nextCheckpoint != null)
        {
            _targetPos = _nextCheckpoint.transform.position;
            Vector3 dir = (_targetPos - transform.position).normalized;
            transform.position += dir * _speed * deltaTime;

            // Collision manuelle avec checkpoint (sans collider)
            float dist = Vector3.Distance(transform.position, _targetPos);
            if (dist < 2f)
            {
                OnCheckPointPassed(_nextCheckpoint);
            }
        }
    }
}
