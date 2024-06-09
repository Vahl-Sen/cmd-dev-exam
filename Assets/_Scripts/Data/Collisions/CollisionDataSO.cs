using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Collision Data", menuName = "Data/Collision")]
public class CollisionDataSO : ScriptableObject
{
    [Header("Collision")]
    public LayerMask CollisionLayer;
    public float GrounderDistance = 0.05f;
}
