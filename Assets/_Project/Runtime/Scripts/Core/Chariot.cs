using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class Chariot : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private Transform _movingTransform;
    private Input _input;
    float _totalTime;
    private Racer _racer;
    
    public float TotalTime => _totalTime;
    public Transform MovingTransform => _movingTransform;

    public int RacerId
    {
        get;
        set;
    }

    private void Start() {
        _input = GetComponent<Input>();
        StartCoroutine(RespawnInvincibility());
    }

    public void Possess<T>(int racerId) where T : Input {
        _input = transform.AddComponent<T>();
        if (_input.IsPlayer) {
            _camera.enabled = true;
        }
        RacerId = racerId;
        if(RacerId < 0) return;
        _racer = RaceManager.instance.Racers[RacerId];
    }

    public bool UnPossess() {
        _camera.enabled = false;
        bool isPlayer = _input.IsPlayer;
        Destroy(_input);
        _racer = null;
        return isPlayer;
    }

    private void Update()
    {
        if (_racer == null || _racer.HasFinished) return;

        _totalTime += Time.deltaTime;
        _racer.TotalTime = _totalTime;
    }
    public void OnCheckPointPassed(CheckPoint checkPoint)
    {
        if(_racer == null || _racer.HasFinished) return;
        
        int expectedIndex = (_racer.LastCheckpointIndex + 1) % _racer.Checkpoints.Count;
        if (checkPoint.Index != expectedIndex)
            return;
        
        _racer.UpdateLastCheckpointIndex(checkPoint.Index);
        
        if (checkPoint.IsFinishLine)
        {
            _racer.UpdateCurrentLap(_racer.CurrentLap + 1);

            if (_racer.CurrentLap >= _racer.LapsToWin)
            {
                _racer.Finish();
                Debug.Log($"{_racer.RacerName} finished the race in {_totalTime:F2}s !");
                return;
            }
        }
        
        _racer.UpdateNextCheckpoint();

    }
    IEnumerator RespawnInvincibility() {
        CollisionManager collisions = GetComponent<CollisionManager>();
        collisions.enabled = false;
        float t = Resources.Load<ChariotConfig>("ChariotConfig").RespawnInvincibilityTime;
        yield return new WaitForSeconds(t);
        collisions.enabled = true;
    }
}
