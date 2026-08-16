using System;
using UnityEngine;

/// <summary>
/// Listens to player movement/state events and drives
/// sprite orientation and animation playback.
/// </summary>
public class PlatformerAnimationManager: MonoBehaviour {
    
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    [SerializeField] private float minimumMoveAmount = 0.4f;
    
    private float _lockedUntil;
    private bool IsLocked => Time.time < _lockedUntil;
    
    private PlayerStateInfo _pendingState;
    private bool _hasPendingState;

    #region Animation Hashes

    // States
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Fall = Animator.StringToHash("Fall");
    private static readonly int Land = Animator.StringToHash("Land");
    private static readonly int Dash = Animator.StringToHash("Dash");
    private static readonly int WallSlide = Animator.StringToHash("WallSlide");

    // Parameters
    private static readonly int MoveAmount = Animator.StringToHash("MoveAmount");
    
    #endregion
    
    #region Unity Lifecycle
    
    private void OnEnable() { RegisterPlayerEvents(true); }
    private void OnDisable() { RegisterPlayerEvents(false); }

    private void Update() {
        if (!_hasPendingState || IsLocked) return;
        
        var stateToPlay = _pendingState;
        _hasPendingState = false;
        _pendingState = null;

        PlayState(stateToPlay);
    }
    
    #endregion
    
    #region Event Registration
    
    private void RegisterPlayerEvents(bool register) {
        if (register) {
            PlatformerPlayerEvents.StateChanged += HandleStateChange;
            PlatformerPlayerEvents.Turned += HandleTurn;
            PlatformerPlayerEvents.MoveAmountChanged += SetMoveAmount;
        }
        else {
            PlatformerPlayerEvents.StateChanged -= HandleStateChange;
            PlatformerPlayerEvents.Turned -= HandleTurn;
            PlatformerPlayerEvents.MoveAmountChanged -= SetMoveAmount;
        }
    }
    
    #endregion

    #region Player State Handling

    /// <summary>
    /// Responds to player state changes by transitioning
    /// to the appropriate animation state.
    /// </summary>
    private void HandleStateChange(PlayerStateInfo from, PlayerStateInfo to) {

        // If we're locked, buffer the latest requested state
        if (IsLocked) {
            _pendingState = to;
            _hasPendingState = true;
            return;
        }

        PlayState(to);
    }
    
    private void PlayState(PlayerStateInfo stateInfo) {

        // Lock animation switching for this state's duration
        _lockedUntil = Time.time + stateInfo.Duration;

        switch (stateInfo.State) {
            case PlayerState.Idle:
                animator.CrossFade(Idle, 0f);
                break;
            case PlayerState.Walk:
                animator.CrossFade(Walk, 0f);
                break;
            case PlayerState.Run:
                animator.CrossFade(Run, 0f);
                break;
            case PlayerState.Jump:
                animator.CrossFade(Jump, 0f);
                break;
            case PlayerState.Fall:
                animator.CrossFade(Fall, 0f);
                break;
            case PlayerState.Land:
                animator.CrossFade(Land, 0f);
                break;
            case PlayerState.Dash:
                animator.CrossFade(Dash, 0f);
                break;
            case PlayerState.WallSlide:
                animator.CrossFade(WallSlide, 0f);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stateInfo), stateInfo, null);
        }
    }
    
    #endregion
    
    #region Facing / Sprite Orientation
    
    /// <summary>
    /// Flips the sprite horizontally when the player turns.
    /// Direction convention:
    ///  -  1 = facing right
    ///  - -1 = facing left
    /// </summary>
    private void HandleTurn(int direction) {
        spriteRenderer.flipX = direction < 0;
    }
    
    #endregion
    
    #region Animator Parameters
    
    /// <summary>
    /// Sets the MoveAmount animator parameter to drive
    /// the speed of walking and running animations.
    /// </summary>
    private void SetMoveAmount(float amount) {
        animator.SetFloat(MoveAmount, amount);
    }
    
    #endregion
}
