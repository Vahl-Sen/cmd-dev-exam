using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Movement Data", menuName = "Data/Movement")]
public class MovementDataSO : ScriptableObject
{
    [Header("Collision")]
    public float CheckDistance = 0.05f;
    public float GroundedForce = -0.5f;

    [Header("Movement")]
    public float MaxMoveSpeed = 12f;
    public float Acceleration = 90f;
    public float GroundDeceleration = 60f;

    [Header("Air")]
    public float MaxFallSpeed = -40f;
    public float FallAcceleration = 80f;
    public float AirDeceleration = 30f;
    public float EarlyJumpMultiplier = 2.5f;

    [Header("Jump")]
    public float JumpHeight = 2.5f;
    public float TimeUntilPeak = 0.75f;
    public float TimeUntilLand = 0.55f;
}
