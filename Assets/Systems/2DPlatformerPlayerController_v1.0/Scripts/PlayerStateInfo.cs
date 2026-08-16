using System;
using UnityEngine;

/// <summary>
/// Represents a player state along with how long
/// the player has been in that state.
/// </summary>
[Serializable]
public class PlayerStateInfo {

    /// <summary>
    /// The current player state.
    /// </summary>
    public PlayerState State { get; private set; }

    /// <summary>
    /// Time (in seconds) spent in this state.
    /// </summary>
    public float Duration { get; private set; }

    /// <summary>
    /// Creates a new player state info instance.
    /// </summary>
    public PlayerStateInfo(PlayerState state, float duration = 0f) {
        State = state;
        Duration = duration;
    }
}