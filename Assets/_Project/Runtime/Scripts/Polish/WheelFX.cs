using System;
using UnityEngine;

public class WheelFX : MonoBehaviour
{
    [SerializeField] private OnGround onGround;
    [SerializeField] private SpeedProxy speed;
    [SerializeField] float thresholdSpeed;
    [SerializeField] private GameObject VFXs;
    
    private void Update() {
        VFXs.SetActive(onGround.IsOnGround && speed.ForwardSpeed > thresholdSpeed); 
    }
}
