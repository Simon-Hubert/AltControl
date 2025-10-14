using UnityEngine;

[CreateAssetMenu(fileName = "IAConfig", menuName = "Scriptable Objects/IAConfig")]
public class IAConfig : ScriptableObject
{
    [Header("AI Settings")]
    public float Speed = 10f;
    [Range(0.1f, 5f)] public float SteerSensitivity = 1f; 
    [Range(0.1f, 45f)] public float MaxSteerAngle = 30f;
    public float MinAxisPower = 0.5f;    
    public float SteerStrength = 1f; 
    public float ErrorInterval = 2f; 
    public float MaxErrorAngle = 0.01f; 
    public float LookAhead = 2f; 
}
