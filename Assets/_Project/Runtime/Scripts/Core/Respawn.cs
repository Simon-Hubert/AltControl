using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public delegate void RespawnHandle(Vector3 position, Quaternion rotation);

public class Respawn : MonoBehaviour
{
    [SerializeField] private GameObject _chariotPrefab;
    [SerializeField] private Transform _transform;
    [SerializeField] private GameObject _visual;
    [SerializeField] private CollisionManager _collisions;
    [SerializeField] private Rigidbody _rb;
    
    private CheckPoint _spawnPoint;
    private bool _isRespawning;
    
    private void OnEnable() {
        CollisionManager collisionManager = GetComponent<CollisionManager>();
        collisionManager.OnRespawn += OnRespawn;
    }
    
    private void OnDisable() {
        CollisionManager collisionManager = GetComponent<CollisionManager>();
        collisionManager.OnRespawn -= OnRespawn;
    }

    private void Update() {
        if(Vector3.Dot(_transform.up, Vector3.up) < 0.7) OnRespawn();
    }

    public void OnRespawn() {
        if (_isRespawning) return;
        _isRespawning = true;
        Instantiate(_chariotPrefab, _transform.position, transform.rotation);
        _visual.SetActive(false);
        _collisions.enabled = false;
        StartCoroutine(RespawnRoutine());
    }
    
    private void RespawnChar(Vector3 position, Quaternion rotation) {
        _visual.SetActive(true);
        StartCoroutine(RespawnInvincibility());
        _transform.position = position;
        _transform.rotation = rotation;
        _rb.linearVelocity = Vector3.zero;
        _isRespawning = false;
    }

    public void SetSpawnPoint(CheckPoint cp) {
        _spawnPoint = cp;
    }

    private void AddToRespawnQueue() {
        _spawnPoint.AddToRespawnQueue(RespawnChar);
    }

    private IEnumerator RespawnRoutine() {
        float t = Resources.Load<ChariotConfig>("ChariotConfig").WaitForRespawnTime;
        yield return new WaitForSeconds(t);
        AddToRespawnQueue();
    }

    private IEnumerator RespawnInvincibility() {
        float t = Resources.Load<ChariotConfig>("ChariotConfig").RespawnInvincibilityTime;
        yield return new WaitForSeconds(t);
        _collisions.enabled = true;
    }
}
