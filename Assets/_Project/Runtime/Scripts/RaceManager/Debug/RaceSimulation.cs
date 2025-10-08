using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaceSimulation : MonoBehaviour
{
[Header("Simulation Settings")]
    [SerializeField] private int racerCount = 5;
    [SerializeField] private int checkpointCount = 6;
    [SerializeField] private int lapsToWin = 3;
    [SerializeField] private float trackRadius = 50f;
    [SerializeField] private float racerSpeedMin = 15f;
    [SerializeField] private float racerSpeedMax = 25f;
    [SerializeField] private float checkpointRadius = 3f;

    private List<CheckPoint> _checkpoints = new();
    private List<Racer> _racers = new();
    private bool _raceFinished = false;

    void Start()
    {
        GenerateTrack();
        SpawnRacers();

        Debug.Log($"🏁 Simulation started with {_racers.Count} racers on {_checkpoints.Count} checkpoints!");
    }

    void Update()
    {
        if (_raceFinished) return;

        // Update chaque racer
        foreach (var r in _racers)
        {
            r.SimulateMovement(Time.deltaTime);
            r.UpdateProgress(_checkpoints);

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
            var ordered = _racers.OrderByDescending(r => r.GetRaceProgress()).ToList();
            string ranking = string.Join(" > ", ordered.Select(r => r.name));
            Debug.Log($"Classement: {ranking}");
        }
    }

    // --- Génération des checkpoints en cercle ---
    private void GenerateTrack()
    {
        for (int i = 0; i < checkpointCount; i++)
        {
            float angle = i * Mathf.PI * 2f / checkpointCount;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * trackRadius;
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position = pos;
            go.transform.localScale = Vector3.one * checkpointRadius;
            go.name = $"Checkpoint_{i}";

            CheckPoint cp = go.AddComponent<CheckPoint>();
            cp.SetData(i, i == 0); // 0 = ligne d’arrivée
            _checkpoints.Add(cp);
        }
    }

    // --- Création des racers ---
    private void SpawnRacers()
    {
        for (int i = 0; i < racerCount; i++)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            go.name = $"Racer_{i + 1}";
            go.transform.position = _checkpoints[0].transform.position + new Vector3(i * 2, 0, 0);

            Racer racer = go.AddComponent<Racer>();
            float randomSpeed = Random.Range(racerSpeedMin, racerSpeedMax);
            racer.InitSimulation(_checkpoints, lapsToWin, randomSpeed);

            _racers.Add(racer);
        }
    }

    // --- Résultat final ---
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
