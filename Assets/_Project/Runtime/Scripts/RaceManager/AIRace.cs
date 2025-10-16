using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;
using Random = System.Random;

public class AIRace : RaceManager
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] GameObject _aiPrefab;
    [SerializeField] private bool isTest = false;
    [SerializeField] private List<IAConfig> _aiConfigs;
    [SerializeField] private Placement _placementUI;
    
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

        if (_aiConfigs.Count <= 0) {
            foreach (IAConfig aiConfig in Resources.LoadAll<IAConfig>("AIConfigs"))
            {
                _aiConfigs.Add(aiConfig);
            }
        }
        
        Vector3 startPos = _checkPoints[0].transform.position;
        Vector3 nextPos = _checkPoints[1].transform.position;
        Vector3 forwardDir = (nextPos - startPos).normalized;
        List<Transform> spawns = _spawns;
        int i; 
        for (i = 0; i < _raceConfig.Racers - 1; i++) {
            SpawnAI(spawns, forwardDir, i);
        }
        
        SpawnPlayer(spawns, forwardDir, i);
        StartRace();
    }

    private void SpawnAI(List<Transform> spawns, Vector3 forwardDir, int index) {
        Transform spawn = spawns[UnityEngine.Random.Range(0, spawns.Count)];
        GameObject go = Instantiate(_aiPrefab, spawn.position, Quaternion.LookRotation(forwardDir, Vector3.up));
        spawns.Remove(spawn);
            
        go.name = $"Racer_{index + 1}";
        Racer racer = go.GetComponentInChildren<Racer>();
        racer.SetRacerName($"Racer_{index + 1}");
        AIInput input = go.GetComponentInChildren<AIInput>();
        racer.Init(_checkPoints, _raceConfig.Laps);
        input.Init(_aiConfigs[UnityEngine.Random.Range(0, _aiConfigs.Count)]);
        OnRaceStarted += input.StartUp;
        _racers.Add(racer);
    }
    
    private void SpawnPlayer(List<Transform> spawns, Vector3 forwardDir, int index) {
        Transform spawn = spawns[^1];
        GameObject go = Instantiate(_playerPrefab, spawn.position, Quaternion.LookRotation(forwardDir, Vector3.up));
        spawns.Remove(spawn);
        
        go.name = $"Racer_{index + 1}";
        Racer racer = go.GetComponentInChildren<Racer>();
        racer.SetRacerName($"Racer_{index + 1}");
        racer.Init(_checkPoints, _raceConfig.Laps);
        PlayerInput input = go.GetComponent<PlayerInput>();
        OnRaceStarted += input.StartUp;
        _racers.Add(racer);
        _placementUI.SetRacer(racer);
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
            for (int i = 0; i < ordered.Count; i++) {
                ordered[i].Placement = i;
            }
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
