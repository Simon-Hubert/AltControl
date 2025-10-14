using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;
using Random = System.Random;

[RequireComponent(typeof(SplineContainer))]
public class AIRace : RaceManager
{
    [SerializeField] GameObject _aiPrefab;
    [SerializeField] private bool isTest = false;

    private void Start()
    {
        if (!isTest)
        {
            Init();
        }
    }

    public override void Init(List<CheckPoint> checkpoints = null)
    {
        base.Init(checkpoints);
        
        Vector3 startPos = _checkPoints[0].transform.position;
        Vector3 nextPos = _checkPoints[1].transform.position;
        Vector3 forwardDir = (nextPos - startPos).normalized;
        List<Transform> spawns = _spawns;
        for (int i = 0; i < _raceConfig.Racers - 1; i++)
        {
            Transform spawn = spawns[UnityEngine.Random.Range(0, spawns.Count)];
            GameObject go = Instantiate(_aiPrefab, spawn.position, Quaternion.LookRotation(forwardDir, Vector3.up));
            spawns.Remove(spawn);
            
            go.name = $"Racer_{i + 1}";
            Racer racer = go.GetComponentInChildren<Racer>();
            racer.SetRacerName($"Racer_{i + 1}");
            //float randomSpeed = Random.Range(racerSpeedMin, racerSpeedMax);
            AIInput input = go.GetComponentInChildren<AIInput>();
            racer.Init(_checkPoints, _raceConfig.Laps);
            input.Init(/*_raceSpline*/);
            //racer.InitSimulation(_checkPoints, _raceConfig.Laps, 15);

            _racers.Add(racer);
        }

    }
    void Update()
    {
        if (_raceFinished || !RaceStarted) return;

        // Update chaque racer
        foreach (var r in _racers)
        {
            //r.SimulateMovement(Time.deltaTime);
            r.UpdateProgress(_checkPoints);

            // Fin de course ?
            if (_racers.All(x => x.HasFinished))
            {
                _raceFinished = true;
                DisplayFinalRanking();
                break;
            }
        }

        // Affichage debug du classement temps réel
        if (Time.frameCount % 30 == 0) // toutes les 0.5s environ
        {
            UpdateRace();
        }
    }

    public override void UpdateRace()
    {
        base.UpdateRace();

        foreach (Racer racer in _racers)
        {
            racer.UpdateProgress(_checkPoints);
            if (racer.HasFinished)
            {
                StopRace();
            }
        }

        List<Racer> ordered = GetRankedRacers();
        if(ordered.Count > 0)
        {
            Debug.Log($"Classement actuel: {string.Join(", ", ordered.Select(r => r.RacerName))}");
        }

    }
    
    public override void CheckWinCondition()
    {
        List<Racer> ordered = GetRankedRacers();
        Racer winner = ordered.First();
        Debug.Log($"Winner is {winner.RacerName}");
    }
    
    private void DisplayFinalRanking()
    {
        var ordered = _racers.OrderByDescending(r => r.GetRaceProgress()).ThenBy(r => r.TotalTime).ToList();
        Debug.Log("🏆 COURSE TERMINÉE !");
        for (int i = 0; i < ordered.Count; i++)
        {
            Debug.Log($"{i + 1}. {ordered[i].name} — {ordered[i].TotalTime:F2}s");
        }
    }
}
