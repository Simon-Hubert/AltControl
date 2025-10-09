using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ChariotConfig", menuName = "ChariotConfig")]
public class ChariotConfig : ScriptableObject
{
    public float Acceleration;
    public float BrakeForce;
    public float SteerAngle;
    public float RushForce;
    //public float RushDuration;
    //public AnimationCurve RushForceCurve;

    [Header("Forward Friction")] 
    [SerializeField, Label("Extremum Slip")]   private float ExtremumForwardSlip;
    [SerializeField, Label("Extremum Value")]  private float ExtremumForwardValue;
    [SerializeField, Label("Asymptote Slip")]  private float AsymptoteForwardSlip;
    [SerializeField, Label("Asymptote Value")] private float AsymptoteForwardValue;
    [SerializeField, Label("Stiffness")]       private float ForwardStiffness;
    
    [Header("Sideways Friction")]
    [SerializeField, Label("Extremum Slip")]   private float ExtremumSidewaysSlip;
    [SerializeField, Label("Extremum Value")]  private float ExtremumSidewaysValue;
    [SerializeField, Label("Asymptote Slip")]  private float AsymptoteSidewaysSlip;
    [SerializeField, Label("Asymptote Value")] private float AsymptoteSidewaysValue;
    [SerializeField, Label("Stiffness")]       private float SidewaysStiffness;

    public WheelFrictionCurve ForwardFriction => new WheelFrictionCurve
    {
        asymptoteSlip = AsymptoteForwardSlip,
        asymptoteValue = AsymptoteForwardValue,
        extremumSlip = ExtremumForwardSlip,
        extremumValue = ExtremumForwardValue,
        stiffness = ForwardStiffness
    };
    
    public WheelFrictionCurve SidewaysFriction=> new WheelFrictionCurve
    {
        asymptoteSlip = AsymptoteSidewaysSlip,
        asymptoteValue = AsymptoteSidewaysValue,
        extremumSlip = ExtremumSidewaysSlip,
        extremumValue = ExtremumSidewaysValue,
        stiffness = SidewaysStiffness
    };
    
}
