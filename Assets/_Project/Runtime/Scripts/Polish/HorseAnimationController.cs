using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class HorseAnimationController : MonoBehaviour
{
    private Animator _animator;
    private float _maxSpeed;
    private float _maxAnimationSpeed;
    private SpeedProxy _speed;


    void Start() {
        _animator = GetComponent<Animator>();
        _speed = GetComponentInParent<SpeedProxy>();

        ChariotConfig config = Resources.Load<ChariotConfig>("ChariotConfig");
        _maxSpeed = config.MaxSpeed;
        _maxAnimationSpeed = config.MaxAnimationSpeed;
        
        StartCoroutine(StartAnimator());
    }


    private void Update() {
        _animator.speed = GetAnimationSpeed(_speed.ForwardSpeed);
    }
    
    private float GetAnimationSpeed(float currentSpeed) {
        float iL = Mathf.InverseLerp(0, _maxSpeed, currentSpeed);
        return Mathf.Lerp(0, _maxAnimationSpeed, iL);
    }

    IEnumerator StartAnimator() {
        _animator.enabled = false;
        yield return new WaitForSeconds(Random.Range(0, 1.5f));
        _animator.enabled = true;
    }
}
