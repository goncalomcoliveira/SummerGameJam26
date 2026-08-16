using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class TopDownInputReader : MonoBehaviour {

    private PlayerInput _playerInput;

    private InputAction _moveAction;
    private InputAction _runAction;

    /// <summary>
    /// Current movement input vector.
    /// </summary>
    public Vector2 Movement => _moveAction.ReadValue<Vector2>();
    
    /// <summary>
    /// If the run input is currently held.
    /// </summary>
    public bool RunHeld => _runAction.IsPressed();

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