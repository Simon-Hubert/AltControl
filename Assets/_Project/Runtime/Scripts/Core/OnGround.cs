using System;
using UnityEngine;
using UnityEngine.Events;

public class OnGround : MonoBehaviour
{
    [SerializeField] private LayerMask _mask;
    [SerializeField] private float _timeUntilDeath;
    [SerializeField] private float _distance;
    [SerializeField] private UnityEvent _onDeathFromSky;

    private float _timer;
    
    private void Update() {
        if (!Physics.Raycast(transform.position, -transform.up, _distance, _mask)) {
            _timer += Time.deltaTime;
        }
        else {
            _timer = 0;
        }

        if (_timer > _timeUntilDeath) {
            _onDeathFromSky.Invoke();
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawLine(transform.position, transform.position - transform.up * _distance);
    }
}
