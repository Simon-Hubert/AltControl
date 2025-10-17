using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour, IControllable
{
    private bool _rightRushInput;
    private bool _leftRushInput;
    
    private bool RushInput => _rightRushInput && _leftRushInput;
    private bool _gameStarted;

    [SerializeField] private string _sceneName;

    private void Update() {
        if (RushInput && !_gameStarted) {
            _gameStarted = true;
            SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
        }
    }

    public void OnRightAxis(float value) { }
    
    public void OnLeftAxis(float value) { }
    
    public void OnRightButton() {
        StartCoroutine(ResetRightBoolCoroutine());
    }
    
    public void OnLeftButton() {
        StartCoroutine(ResetLeftBoolCoroutine());
    }

    public void Enable(bool enabled) { }

    private IEnumerator ResetRightBoolCoroutine() {
        _rightRushInput = true;
        yield return new WaitForSeconds(0.2f);
        _rightRushInput = false;
    }
    
    private IEnumerator ResetLeftBoolCoroutine() {
        _leftRushInput = true;
        yield return new WaitForSeconds(0.2f);
        _leftRushInput = false;
    }
}
