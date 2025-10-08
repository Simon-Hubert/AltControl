using UnityEngine;

public abstract class CameraSmoother : ScriptableObject
{
    public abstract CameraConfiguration Smooth(CameraConfiguration current, CameraConfiguration target);
}
