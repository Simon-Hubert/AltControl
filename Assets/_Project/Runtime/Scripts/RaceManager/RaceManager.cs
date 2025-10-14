using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

public abstract class RaceManager : MonoBehaviour
{
    [SerializeField] protected RaceConfig _raceConfig;
    [SerializeField] protected List<CheckPoint> _checkPoints;
    [SerializeField] protected List<Transform> _spawns;
    
    //[SerializeField] protected SplineContainer _raceSpline;

    public UnityEvent UnityOnRaceStarted;
    public UnityEvent UnityOnRaceStopped;

    protected bool _raceStarted, _raceFinished;
    protected List<Racer> _racers;
    
    public RaceConfig RaceConfig => _raceConfig;
    public bool RaceStarted => _raceStarted;
    public List<Racer> Racers => _racers;
    //public SplineContainer RaceSpline => _raceSpline;
    
    public static RaceManager instance;

    void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(this);
    }

    public virtual void Init(List<CheckPoint> checkpoints = null)
    {
        _racers = new List<Racer>();
        if (checkpoints != null)
        {
            _checkPoints = checkpoints;
            _checkPoints.OrderBy(c => c.Index).ToList();
        }
        else
        {
            _checkPoints = FindObjectsOfType<CheckPoint>().OrderBy(c => c.Index).ToList();

        }
        
        //GenerateSplineFromCheckpoints();
        
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

    /*void GenerateSplineFromCheckpoints()
    {
        if(_checkPoints == null || _checkPoints.Count < 2) return;

        if (_raceSpline == null)
        {
            GameObject splineObj = new GameObject("RaceSpline");
            splineObj.transform.SetParent(transform);
            _raceSpline = splineObj.AddComponent<SplineContainer>();
        }
        
        _raceSpline.Spline.Clear();

        foreach (CheckPoint pt in _checkPoints)
        {
            Vector3 pos = pt.transform.position;
            Vector3 forward = pt.transform.forward;
            _raceSpline.Spline.Add(new BezierKnot(pos, forward * 0.5f, -forward * 0.5f));
        }

        _raceSpline.Spline.Closed = true;
    }*/
}
