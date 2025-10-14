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
        Chariot oldChariot = GetComponent<Chariot>();
        bool player = oldChariot.UnPossess();
        Chariot newChariot = Instantiate(_chariotPrefab).GetComponent<Chariot>();
        if (player) {
            newChariot.Possess<PlayerInput>(oldChariot.RacerId);
        }
        else {
            newChariot.Possess<AIInput>(oldChariot.RacerId);
        }
        oldChariot.RacerId = -1;
        if (_spawnPoint == null) {
            newChariot.transform.position = transform.position + transform.forward * -15f + transform.up;
            newChariot.transform.rotation = transform.rotation;
        }
        else {
            newChariot.transform.position = _spawnPoint.position;
            newChariot.transform.rotation = _spawnPoint.rotation;
        }
        
        Destroy(this);
    }

    IEnumerator RespawnRoutine() {
        yield return new WaitForSeconds(1f);
        RespawnChar();
    }
}
