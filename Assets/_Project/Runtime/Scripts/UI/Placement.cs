using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Placement : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Image _imagePlacement;
    [SerializeField] private TMP_Text _textLap;
    [SerializeField] private float _animationDuration;
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private AnimationCurve _sizeCurve;
    
    private Racer _racer;
    private float _baseSize;
    private float _baseTextSize;
    

    private void Start() {
        _baseSize = _imagePlacement.rectTransform.localScale.x;
        _baseTextSize = _textLap.rectTransform.localScale.x;

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
        StartCoroutine(AnimateLapUI(currentLap));
    }

    
    private void UpdateUI(int index) {
        StartCoroutine(AnimateUI(index));
    }

    IEnumerator AnimateUI(int index) {
        float t = 0;
        bool hasImageChanged = false;
        
        while (t < _animationDuration) {
            t += Time.deltaTime;
            float p = t / _animationDuration;
            float sP = _sizeCurve.Evaluate(p);
            p = _animationCurve.Evaluate(p);
            
            float angle = Mathf.Lerp(0, 360, p);
            float size = Mathf.Lerp(1, 1.5f, sP);

            if (!hasImageChanged && p > 0.5) {
                hasImageChanged = true;
                _imagePlacement.sprite = _sprites[index];
            }
            
            _imagePlacement.rectTransform.localRotation = Quaternion.Euler(0,0,angle);
            _imagePlacement.rectTransform.localScale = new Vector3(_baseSize * size, _baseSize * size, _baseSize * size);
            yield return new WaitForEndOfFrame();
        }
        
        _imagePlacement.rectTransform.localRotation = Quaternion.Euler(0,0,0);
        _imagePlacement.rectTransform.localScale = new Vector3(_baseSize, _baseSize, _baseSize);
    }
    
    IEnumerator AnimateLapUI(int currentLap) {
        float t = 0;
        bool hasImageChanged = false;
        
        while (t < _animationDuration) {
            t += Time.deltaTime;
            float p = t / _animationDuration;
            float sP = _sizeCurve.Evaluate(p);
            p = _animationCurve.Evaluate(p);
            
            float angle = Mathf.Lerp(0, 360, p);
            float size = Mathf.Lerp(1, 1.5f, sP);

            if (!hasImageChanged && p > 0.5) {
                hasImageChanged = true;
                _textLap.text = $"{currentLap + 1} / {_racer.LapsToWin+1}";
            }
            
            _textLap.rectTransform.localRotation = Quaternion.Euler(0,0,angle);
            _textLap.rectTransform.localScale = new Vector3(_baseTextSize * size, _baseTextSize * size, _baseTextSize * size);
            yield return new WaitForEndOfFrame();
        }
        
        _textLap.rectTransform.localRotation = Quaternion.Euler(0,0,0);
        _textLap.rectTransform.localScale = new Vector3(_baseTextSize, _baseTextSize, _baseTextSize);
    }
}
