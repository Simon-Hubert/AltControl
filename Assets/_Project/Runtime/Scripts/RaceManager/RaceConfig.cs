using UnityEngine;

[CreateAssetMenu(fileName = "RaceConfig", menuName = "Scriptable Objects/RaceConfig")]
public class RaceConfig : ScriptableObject
{ 
    public int Laps = 3; 
    public bool AllowAI = true; 
    public bool AllowPlayers = true; 
    public bool EnableDestruction = false; 
    public float TimeLimit = 0f;
    public int Racers = 1;
}
