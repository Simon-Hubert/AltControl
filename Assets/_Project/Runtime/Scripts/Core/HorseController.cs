using System;
using UnityEngine;

public class HorseController : MonoBehaviour, IControllable
{
    private struct HorsePhysics
    {
        private Vector3 acceleration;
        public Vector3 speed;
        public float brakeInput;
        public bool RushInput;
        
        private Vector3 _springForce;
        
        private float _friction;
        private float _brakeForce;
        private float _speed;
        private float _rushSpeed;
        
        public void Init(float friction, float brakeForce, float speed, float rushSpeed) {
            _friction = friction;
            _brakeForce = brakeForce;
            _speed = speed;
            _rushSpeed = rushSpeed;
        }
        

        public void AddSpringPhysics(Vector3 force) {
            _springForce = force;
        }

        public void Update(Vector3 forward) {
            acceleration = -speed * _friction;
            acceleration += _springForce;
            acceleration += -speed * (_brakeForce * brakeInput);
            acceleration += forward * _speed;
            
            if (RushInput) {
                speed += forward * _rushSpeed;
                RushInput = false;
            }
            speed += acceleration * Time.deltaTime;
        }
    }
    
    [SerializeField] private Transform _leftHorse;
    [SerializeField] private Transform _rightHorse;
    [SerializeField] private Transform _center;

    private float _k;

    private HorsePhysics _leftHorsePhysics;
    private HorsePhysics _rightHorsePhysics;

    private float _l0;

    private void Awake() {
        ChariotConfig config = Resources.Load<ChariotConfig>("ChariotConfig");
        _k = config.HorseSpringFactor;
        _leftHorsePhysics.Init(config.HorseFriction, config.HorseBrakeForce, config.HorseSpeed, config.RushForce);
        _rightHorsePhysics.Init(config.HorseFriction, config.HorseBrakeForce, config.HorseSpeed, config.RushForce);
        _l0 = (_leftHorse.position - _rightHorse.position).magnitude;
    }

    private void Update() {
        float x = (_leftHorse.position - _rightHorse.position).magnitude - _l0;
        Vector3 springForce = (_leftHorse.position - _rightHorse.position).normalized * (-_k * x);
        
        _leftHorsePhysics.AddSpringPhysics(springForce);
        _rightHorsePhysics.AddSpringPhysics(-springForce);

        _leftHorsePhysics.Update(_center.up);
        _rightHorsePhysics.Update(_center.up);
        _leftHorse.position += _leftHorsePhysics.speed * Time.deltaTime;
        _rightHorse.position += _rightHorsePhysics.speed * Time.deltaTime;
    }

    public void OnRightAxis(float value) {
        _rightHorsePhysics.brakeInput = value;
    }
    
    public void OnLeftAxis(float value) {
        _leftHorsePhysics.brakeInput = value;
    }
    
    public void OnRightButton() {
        _rightHorsePhysics.RushInput = true;
        Debug.Log("RushRight");
    }
    
    public void OnLeftButton() {
        _leftHorsePhysics.RushInput = true;
    }
}
