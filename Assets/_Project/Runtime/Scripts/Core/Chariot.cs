using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

public class Chariot : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private AudioSource _charSource, _horseSource;
    [SerializeField] List<AudioClip> _clipCollision = new List<AudioClip>();
    private Input _input;
    private SpeedProxy _speed;
    private float _maxSpeed;
    private float _maxFOV;
    private float _minFOV;

    private AudioSource _mainSource;

    public int RacerId
    {
        get;
        set;
    }

    private void Start() {
        _mainSource = GetComponent<AudioSource>();
        _input = GetComponent<Input>();
        _speed = GetComponentInChildren<SpeedProxy>();
        
        ChariotConfig config = Resources.Load<ChariotConfig>("ChariotConfig");
        _maxSpeed = config.MaxSpeed;
        _maxFOV = config.MaxFOV;
        _minFOV = config.MinFOV;
    }

    private void Update() {
        if(_camera) _camera.Lens.FieldOfView = GetFOV(_speed.ForwardSpeed);
    }
    
    private float GetFOV(float currentSpeed) {
        float iL = Mathf.InverseLerp(0, _maxSpeed, currentSpeed);
        return Mathf.Lerp(_minFOV, _maxFOV, iL);
    }

    #region SD

    public void PlayCollisionSound()
    {
        AudioClip  a = _clipCollision[UnityEngine.Random.Range(0, _clipCollision.Count)];
        _mainSource.PlayOneShot(a);
    }

    public void PlayLoopSounds()
    {
        _charSource.Play();
        _horseSource.Play();
    }

    #endregion

    
}
