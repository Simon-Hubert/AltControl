using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class MenuCamera : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private float _changeFollowTime;

    private Racer[] _racers;

    private void Start() {
        RaceManager.Instance.OnRaceStarted += SetRacers;
    }
    
    private void OnDisable() {
        RaceManager.Instance.OnRaceStarted -= SetRacers;
    }

    public void SetRacers() {
        _racers = RaceManager.Instance.Racers.ToArray();
        StartCoroutine(ChangeFollowRoutine());
    }
    
    IEnumerator ChangeFollowRoutine() {
        while (true) {
            _camera.Target.TrackingTarget = _racers[Random.Range(0, _racers.Length)].transform;
            yield return new WaitForSeconds(_changeFollowTime);
        }
    }
}
