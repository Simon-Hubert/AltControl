using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIRace : RaceManager
{
    public override void Init()
    {
        for (int i = 0; i < _raceConfig.Racers; i++)
        {
            //Instantier les IA
        }
        base.Init();
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
