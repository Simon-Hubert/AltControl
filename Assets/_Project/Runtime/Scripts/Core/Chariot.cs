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
    [SerializeField] AudioSource _charSource, _horseSource;
    [SerializeField] List<AudioClip> _clipCollision = new List<AudioClip>();
    private Input _input;

    private AudioSource _mainSource;
    
    public UnityEvent UnityOnCrash;
    public UnityEvent UnityOnBoost;

    public int RacerId
    {
        get;
        set;
    }

    private void Start() {
        _mainSource = GetComponent<AudioSource>();
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

    #region SD

    public void PlayCollisionSound()
    {
        AudioClip  a = _clipCollision[UnityEngine.Random.Range(0, _clipCollision.Count)];
        _mainSource.PlayOneShot(a);
    }

    #endregion

    
}
