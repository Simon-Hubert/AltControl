using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ChariotConfig", menuName = "ChariotConfig")]
public class ChariotConfig : ScriptableObject
{
    [Tooltip("C'est une valeur approchée, la vie de ma mère le premier GD qui viens se plaindre me doit un Kebab")] public float MaxSpeed;
    public float MaxBackSpeed;
    public float BrakeFactor;
    public float SteerAngle;
    public float RushForce;
    [Label("Acceleration")] public float Friction; //Ouais je ment sans vegogne aux GD
    public float DriftFactor;
    public AnimationCurve BrakeCurve;

    public float SideForce;
    public float SideForceTime;
    public AnimationCurve SideForceCurve;

    [Header("Collisions")] 
    public float RespawnForce;
    public float RespawnInvincibilityTime;

}
