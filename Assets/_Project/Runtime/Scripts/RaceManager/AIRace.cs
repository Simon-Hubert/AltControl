using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIRace : RaceManager
{
    public override void Init(List<CheckPoint> checkpoints = null)
    {
        base.Init(checkpoints);
        for (int i = 0; i < _raceConfig.Racers - 1; i++)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            go.name = $"Racer_{i + 1}";
            go.transform.position = _checkPoints[0].transform.position + new Vector3(i * 2, 0, 0);

            Racer racer = go.AddComponent<Racer>();
            racer.name = $"Racer_{i + 1}";
            //float randomSpeed = Random.Range(racerSpeedMin, racerSpeedMax);
            racer.InitSimulation(_checkPoints, _raceConfig.Laps, 15);

            _racers.Add(racer);
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
}
