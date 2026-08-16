using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(
    fileName = "PlatformerMovementStats", 
    menuName = "Player Movement/PlatformerMovementStats"
)]
public class PlatformerMovementStats : ScriptableObject {
    
    [Header("Collision & Layers")]
    
    [Tooltip("Set this to the layer your player is on")]
    public LayerMask playerLayer;
    
    
    [Header("Input")] 
    
    [Tooltip(
        "If enabled, movement input snaps to -1 / 0 / 1.\n" +
        "Prevents slow movement when using analog sticks."
    )]
    public bool snapInput = true;
    
    [Tooltip(
        "Minimum horizontal input magnitude required to register left or right.\n" +
        "Helps prevent drifting from worn or sensitive controllers."
    )]
    [Range(0.01f, 0.99f)] 
    public float horizontalDeadZoneThreshold = 0.1f;
    
    [Tooltip(
        "Minimum vertical input magnitude required to register up or down."
    )]
    [Range(0.01f, 0.99f)] 
    public float verticalDeadZoneThreshold = 0.3f;
    
    [Tooltip(
        "Input magnitude required to be considered 'running' instead of walking.\n" +
        "Only relevant when using analog input (snap input is off)."
    )]
    [Range(0.01f, 0.99f)]
    public float runStickThreshold = 0.8f;
    
    
    [Header("Abilities")]
    
    public bool allowDashing = true;
    public bool allowWallSliding = true;
    
    
    [Header("Horizontal Movement")]
    
    [Tooltip("Maximum horizontal speed when walking.")]
    [Range(1f, 100f)] 
    public float walkMaxSpeed = 14f;
    
    [Tooltip("Maximum horizontal speed when running.")]
    [Range(1f, 100f)] 
    public float runMaxSpeed = 20f;
    
    [Tooltip(
        "Rate at which horizontal speed increases toward the target speed."
    )]
    public float acceleration = 120f;
    
    [Tooltip(
        "How quickly horizontal velocity slows down when grounded and no input is provided."
    )]
    public float groundDeceleration = 60f;
    
    [Tooltip(
        "How quickly horizontal velocity slows down while airborne."
    )]
    public float airDeceleration = 60f;

    [Tooltip(
        "Minimum speed locomotion animations can play at."
    )]
    public float minMoveAmount = 0.35f;
    
    
    [Header("Jump")]
    
    [Tooltip(
        "Upward velocity applied when performing a jump."
    )]
    public float jumpPower = 36f;
    
    [Tooltip(
        "Multiplier applied to jump power for mid-air jumps.\n" +
        "Values below 1 make air jumps feel weaker."
    )]
    [Range(0.01f, 1f)]
    public float airJumpPowerMultiplier = 0.9f;
    
    [Tooltip(
        "Total number of jumps allowed before landing.\n" +
        "Includes the ground jump (2 = double jump)."
    )]
    [Min(1)]
    public int numberOfJumps = 2;
    
    [Tooltip(
        "Time window after leaving a ledge during which a jump is still allowed.\n" +
        "Improves forgiveness and game feel."
    )]
    public float coyoteTime = 0.15f;
    
    [Tooltip(
        "Time window during which a jump input is stored and executed automatically\n" +
        "when landing."
    )]
    public float jumpBuffer = .2f;
    
    
    [Header("Gravity & Falling")]
    
    [Tooltip(
        "Constant downward force applied while grounded.\n" +
        "Helps the character stick to slopes and uneven terrain."
    )]
    [Range(-10f, 0f)]
    public float groundingForce = -1.5f;

    [Tooltip(
        "Acceleration applied downward while airborne."
    )]
    public float fallAcceleration = 110f;
    
    [Tooltip(
        "Maximum downward velocity while falling."
    )]
    public float maxFallSpeed = 40f;

    [Tooltip(
        "Gravity multiplier applied when the jump button is released early.\n" +
        "Higher values create sharper, more responsive short hops."
    )]
    public float jumpCutGravityModifier = 3f;

    [Tooltip(
        "Minimum landing velocity for the landing state to trigger."
    )]
    public float minLandVelocity = 35f;
    
    [Tooltip(
        "Duration of the landing state."
    )]
    public float landDuration = 0.15f;
    
    [Header("Apex Hang")]
    
    [Tooltip(
        "Vertical velocity threshold at which the character is considered\n" +
        "to be at the apex of a jump."
    )]
    [Range(0.5f, 2f)]
    public float apexThreshold = 1f;
    
    [Tooltip(
        "Gravity multiplier applied while near the jump apex.\n" +
        "Lower values create a floatier hang time."
    )]
    [Range(0.01f, 1f)]
    public float apexGravityMultiplier = 0.4f;
    
    [Header("Ground Detection")]
    
    [Tooltip(
        "Distance used for ground and ceiling detection capsule casts."
    )]
    [Range(0f, 0.5f)]
    public float grounderDistance = 0.05f;
    
    
    [Header("Wall Jump")]

    [Tooltip("Horizontal force applied when wall jumping.")]
    public float wallJumpHorizontalPower = 20f;

    [Tooltip("Vertical force applied when wall jumping.")]
    public float wallJumpVerticalPower = 32f;

    [Tooltip("Distance used for wall detection casts.")]
    [Range(0f, 0.5f)]
    public float wallCheckDistance = 0.05f;
    
    [Tooltip("Time window after wall jumping where the direction the player\n" +
             "is facing is locked.")]
    [Range(0f, 2f)]
    public float wallJumpFacingLockTime = 0.15f;
        
    
    [Header("Wall Slide")]

    [Tooltip("Maximum downward speed while sliding on a wall.")]
    public float wallSlideMaxSpeed = 8f;

    [Tooltip("Gravity multiplier while wall sliding.\nLower values feel stickier.")]
    [Range(0.01f, 1f)]
    public float wallSlideGravityMultiplier = 0.5f;
    
    
    [Header("Dash")]

    [Tooltip("Speed applied during a dash.")]
    public float dashSpeed = 40f;

    [Tooltip("Duration of the dash in seconds.")]
    public float dashDuration = 0.15f;

    [Tooltip("Number of dashes available before landing.")]
    [Min(0)]
    public int maxDashes = 1;
}