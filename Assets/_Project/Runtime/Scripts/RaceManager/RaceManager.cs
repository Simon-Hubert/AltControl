using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;
using UnityEngine.UI;

public abstract class RaceManager : MonoBehaviour
{
    [SerializeField] protected RaceConfig _raceConfig;
    [SerializeField] protected List<CheckPoint> _checkPoints;
    [SerializeField] protected List<Transform> _spawns;
    [SerializeField] protected int _countDownRace = 3;
    
    //[SerializeField] protected SplineContainer _raceSpline;

    public UnityEvent UnityOnRaceStarted;
    public UnityEvent UnityOnRaceStopped;
    public UnityEvent UnityOnCountdownRace;
    public UnityEvent UnityOnGo;
    
    public event Action OnRaceStarted;
    public event Action<int> OnCountDownRace;


    protected bool _raceStarted, _raceFinished;
    protected List<Racer> _racers;
    
    public RaceConfig RaceConfig => _raceConfig;
    public bool RaceStarted => _raceStarted;
    public List<Racer> Racers => _racers;
    //public SplineContainer RaceSpline => _raceSpline;
    
    public static RaceManager Instance;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);
    }

    public virtual void Init(List<CheckPoint> checkpoints = null)
    {
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
        
        _racers = FindObjectsOfType<Racer>().ToList();
        
        //foreach (var r in _racers)
        //    r.Init(_checkPoints, _raceConfig.Laps);
        _raceStarted = true;
        _raceFinished = false;
    }

    public virtual void StartRace()
    {
        StartCoroutine(CountdownRace());
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

    public virtual void LastLap()
    {
        GetComponent<AudioSource>().pitch *= 1.25f;
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

    IEnumerator CountdownRace()
    {

        for (int i = 0; i < _countDownRace; i++)
        {
            OnCountDownRace?.Invoke(i);
            if (i == _countDownRace - 1)
            {
                UnityOnGo?.Invoke();
            }
            else
            {
                UnityOnCountdownRace?.Invoke();
            }
            yield return new WaitForSeconds(1);
        }
        
        _raceStarted = true;
        _raceFinished = false;
        
        OnRaceStarted?.Invoke();
        UnityOnRaceStarted?.Invoke();
    }
}
