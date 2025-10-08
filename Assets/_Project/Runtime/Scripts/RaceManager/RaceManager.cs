using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class RaceManager : MonoBehaviour
{
    [SerializeField] protected RaceConfig _raceConfig;
    [SerializeField] protected List<CheckPoint> _checkPoints;
    [SerializeField] protected List<Racer> _racers;

    public UnityEvent UnityOnRaceStarted;
    public UnityEvent UnityOnRaceStopped;

    protected bool _raceStarted, _raceFinished;

    public virtual void Init()
    {
        _raceStarted = true;
        _raceFinished = false;
    }

    public virtual void StartRace()
    {
        _raceStarted = true;
        _raceFinished = false;
        
        UnityOnRaceStarted?.Invoke();
    }

    public virtual void UpdateRace()
    {
        if (!_raceStarted || _raceFinished)
            return;
    }
    
    public virtual void StopRace()
    {
        _raceFinished = true;
        
        UnityOnRaceStopped?.Invoke();
    }
    
    public abstract void CheckWinCondition();
}
