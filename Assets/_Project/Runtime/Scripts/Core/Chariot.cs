using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Chariot : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] AudioSource _charSource, _horseSource;
    private Input _input;

    public UnityEvent UnityOnCrash;
    public UnityEvent UnityOnBoost;

    public int RacerId
    {
        get;
        set;
    }

    private void Start() {
        _input = GetComponent<Input>();
    }

    public void Possess<T>() where T : Input {
        _input = transform.AddComponent<T>();
        if (_input.IsPlayer) {
            _camera.enabled = true;
        }
    }

    public bool UnPossess() {
        _camera.enabled = false;
        bool isPlayer = _input.IsPlayer;
        Destroy(_input);
        return isPlayer;
    }   

    
}
