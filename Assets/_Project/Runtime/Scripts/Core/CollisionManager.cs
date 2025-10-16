using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CollisionManager : MonoBehaviour
{
    private CollisionController[] collisionControllers;
    
    [SerializeField] private UnityEvent _onRespawn;
    public event Action OnRespawn;
    public event Action<Vector3> OnCollide;

    private void OnEnable() {
        collisionControllers = GetComponentsInChildren<CollisionController>();
        foreach (CollisionController controller in collisionControllers) {
            controller.OnRespawn += Respawn;
            controller.OnCollide += Collide;
        }
    }

    private void OnDisable() {
        foreach (CollisionController controller in collisionControllers) {
            controller.OnRespawn -= Respawn;
            controller.OnCollide -= Collide;
        }
    }

    private void Respawn() {
        _onRespawn?.Invoke();
        OnRespawn?.Invoke();
    }

    private void Collide(Vector3 force) {
        OnCollide?.Invoke(force);
    }
}
