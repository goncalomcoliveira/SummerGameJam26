using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TopDownMovement : MonoBehaviour {
    
    [SerializeField] private TopDownInputReader inputSource;
    [SerializeField] private TopDownMovementStats moveStats;
    [SerializeField] private TopDownAnimationManager animationManager;
    
    private Rigidbody2D _rb;
    
    // Movement state
    private Vector2 _currentVelocity;
    
    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Handles physics-based movement updates.
    /// </summary>
    private void FixedUpdate() {
        HandleMovement(moveStats.acceleration, moveStats.deceleration, inputSource.Movement);
    }

    #region Movement
    
    /// <summary>
    /// Handles input processing, velocity calculation, animation updates, and movement application.
    /// </summary>
    private void HandleMovement(float acceleration, float deceleration, Vector2 moveInput) {
        
        var processedInput = ProcessMovementInput(inputSource.Movement);
        
        var isRunning = ShouldRun(processedInput);
        
        animationManager.SetMoveInput(processedInput);
        animationManager.SetRunningInput(isRunning);

        UpdateVelocity(processedInput, isRunning);
        ApplyVelocity();
    }
    
    /// <summary>
    /// Applies input normalization or snapping based on movement settings.
    /// </summary>
    private Vector2 ProcessMovementInput(Vector2 rawInput) {
        var filteredInput = ApplyDeadZones(rawInput);
        return !moveStats.snapInput ? filteredInput : filteredInput.normalized;
    }
    
    /// <summary>
    /// Applies dead zones to raw movement input.
    /// </summary>
    private Vector2 ApplyDeadZones(Vector2 input) {
        input.x = Mathf.Abs(input.x) < moveStats.deadZoneThreshold ? 0f : input.x;
        input.y = Mathf.Abs(input.y) < moveStats.deadZoneThreshold ? 0f : input.y;

        return input;
    }
    
    /// <summary>
    /// Determines whether the character should be running.
    /// </summary>
    private bool ShouldRun(Vector2 moveInput) {
        return inputSource.RunHeld && moveInput.magnitude > moveStats.runStickThreshold;
    }
    
    /// <summary>
    /// Updates the current velocity using acceleration/deceleration.
    /// </summary>
    private void UpdateVelocity(Vector2 moveInput, bool isRunning) {
        if (moveInput == Vector2.zero) {
            Decelerate();
            return;
        }
        var targetVelocity = CalculateTargetVelocity(moveInput, isRunning);
        AccelerateTowards(targetVelocity);
    }
    
    /// <summary>
    /// Calculates the velocity based on movement input and run state.
    /// </summary>
    private Vector2 CalculateTargetVelocity(Vector2 moveInput, bool isRunning) {
        var maxSpeed = isRunning
            ? moveStats.runMaxSpeed
            : moveStats.walkMaxSpeed;
        return moveInput * maxSpeed;
    }

    /// <summary>
    /// Accelerates the current velocity toward a target velocity.
    /// </summary>
    private void AccelerateTowards(Vector2 targetVelocity) {
        _currentVelocity = Vector2.Lerp(
            _currentVelocity,
            targetVelocity,
            moveStats.acceleration * Time.fixedDeltaTime
        );
    }

    /// <summary>
    /// Decelerates velocity toward zero when no input is present.
    /// </summary>
    private void Decelerate() {
        _currentVelocity = Vector2.Lerp(
            _currentVelocity,
            Vector2.zero,
            moveStats.deceleration * Time.fixedDeltaTime
        );
    }
    
    /// <summary>
    /// Applies the current velocity to the Rigidbody.
    /// </summary>
    private void ApplyVelocity() {
        _rb.linearVelocity = _currentVelocity;
    }
    
    #endregion
}