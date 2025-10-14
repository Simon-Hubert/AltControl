using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class Chariot : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;
    private Input _input;

    public int RacerId
    {
        get;
        set;
    }

    private void Start() {
        _input = GetComponent<Input>();
        StartCoroutine(RespawnInvincibility());
    }

    public void Possess<T>() where T : Input {
        _input = transform.AddComponent<T>();
        if (_input.IsPlayer) {
            _camera.enabled = true;
        }
    }

    public bool UnPossess() {
        _camera.enabled = false;
        bool isPlayer = _input.IsPlayer;
        Destroy(_input);
        return isPlayer;
    }   

    IEnumerator RespawnInvincibility() {
        CollisionManager collisions = GetComponent<CollisionManager>();
        collisions.enabled = false;
        float t = Resources.Load<ChariotConfig>("ChariotConfig").RespawnInvincibilityTime;
        yield return new WaitForSeconds(t);
        collisions.enabled = true;
    }
}
