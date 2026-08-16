using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlatformerInputReader : MonoBehaviour {

    private PlayerInput _playerInput;

    private InputAction _moveAction;
    private InputAction _runAction;
    private InputAction _jumpAction;
    private InputAction _dashAction;
    
    /// <summary>
    /// Current movement input vector.
    /// </summary>
    public Vector2 Movement => _moveAction.ReadValue<Vector2>();
    
    /// <summary>
    /// If the run input is currently held.
    /// </summary>
    public bool RunHeld => _runAction.IsPressed();
    
    public bool JumpPressed => _jumpAction.WasPressedThisFrame();
    public bool JumpHeld => _jumpAction.IsPressed();
    public bool JumpReleased => _jumpAction.WasReleasedThisFrame();
    
    public bool DashPressed => _dashAction.WasPressedThisFrame();

    private void Awake() {
        CacheInputReferences();
        EnableInput();
    }

    private void OnDisable() {
        DisableInput();
    }

    /// <summary>
    /// Caches input actions from the PlayerInput component.
    /// </summary>
    private void CacheInputReferences() {
        _playerInput = GetComponent<PlayerInput>();

        var actions = _playerInput.actions;
        _moveAction = actions["Move"];
        _runAction = actions["Run"];
        _jumpAction = actions["Jump"];
        _dashAction = actions["Dash"];
    }

    /// <summary>
    /// Enables the input action asset.
    /// </summary>
    private void EnableInput() {
        _playerInput.actions.Enable();
    }

    /// <summary>
    /// Disables the input action asset.
    /// </summary>
    private void DisableInput() {
        _playerInput.actions.Disable();
    }
}