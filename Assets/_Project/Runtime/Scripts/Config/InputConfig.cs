using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputConfig", menuName = "InputConfig")]
public class InputConfig : ScriptableObject
{
    public InputActionReference RightAxis;
    public InputActionReference LeftAxis;
    public InputActionReference RightButton;
    public InputActionReference LeftButton;
}
