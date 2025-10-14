using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Respawn : MonoBehaviour
{
    [SerializeField] private GameObject _chariotPrefab;
    
    [SerializeField] private Transform _spawnPoint;
    
    private void OnEnable() {
        CollisionManager collisionManager = GetComponent<CollisionManager>();
        collisionManager.OnRespawn += OnRespawn;
    }
    
    private void OnDisable() {
        CollisionManager collisionManager = GetComponent<CollisionManager>();
        collisionManager.OnRespawn -= OnRespawn;
    }

    private void OnRespawn() {
        StartCoroutine(RespawnRoutine());
    }
    
    private void RespawnChar() {
        Instantiate(_chariotPrefab, transform.position, transform.rotation);
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;
    }

    IEnumerator RespawnRoutine() {
        yield return new WaitForSeconds(1f);
        RespawnChar();
    }
}
