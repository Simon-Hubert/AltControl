using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AViewVolume : MonoBehaviour
{
    [SerializeField] private int _priority;
    [SerializeField] private AView _view;
    [SerializeField] private bool _isCutOnSwitch;

    private int _uid;

    private static int _nextUid;
    
    public AView View => _view;
    public int Priority => _priority;
    public int UID => _uid;

    protected bool isActive
    {
        get;
        private set;
    }

    private void Awake()
    {
        _uid = _nextUid++;
    }


    public virtual float ComputeSelfWeight()
    {
        return 1;
    }

    protected void SetActive(bool active)
    {
        if (active)
        {
            ViewVolumeBlender.instance.AddVolume(this);
            if (_isCutOnSwitch)
            {
                ViewVolumeBlender.instance.Update();
                CameraController.instance.Cut();
            }
        }
        else
        {
            ViewVolumeBlender.instance.RemoveVolume(this);
        }
        isActive = active;
    }
}
