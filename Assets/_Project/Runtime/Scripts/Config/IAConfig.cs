using UnityEngine;

[CreateAssetMenu(fileName = "AIConfig", menuName = "Scriptable Objects/AIConfig")]
public class IAConfig : ScriptableObject
{
    public float Boost = 10f;
    [Range(0.1f, 5f)] public float SteerSensitivity = 1f; 
    [Range(0.1f, 45f)] public float MaxSteerAngle = 30f;
    public float MinAxisPower = 0.5f;    
    public float SteerStrength = 0.4f; 
    public float ErrorInterval = 2f; 
    public float MaxErrorAngle = 5f; 
    public float LookAhead = 2f;
    public float CoolDownAccel = 1.5f;
    public float TriggerBoostLimit = 0.9f; 
}
