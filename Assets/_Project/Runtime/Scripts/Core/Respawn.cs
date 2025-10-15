using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Respawn : MonoBehaviour
{
    [SerializeField] private GameObject _chariotPrefab;
    [SerializeField] private Transform _transform;
    [SerializeField] private GameObject _visual;
    [SerializeField] private CollisionManager _collisions;
    
    private Transform _spawnPoint;
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
        Debug.Log(Vector3.Dot(_transform.up, Vector3.up));
        if(Vector3.Dot(_transform.up, Vector3.up) < 0.7) OnRespawn();
    }

    private void OnRespawn() {
        if (_isRespawning) return;
        _isRespawning = true;
        Instantiate(_chariotPrefab, _transform.position, transform.rotation);
        _visual.SetActive(false);
        _collisions.enabled = false;
        StartCoroutine(RespawnRoutine());
    }
    
    private void RespawnChar() {
        _visual.SetActive(true);
        StartCoroutine(RespawnInvincibility());
        _transform.position = _spawnPoint.position;
        _transform.rotation = _spawnPoint.rotation * Quaternion.Euler(0,90,0);
        _isRespawning = false;
    }

    public void SetSpawnPoint(CheckPoint cp) {
        _spawnPoint = cp.transform;
    }

    IEnumerator RespawnRoutine() {
        yield return new WaitForSeconds(1f);
        RespawnChar();
    }
    
    IEnumerator RespawnInvincibility() {
        float t = Resources.Load<ChariotConfig>("ChariotConfig").RespawnInvincibilityTime;
        yield return new WaitForSeconds(t);
        _collisions.enabled = true;
    }
}
