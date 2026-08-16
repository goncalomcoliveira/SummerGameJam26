using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TopDownAnimationManager : MonoBehaviour {
    
    private Animator _animator;
    
    //Movement
    private Vector2 _moveInput;
    private Vector2 _lastMoveDirection = new(0f, -1f);   //Start Looking South
    private bool _isRunning;
    
    // Animator parameter hashes
    private static readonly int MoveXHash = Animator.StringToHash("MoveX");
    private static readonly int MoveYHash = Animator.StringToHash("MoveY");
    private static readonly int LastMoveXHash = Animator.StringToHash("LastMoveX");
    private static readonly int LastMoveYHash = Animator.StringToHash("LastMoveY");
    private static readonly int MoveMagnitudeHash = Animator.StringToHash("MoveMagnitude");
    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");
    
    void Awake() {
        _animator = GetComponent<Animator>();
    }

    void Update() {
        UpdateAnimatorParameters();
    }
    
    /// <summary>
    /// Sends current movement state into the Animator.
    /// </summary>
    private void UpdateAnimatorParameters() {
        
        var normalizedMove = _moveInput.normalized;

        _animator.SetFloat(MoveXHash, normalizedMove.x);
        _animator.SetFloat(MoveYHash, normalizedMove.y);

        _animator.SetFloat(LastMoveXHash, _lastMoveDirection.x);
        _animator.SetFloat(LastMoveYHash, _lastMoveDirection.y);

        _animator.SetFloat(MoveMagnitudeHash, _moveInput.magnitude);
        
        _animator.SetBool(IsRunningHash, _isRunning);
    }
    
    /// <summary>
    /// Setter for the current movement input used for animation.
    /// </summary>
    public void SetMoveInput(Vector2 input) {
        _moveInput = input;

        if (input != Vector2.zero) {
            _lastMoveDirection = input.normalized;
        }
    }
    
    /// <summary>
    /// Setter for the running state used for animation.
    /// </summary>
    public void SetRunningInput(bool isRunning) {
        _isRunning = isRunning;
    }
}