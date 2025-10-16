using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownAnim : MonoBehaviour
{
    [SerializeField] protected List<Sprite> _spritesCountDown = new List<Sprite>();
    [SerializeField] protected Image _img;
    
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void CountDown(int i)
    {
        _img.sprite = _spritesCountDown[i];
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        float elapsedTime = 0;
        while (elapsedTime < 0.5f)
        {
            elapsedTime += Time.deltaTime;
            _rectTransform.localScale = Vector3.Lerp(new Vector3(3,3,3), new Vector3(1, 1, 1), elapsedTime / .5f);
            float p = elapsedTime / 0.5f;
            float a = Mathf.Lerp(0, 1, p);
            _img.color = new Color(1, 1, 1, a);
            yield return null;
        }
        _img.color = new Color(1, 1, 1, 1);

        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime;
            float a = Mathf.Lerp(1, 0, elapsedTime / 1);
            _img.color = new Color(1, 1, 1, a);
            _rectTransform.localScale = Vector3.Lerp(new Vector3(1,1,1), new Vector3(1.5f, 1.5f, 1.5f), elapsedTime / 1);
            yield return null;
        }
        _img.color = new Color(1, 1, 1, 0);
        
    }
}
