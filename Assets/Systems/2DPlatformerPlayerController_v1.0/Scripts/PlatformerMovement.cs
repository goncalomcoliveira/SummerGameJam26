using UnityEngine;

public class PlatformerMovement: MonoBehaviour {
    
    // Serialized References
    [Header("External References")]
    [SerializeField] private PlatformerInputReader inputSource;
    [SerializeField] private PlatformerMovementStats moveStats;

    [Header("Colliders")]
    [SerializeField] private CapsuleCollider2D bodyCollider;
    
    // Components & Cached Data
    private Rigidbody2D _rb;
    private bool _cachedQueryStartInColliders;
    
    // Player State
    private PlayerStateInfo _currentStateInfo = new(PlayerState.Idle);
    private PlayerStateInfo _previousStateInfo;
    
    // Time & Velocity
    private float _time;                // Internal clock (used for jump buffering & coyote time)
    private Vector2 _currentVelocity;   // Our own velocity we apply to the Rigidbody
    
    // Input Processing
    private Vector2 _processedMovementInput;    // Final movement input after snapping & dead zones
    
    // Collisions
    private bool _grounded;
    private float _frameLeftGrounded = float.MinValue;  // Time when the player last left the ground (for coyote time)
    
    // Turning
    private int _facingDirection = 1;      // 1 = right, -1 = left
    private bool _facingLocked;
    private float _facingLockEndTime;
    
    // Jumping
    private bool _jumpToConsume;        // Whether a ground/coyote jump is still available
    private int _airJumpsToConsume;     // Remaining mid-air jumps
    private bool _endedJumpEarly;       // Jump was released early (used for jump cut)
    private bool _bufferedJumpUsable;   // Jump input buffering
    private float _timeJumpWasPressed   // Timestamp for most recent jump press (for jump buffer).
        = float.NegativeInfinity;  
    private bool _coyoteUsable;         // Coyote time availability 
 
    // Landing
    private bool _justLanded;
    private float _landingImpactVelocity;
    
    // Wall Detection
    private bool _touchingWall;
    private int _wallDirection; // -1 = left wall, +1 = right wall
    
    // Dash
    private bool _isDashing;
    private float _dashEndTime;
    private int _dashesRemaining;
    
    // Computed properties
    private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + moveStats.jumpBuffer;      // Is a buffered jump still valid?
    private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + moveStats.coyoteTime;   // Is coyote time still valid?
    private bool IsWallSliding => moveStats.allowWallSliding && _touchingWall && !_grounded && _processedMovementInput.x * _wallDirection > 0f && _currentVelocity.y <= 0f;
    
