using System;
using UnityEngine;
using UnityEngine.Events;

public class CollisionManager : MonoBehaviour
{
    private CollisionController[] collisionControllers;
    
    [SerializeField] private UnityEvent _onRespawn;
    public event Action OnRespawn;
    
    [SerializeField] private UnityEvent<Vector3> _onCollide;
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
        _onCollide?.Invoke(force);
        OnCollide?.Invoke(force);
    }
}
