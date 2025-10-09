using System;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private int _index;
    [SerializeField] private bool _isFinishLine;
    
    public int Index => _index;
    public bool IsFinishLine => _isFinishLine;

    private void OnTriggerEnter(Collider other)
    {
        Racer racer = other.GetComponent<Racer>();
        if (racer != null)
        {
            racer.OnCheckPointPassed(this);
        }
    }
    
    public void SetData(int index, bool isFinishLine)
    {
        _index = index;
        _isFinishLine = isFinishLine;
    }
}
