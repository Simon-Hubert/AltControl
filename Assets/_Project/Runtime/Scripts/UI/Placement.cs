using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Placement : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Image _imagePlacement;
    [SerializeField] private TMP_Text _textLap;
    
    private Racer _racer;

    public void SetRacer(Racer racer) {
        _racer = racer;
        racer.OnPositionChange += UpdateUI;
        racer.OnFinishedLap += UpdateLapUI;
        UpdateLapUI(0);
    }
    
    private void OnDisable() {
        _racer.OnPositionChange -= UpdateUI;
        _racer.OnFinishedLap -= UpdateLapUI;

    }

    private void UpdateLapUI(int currentLap) {
        _textLap.text = $"{currentLap + 1} / {_racer.LapsToWin+1}";
    }

    
    private void UpdateUI(int index) {
        _imagePlacement.sprite = _sprites[index];
    }
}
