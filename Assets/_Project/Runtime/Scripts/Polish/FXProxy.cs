using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class FXProxy : MonoBehaviour
{
    [SerializeField] private bool _isPlayer = false;
    
    [SerializeField] private CollisionManager _collisionManager;
    [SerializeField] private CollisionController _collisionControllerHorse, _collisionControllerChariot;
    [SerializeField] private ChariotController _chariot;
    
    [SerializeField] private UnityEvent UnityOnCrash;
    [SerializeField] private UnityEvent UnityOnBoost;
    [SerializeField] private UnityEvent<Vector3> UnityOnCollision;
    [SerializeField] private UnityEvent UnityOnStartUp;

    private Vector3 _spawnPos;
    private Camera _camera;

    private void Start()
    {
        if (_isPlayer)
        {
            _camera = Camera.main;
            _chariot.OnBoost += _camera.gameObject.GetComponentInChildren<ParticleSystem>().Play;
        }
        _chariot.OnBoost += UnityOnBoost.Invoke;
        _collisionManager.OnCollide += UnityOnCollision.Invoke;
        _collisionManager.OnRespawn += UnityOnCrash.Invoke;
        RaceManager.Instance.OnRaceStarted += UnityOnStartUp.Invoke;
        _collisionControllerHorse.OnCrash += SetSFXPos;
        _collisionControllerChariot.OnCrash += SetSFXPos;
    }

    public void SetSFXPos(Vector3 position)
    {
        _spawnPos = position;
    }
}
