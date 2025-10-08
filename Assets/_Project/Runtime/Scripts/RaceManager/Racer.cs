using System.Collections.Generic;
using UnityEngine;

public class Racer : MonoBehaviour
{
    [SerializeField] string _racerName;
    
    bool _hasFinisded = false;
    bool _isAlive = true;
    int _currentLap; 
    int _lastCheckpointIndex; 
    float _totalTime;
    private float _distToFinishLine;

    public bool HasFinisded => _hasFinisded;
    public bool IsAlive => _isAlive;
    public int CurrentLap => _currentLap;
    public int LastCheckpointIndex => _lastCheckpointIndex;
    public float TotalTime => _totalTime;
    public float DistToFinishLine => _distToFinishLine;

    public void UpdateProgress(List<CheckPoint> checkPoints)
    {
        
    }
}
