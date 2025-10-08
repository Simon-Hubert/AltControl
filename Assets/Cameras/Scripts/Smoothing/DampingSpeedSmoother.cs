using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "CameraSmoothing/DampingSpeed")]
public class DampingSpeedSmoother : CameraSmoother
{
    [SerializeField] private float _dampingCoefficient = 0.1f;
    
    public override CameraConfiguration Smooth(CameraConfiguration current, CameraConfiguration target) {
        current += _dampingCoefficient * (target - current);
        return current;
    }
}
