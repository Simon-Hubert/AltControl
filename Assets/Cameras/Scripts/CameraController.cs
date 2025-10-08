using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    [field: SerializeField] public Camera Camera { get; set; }
    [SerializeField] private CameraSmoother _smoother;
    private CameraConfiguration _configuration;
    public static CameraController instance;

    private readonly List<AView> _activeViews = new List<AView>();
    private CameraConfiguration _target;
    private bool _isCutRequested;
    
    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("Y a plusieurs instances de ton singleton (askip)");
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void Update() {
        _target = ComputeAverage();
        if (_isCutRequested)
        {
            _configuration = _target;
            _isCutRequested = false;
        }
        else
        {
            _configuration = _smoother.Smooth(_configuration, _target);
        }
        ApplyConfiguration();
    }
    
    private void ApplyConfiguration() {
        Camera.transform.rotation = _configuration.GetRotation();
        Camera.transform.position = _configuration.GetPosition();
        Camera.fieldOfView = _configuration.Fov;
    }

    public void AddView(AView view) {
        if(!_activeViews.Contains(view)) _activeViews.Add(view);
    }
    
    public void RemoveView(AView view) {
        if(_activeViews.Contains(view)) _activeViews.Remove(view);
    }

    public void Cut()
    {
        _isCutRequested = true;
    }

    private CameraConfiguration ComputeAverage() {
        CameraConfiguration config = new CameraConfiguration();
        Vector2 yawVector = Vector2.zero;
        float weightSum = 0f;
        
        foreach (AView view in _activeViews) {
            config += view.Weight * view.GetConfiguration();
            yawVector += view.Weight * new Vector2(
                Mathf.Cos(view.GetConfiguration().Yaw * Mathf.Deg2Rad),
                Mathf.Sin(view.GetConfiguration().Yaw * Mathf.Deg2Rad));
            weightSum += view.Weight;
        }
        config /= weightSum;
        config.Yaw += Mathf.DeltaAngle(config.Yaw,  Mathf.Atan2(yawVector.y, yawVector.x) * Mathf.Rad2Deg); //Angle de la somme des vecteurs de yaw
        return config;
    }

    private void OnDrawGizmos() {
        _configuration.DrawGizmo(Color.magenta);
    }
}
