using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "TopDownMovementStats", menuName = "Player Movement/TopDownMovementStats")]
public class TopDownMovementStats : ScriptableObject {
    
    [Header("Input")] 
    [Tooltip("Makes all Input snap to an integer. Prevents gamepads from walking slowly.")]
    public bool snapInput = true;
    
    [Tooltip("Minimum input required before a left or right is recognized. Avoids drifting with sticky controllers")]
    [Range(0.01f, 0.99f)] 
    public float deadZoneThreshold = 0.1f;
    
    [Tooltip("Minimum movement input magnitude required to trigger running.")]
    [Range(0.01f, 0.99f)] 
    public float runStickThreshold = 0.8f;
    
    [Header("Walk")]
    [Tooltip("Maximum movement speed while walking.")]
    [Range(1f, 100f)] 
    public float walkMaxSpeed = 12.5f;
    
    [Tooltip("How quickly the character accelerates toward target speed.")]
    [Range(0.25f, 50f)] 
    public float acceleration = 5f;
    
    [Tooltip("How quickly the character slows down when movement input stops.")]
    [Range(0.25f, 50f)] 
    public float deceleration = 20f;
    
    [Header("Run")]
    [Tooltip("Maximum movement speed while running.")]
    [Range(1f, 100f)] 
    public float runMaxSpeed = 20f;
}