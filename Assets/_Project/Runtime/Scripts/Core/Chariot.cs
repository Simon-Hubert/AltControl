using System;
using Unity.Mathematics;
using UnityEngine;

public class Chariot : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private readonly SecondOrderDynamics3D _positionDynamic = new SecondOrderDynamics3D();
    private readonly SecondOrderDynamicsAngle _rotationDynamic = new SecondOrderDynamicsAngle();

    private void Awake() {
        ChariotConfig config = Resources.Load<ChariotConfig>("ChariotConfig");

        _positionDynamic.Init(_target.position, config.ChariotPosF, config.ChariotPosZ, config.ChariotPosR);
        _rotationDynamic.Init(_target.rotation.eulerAngles.z, config.ChariotRotF, config.ChariotRotZ, config.ChariotRotR);
    }

    private void Update() {
        transform.position = _positionDynamic.Update(Time.deltaTime, _target.position);
        transform.rotation = Quaternion.Euler(0,0, _rotationDynamic.Update(Time.deltaTime, _target.rotation.eulerAngles.z));
    }
}
