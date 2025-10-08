using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    [SerializeField] protected RaceConfig _raceConfig;
    [SerializeField] protected List<CheckPoint> _checkPoints;
    [SerializeField] protected List<Racer> _racers;
}
