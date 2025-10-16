using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : Input
{
    private InputActionReference _rightAxis;
    private InputActionReference _leftAxis;
    private InputActionReference _rightButton;
    private InputActionReference _leftButton;

    private IControllable _controllable;

    public override bool IsPlayer => true;


    private void Awake() {
        _controllable = GetComponent<IControllable>();
        _controllable.Enable(false);
    }

    private void OnEnable() {
        SetConfig();
        _rightAxis.action.performed += RightAxis;
        _rightAxis.action.canceled += RightAxis;
        _leftAxis.action.performed += LeftAxis;
        _leftAxis.action.canceled += LeftAxis;
        _rightButton.action.started += RightButton;
        _leftButton.action.started += LeftButton;
    }

    private void OnDisable() {
        SetConfig();
        _rightAxis.action.performed -= RightAxis;
        _rightAxis.action.canceled -= RightAxis;
        _leftAxis.action.performed -= LeftAxis;
        _leftAxis.action.canceled -= LeftAxis;

        _rightButton.action.started -= RightButton;
        _leftButton.action.started -= LeftButton;
    }

    public void StartUp()
    {
        _controllable.Enable(true);
    }
    private void RightAxis(InputAction.CallbackContext ctx ) {
        _controllable.OnRightAxis(ctx.ReadValue<float>());
    }
    
    private void LeftAxis(InputAction.CallbackContext ctx) {
        _controllable.OnLeftAxis(ctx.ReadValue<float>());
    }
    
    private void RightButton(InputAction.CallbackContext ctx) {
        _controllable.OnRightButton();
    }
    
    private void LeftButton(InputAction.CallbackContext ctx) {
        _controllable.OnLeftButton();
    }
    
    private void SetConfig() {
        InputConfig config = Resources.Load<InputConfig>("InputConfig");
        _rightAxis = config.RightAxis;
        _leftAxis = config.LeftAxis;
        _rightButton = config.RightButton;
        _leftButton = config.LeftButton;
    }
}
