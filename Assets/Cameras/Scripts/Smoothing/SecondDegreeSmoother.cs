using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;


[CreateAssetMenu(menuName = "CameraSmoothing/SecondDegreeSmoother")]
public class SecondDegreeSmoother : CameraSmoother
{
    private CameraConfiguration _lastPos;
    private CameraConfiguration _speed;

    private Vector2 _lastYawVector;
    private Vector2 _yawSpeed;
    
    [SerializeField] private float _f, _z, _r; 
    private float _k1, _k2, _k3;

    public override CameraConfiguration Smooth(CameraConfiguration current, CameraConfiguration target) {
        _k1 = _z / (PI * _f);
        _k2 = 1 / ((2 * PI * _f) * (2 * PI * _f));
        _k3 = _r * _z / (2 * PI * _f);
        
        
        CameraConfiguration lastSpeed = (target - _lastPos) / Time.deltaTime;
        _lastPos = target;
        
        Vector2 yawVector =  new Vector2(
            Mathf.Cos(target.Yaw * Deg2Rad),
            Mathf.Sin(target.Yaw * Deg2Rad));
        
        Vector2 currentYawVector =  new Vector2(
            Mathf.Cos(target.Yaw * Deg2Rad),
            Mathf.Sin(target.Yaw * Deg2Rad));


        Vector2 lastYawSpeed = (yawVector - _lastYawVector) / Time.deltaTime;
        _lastYawVector = yawVector;
        
        _yawSpeed += Time.deltaTime * (yawVector + _k3 * lastYawSpeed - currentYawVector - _k1 * _yawSpeed) / _k2;
        
        current += Time.deltaTime * _speed;
        _speed += Time.deltaTime * (target + _k3 * lastSpeed - current - _k1 * _speed) / _k2;

        currentYawVector += _yawSpeed * Time.deltaTime;
        current.Yaw = Atan2(currentYawVector.y, currentYawVector.x) * Rad2Deg;
         
        return current;
    }
}
