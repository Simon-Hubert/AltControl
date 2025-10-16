using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class FXProxy : MonoBehaviour
{
    [SerializeField] private CollisionManager _collisionManager;
    [SerializeField] private ChariotController _chariot;
    [SerializeField] private UnityEvent UnityOnCrash;
    [SerializeField] private UnityEvent UnityOnBoost;
    [SerializeField] private UnityEvent<Vector3> UnityOnCollision;
    [SerializeField] private UnityEvent UnityOnStartUp;

    private void Start()
    {
        _chariot.OnBoost += UnityOnBoost.Invoke;
        _collisionManager.OnCollide += UnityOnCollision.Invoke;
        _collisionManager.OnRespawn += UnityOnCrash.Invoke;
        RaceManager.Instance.OnRaceStarted += UnityOnStartUp.Invoke;
    }
}
