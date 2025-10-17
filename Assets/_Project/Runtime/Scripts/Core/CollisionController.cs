using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CollisionController : MonoBehaviour
{
    private float _respawnForce;

    public event Action OnRespawn;
    public event Action<Vector3> OnCollide; 
    public event Action<Vector3> OnCrash;

    private void Awake() {
        ChariotConfig config = Resources.Load<ChariotConfig>("ChariotConfig");
        _respawnForce = config.RespawnForce * 100f;
    }

    private void OnCollisionEnter(Collision other) {
        Vector3 force = other.impulse; // C'est en World
        
        if (force.magnitude > _respawnForce) {
            OnCrash?.Invoke(other.GetContact(0).point);
            OnRespawn?.Invoke();
        }
        
        OnCollide?.Invoke(force);
    }
    
}