    #region Unity Lifecycle
    
    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
    }
    
    private void Update() {
        _time += Time.deltaTime;
        ProcessInput();
    }
    
    private void FixedUpdate() {
        
        if (_currentStateInfo.State == PlayerState.Land) {
            _justLanded = false;
        }
        
        UpdateFacingLock();
        CheckCollisions();

        UpdateState();
        
        if (HandleMovementOverrides()) {
            ApplyVelocity();
            return;
        }
        
        HandleJump();
        HandleDirection();
        HandleGravity();
        
        ApplyVelocity();
    }
    
    #endregion

    #region Overrides

    private bool HandleMovementOverrides() {
        if (_isDashing) {
            HandleDash();
            return true;
        }
        
        //Handle stuns, knockbacks, anything that can override movement
        
        return false;
    }

    #endregion
    
    #region Player State
    
    private void UpdateState() {
        var newStateInfo = DetermineState();

        if (newStateInfo.State == _currentStateInfo.State)
            return;

        _previousStateInfo = _currentStateInfo;
        _currentStateInfo = newStateInfo;

        PlatformerPlayerEvents.StateChanged?.Invoke(_previousStateInfo, _currentStateInfo);
    }
    
    private PlayerStateInfo DetermineState() {

        // Highest priority states first
        if (_isDashing)
            return new PlayerStateInfo(PlayerState.Dash, moveStats.dashDuration);

        if (IsWallSliding)
            return new PlayerStateInfo(PlayerState.WallSlide);

        // Landing check
        if (_justLanded && _landingImpactVelocity > moveStats.minLandVelocity) {
            return new PlayerStateInfo(PlayerState.Land, moveStats.landDuration);
        }
        
        if (!_grounded) {
            return _currentVelocity.y > 0.1f ? new PlayerStateInfo(PlayerState.Jump) : new PlayerStateInfo(PlayerState.Fall);
        }

        // Grounded states
        if (Mathf.Abs(_currentVelocity.x) < 0.1f)
            return new PlayerStateInfo(PlayerState.Idle);

        var isRunning =
            inputSource.RunHeld &&
            Mathf.Abs(_currentVelocity.x) > moveStats.walkMaxSpeed + 0.1f;

        return isRunning ? new PlayerStateInfo(PlayerState.Run) : new PlayerStateInfo(PlayerState.Walk);
    }
    
    #endregion
    
    #region Input Handling
    
    /// <summary>
    /// Reads raw input, applies snapping + dead zones and buffers jump input.
    /// </summary>
    private void ProcessInput() {
        _processedMovementInput = 
            moveStats.snapInput 
                ? new Vector2(Mathf.Sign(inputSource.Movement.x), Mathf.Sign(inputSource.Movement.y)) 
                : inputSource.Movement;

        var moveAmount = Mathf.Clamp(Mathf.Abs(_processedMovementInput.x), moveStats.minMoveAmount, 1f);
        PlatformerPlayerEvents.MoveAmountChanged?.Invoke(moveAmount);
        
        HandleDeadZones();

        if (inputSource.JumpPressed) {
            _timeJumpWasPressed = _time;
            _bufferedJumpUsable = true;
        }
        
        if (inputSource.DashPressed 
            && moveStats.allowDashing
            && _dashesRemaining > 0 
            && !_isDashing) {
            StartDash();
        }
    }

    /// <summary>
    /// Removes small input noise near zero.
    /// </summary>
    private void HandleDeadZones() {
        if (Mathf.Abs(inputSource.Movement.x) < moveStats.horizontalDeadZoneThreshold) {
            _processedMovementInput.x = 0;
        }
        if (Mathf.Abs(inputSource.Movement.y) < moveStats.verticalDeadZoneThreshold) {
            _processedMovementInput.y = 0;
        }
    }
    
    #endregion
    
    #region Collision Detection
    
    /// <summary>
    /// Checks ground & ceiling collisions using capsule casts
    /// and updates grounded / coyote state.
    /// </summary>
    private void CheckCollisions() {
        Physics2D.queriesStartInColliders = false;

        // Ground and Ceiling collision detection
        bool groundHit = Physics2D.CapsuleCast(
            bodyCollider.bounds.center, 
            bodyCollider.size, 
            bodyCollider.direction, 
            0, 
            Vector2.down, 
            moveStats.grounderDistance, 
            ~moveStats.playerLayer
        );
        bool ceilingHit = Physics2D.CapsuleCast(
            bodyCollider.bounds.center, 
            bodyCollider.size, 
            bodyCollider.direction, 
            0, 
            Vector2.up, 
            moveStats.grounderDistance, 
            ~moveStats.playerLayer
        );
        
        // Wall detection
        bool leftWallHit = Physics2D.CapsuleCast(
            bodyCollider.bounds.center,
            bodyCollider.size,
            bodyCollider.direction,
            0,
            Vector2.left,
            moveStats.wallCheckDistance,
            ~moveStats.playerLayer
        );
        bool rightWallHit = Physics2D.CapsuleCast(
            bodyCollider.bounds.center,
            bodyCollider.size,
            bodyCollider.direction,
            0,
            Vector2.right,
            moveStats.wallCheckDistance,
            ~moveStats.playerLayer
        );

        _touchingWall = (leftWallHit || rightWallHit) && !_grounded;
        _wallDirection = leftWallHit ? -1 : rightWallHit ? 1 : 0;

        // While wall sliding, force the player to face away the wall.
        if (IsWallSliding) {
            SetFacingDirection(-_wallDirection);
        }
        
        // Stop vertical velocity if hitting a ceiling
        if (ceilingHit) _currentVelocity.y = Mathf.Min(0, _currentVelocity.y);

        // Landing on the ground
        if (!_grounded && groundHit) {
            _justLanded = true;
            _landingImpactVelocity = Mathf.Abs(_currentVelocity.y);
            _grounded = true;
            _coyoteUsable = true;
            _endedJumpEarly = false;
            _jumpToConsume = true;
            _airJumpsToConsume = Mathf.Max(0, moveStats.numberOfJumps - 1);
            _dashesRemaining = moveStats.maxDashes;
        }
        
        // Leaving the ground
        else if (_grounded && !groundHit) {
            _grounded = false;
            _frameLeftGrounded = _time;
            _justLanded = false;
        }

        Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
    }
    
    #endregion
    
    #region Jump Logic
    
    /// <summary>
    /// Handles jump buffering, coyote time, jump cut, and multi-jump logic.
    /// </summary>
    private void HandleJump() {

        // Jump cut
        if (!_endedJumpEarly && !_grounded && !inputSource.JumpHeld && _currentVelocity.y > 0) {
            _endedJumpEarly = true;
        }

        if (!HasBufferedJump) return;

        // Ground or coyote jump
        if (_jumpToConsume && (_grounded || CanUseCoyote)) {
            ExecuteGroundJump();
            return;
        }
        
        // Wall jump
        if (moveStats.allowWallSliding && _touchingWall && !_grounded) {
            ExecuteWallJump();
            return;
        }

        // Air jump
        if (_airJumpsToConsume > 0) {
            ExecuteAirJump();
        }
    }
    
    private void ExecuteGroundJump() {
        _jumpToConsume = false;
        _coyoteUsable = false;
        _bufferedJumpUsable = false;
        _timeJumpWasPressed = 0;
        _endedJumpEarly = false;

        _currentVelocity.y = moveStats.jumpPower;
    }
    
    private void ExecuteAirJump() {
        _airJumpsToConsume--;
        _bufferedJumpUsable = false;
        _timeJumpWasPressed = 0;
        _endedJumpEarly = false;

        _currentVelocity.y = moveStats.jumpPower * moveStats.airJumpPowerMultiplier;
    }
    
    private void ExecuteWallJump() {
        _bufferedJumpUsable = false;
        _timeJumpWasPressed = 0;
        _endedJumpEarly = false;
        _coyoteUsable = false;

        // Consume air jumps so wall jumping can't be abused infinitely
        _airJumpsToConsume = Mathf.Max(0, _airJumpsToConsume - 1);

        var jumpDirection = -_wallDirection;
        
        _currentVelocity = new Vector2(
            jumpDirection * moveStats.wallJumpHorizontalPower,
            moveStats.wallJumpVerticalPower
        );
        
        // Face away from the wall
        SetFacingDirection(jumpDirection);

        // Lock facing during wall jump wind-up
        LockFacing(moveStats.wallJumpFacingLockTime);
    }
    
    #endregion

    #region Horizontal Movement
    
    private void HandleDirection() {
        
        // Turn Detection
        if (_processedMovementInput.x != 0 && !_touchingWall) {
            SetFacingDirection((int)Mathf.Sign(_processedMovementInput.x));
        }
        
        if (_processedMovementInput.x == 0) {
            var deceleration = 
                _grounded ? moveStats.groundDeceleration : moveStats.airDeceleration;
            
            _currentVelocity.x = Mathf.MoveTowards(
                _currentVelocity.x, 
                0, 
                deceleration * Time.fixedDeltaTime
            );
        }
        else {
            var maxSpeed = 
                inputSource.RunHeld && _processedMovementInput.magnitude > moveStats.runStickThreshold 
                    ? moveStats.runMaxSpeed 
                    : moveStats.walkMaxSpeed;
            _currentVelocity.x = Mathf.MoveTowards(
                _currentVelocity.x, 
                _processedMovementInput.x * maxSpeed, 
                moveStats.acceleration * Time.fixedDeltaTime
            );
        }
    }
    
    #endregion
    
    #region Gravity Application
    
    private void HandleGravity() {
        // If grounded, apply grounding gravity
        if (_grounded && _currentVelocity.y <= 0f) {
            _currentVelocity.y = moveStats.groundingForce;
            return;
        }
        
        var airborneGravity = moveStats.fallAcceleration;
        
        // Wall slide
        if (IsWallSliding) {
            airborneGravity *= moveStats.wallSlideGravityMultiplier;

            _currentVelocity.y = Mathf.MoveTowards(
                _currentVelocity.y,
                -moveStats.wallSlideMaxSpeed,
                airborneGravity * Time.fixedDeltaTime
            );

            return;
        }
        
        // Jump cut (early release)
        if (_endedJumpEarly && _currentVelocity.y > 0f) {
            airborneGravity *= moveStats.jumpCutGravityModifier;
        }
        
        // Apex hang
        if (!_grounded && Mathf.Abs(_currentVelocity.y) < moveStats.apexThreshold) {
            airborneGravity *= moveStats.apexGravityMultiplier;
        }
        
        // Apply airborne gravity
        _currentVelocity.y = Mathf.MoveTowards(
            _currentVelocity.y, 
            -moveStats.maxFallSpeed, 
            airborneGravity * Time.fixedDeltaTime
        );
    }
    
    #endregion

    #region Dash

    private void StartDash() {
        _isDashing = true;
        _dashesRemaining--;
        _dashEndTime = _time + moveStats.dashDuration;

        var dashDirection =
            _processedMovementInput.x != 0
                ? (int)Mathf.Sign(_processedMovementInput.x)
                : _facingDirection; // fallback facing

        _currentVelocity = new Vector2(
            dashDirection * moveStats.dashSpeed,
            0
        );
        
        SetFacingDirection(dashDirection);

        // Lock facing during the full dash duration
        LockFacing(moveStats.dashDuration);
    }
    
    private void HandleDash() {
        if (_time < _dashEndTime)
            return;

        _isDashing = false;

        // Refresh dashes if dash ended on the ground
        if (_grounded) {
            _dashesRemaining = moveStats.maxDashes;
        }
    }

    #endregion
    
    #region Final Velocity Application
    
    private void ApplyVelocity() => _rb.linearVelocity = _currentVelocity;

    #endregion
    
    #region Facing / Orientation
    
    /// <summary>
    /// Sets the player facing direction if allowed.
    /// This is the only place the facing direction should change.
    /// Respects facing locks (dash, wall jump wind-up, etc).
    /// </summary>
    private void SetFacingDirection(int direction) {
        if (direction == 0)
            return;

        // Prevent direction changes while locked
        if (_facingLocked)
            return;

        // Ignore redundant flips
        if (direction == _facingDirection)
            return;

        _facingDirection = direction;
        
        // Notify player state based listeners
        PlatformerPlayerEvents.Turned?.Invoke(direction);
    }
    
    /// <summary>
    /// Locks facing direction for a fixed duration.
    /// Used during wall jump wind-up and dashes to prevent input from fighting animations.
    /// </summary>
    private void LockFacing(float duration) {
        _facingLocked = true;
        _facingLockEndTime = _time + duration;
    }

    /// <summary>
    /// Updates the facing lock timer and releases it when expired.
    /// Called once per FixedUpdate.
    /// </summary>
    private void UpdateFacingLock() {
        if (_facingLocked && _time >= _facingLockEndTime) {
            _facingLocked = false;
        }
    }
    
    #endregion
}