using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Placement : MonoBehaviour
{
    [SerializeField] private string SpriteSheetName;
    [SerializeField] private Image _imagePlacement;
    [SerializeField] private TMP_Text _textLap;
    
    private Sprite[] _sprites;
    private Racer _racer;

    private void Awake() {
        _sprites = Resources.LoadAll<Sprite>("UI/" + SpriteSheetName);
    }

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
        _textLap.text = $"{currentLap} / {_racer.LapsToWin+1}";
    }

    
    private void UpdateUI(int index) {
        _imagePlacement.sprite = _sprites[index];
    }
}
