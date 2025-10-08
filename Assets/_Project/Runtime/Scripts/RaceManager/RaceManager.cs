using System.Collections.Generic;
using System.Linq;
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
    
    public RaceConfig RaceConfig => _raceConfig;
    public bool RaceStarted => _raceStarted;

    public virtual void Init()
    {
        _checkPoints = FindObjectsOfType<CheckPoint>().OrderBy(c => c.Index).ToList();
        _racers = FindObjectsOfType<Racer>().ToList();
        
        foreach (var r in _racers)
            r.Init(_checkPoints, _raceConfig.Laps);
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

    public List<Racer> GetRankedRacers()
    {
        return _racers.OrderByDescending(r => r.GetRaceProgress()).ToList();
    }
}
