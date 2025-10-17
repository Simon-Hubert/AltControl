using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndRace : MonoBehaviour
{
    [SerializeField] private GameObject _uiInput;
    [SerializeField] private GameObject _camera;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _image, _classementImage;
    [SerializeField] private float _timerToSwitchOnMainmenu = 10f;

    private GameObject _playerObj;
    void Start()
    {
        _canvasGroup.alpha = 0;
    }

    public void WinnerRace()
    {
        _playerObj = RaceManager.Instance.PlayerObj;
        MenuCamera cam = Instantiate(_camera).GetComponent<MenuCamera>();
        cam.SetRacers();
        Destroy(_playerObj);
        Instantiate(_uiInput);
        _image.sprite = _classementImage.sprite;
        _classementImage.enabled = false;
        _canvasGroup.alpha = 1;
    }
}
