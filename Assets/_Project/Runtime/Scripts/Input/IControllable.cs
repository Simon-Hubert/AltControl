using UnityEngine;

public interface IControllable
{
    public void OnRightAxis(float value);
    public void OnLeftAxis(float value);
    public void OnRightButton();
    public void OnLeftButton();
}
