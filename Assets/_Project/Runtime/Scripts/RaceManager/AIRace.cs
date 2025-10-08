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
            if (racer.HasFinisded)
            {
                StopRace();
            }
        }
    }
    
    public override void CheckWinCondition()
    {
        List<Racer> racersOrdered = _racers.OrderByDescending(racer => racer.CurrentLap).ThenBy(racer => racer.DistToFinishLine).ToList();
        Racer _winner = racersOrdered.First();
        Debug.Log($"Winner is {_winner}");
    }
}
