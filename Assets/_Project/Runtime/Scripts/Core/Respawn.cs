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
    
    private void OnEnable() {
        CollisionManager collisionManager = GetComponent<CollisionManager>();
        collisionManager.OnRespawn += OnRespawn;
    }
    
    private void OnDisable() {
        CollisionManager collisionManager = GetComponent<CollisionManager>();
        collisionManager.OnRespawn -= OnRespawn;
    }

    private void OnRespawn() {
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
