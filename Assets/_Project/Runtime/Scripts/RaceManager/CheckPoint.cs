using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private int _index;
    [SerializeField] private bool _isFinishLine;
    [SerializeField] private float _width;

    private bool _passed = false;
    
    public int Index => _index;
    public bool IsFinishLine => _isFinishLine;
    private float _respawnBusTime;
    
    private Queue<RespawnHandle> _respawnQueue = new Queue<RespawnHandle>();

    private void OnTriggerEnter(Collider other)
    {
        Racer racer = other.GetComponent<Racer>();
        if (racer != null)
        {
            racer.OnCheckPointPassed(this);
            if ( _isFinishLine && racer.CurrentLap == racer.LapsToWin - 1 && !_passed)
            {
                _passed = true;
                RaceManager.Instance.LastLap();
            }
        }
    }
    
    public void SetData(int index, bool isFinishLine)
    {
        _index = index;
        _isFinishLine = isFinishLine;
    }

    private void Start() {
        _respawnBusTime = Resources.Load<ChariotConfig>("ChariotConfig").RespawnBusTimer;
        StartCoroutine(RespawnRoutine());
    }
    
    private void RespawnQueue() {
        if(_respawnQueue.Count <= 0 ) return;
        int toRespawn = Mathf.Min(_respawnQueue.Count, 5);

        if (toRespawn == 1) {
            _respawnQueue.Dequeue()(transform.position, transform.rotation * Quaternion.Euler(0,90,0));
            return;
        }
        
        for (int i = 0; i < toRespawn; i++) {
            Vector3 pos = transform.position + (transform.forward * _width / 2) - (transform.forward * (i * _width) / (toRespawn-1));
            _respawnQueue.Dequeue()(pos, transform.rotation * Quaternion.Euler(0,90,0));
        }
    }
    
    public void AddToRespawnQueue(RespawnHandle respawnChar) {
        _respawnQueue.Enqueue(respawnChar);
    }

    private IEnumerator RespawnRoutine() {
        while (true) {
            yield return new WaitForSeconds(_respawnBusTime);
            RespawnQueue();
        }
    }
    
    private void OnDrawGizmosSelected() {
        Gizmos.DrawLine(
            transform.position + transform.forward * _width / 2,
            transform.position - transform.forward * _width / 2
        );
        
        for (int i = 0; i < 5; i++) {
            Vector3 pos = transform.position + (transform.forward * _width / 2) - (transform.forward * i * _width / (5-1));
            Gizmos.DrawSphere(pos, 0.5f);
        }
    }

    
}
