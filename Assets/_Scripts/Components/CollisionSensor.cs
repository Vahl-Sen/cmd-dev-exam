using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSensor : BaseComponent
{
    [Header("Data")]
    [SerializeField] private CollisionDataSO _collisionData;

    public bool CheckGround()
    {
        bool groundHit = Physics2D.CapsuleCast
            (_collider.bounds.center,
            _collider.size,
            _collider.direction,
            0f,
            Vector2.down,
            _collisionData.GrounderDistance,
            _collisionData.CollisionLayer);

        return groundHit;
    }

    public bool CheckCeiling()
    {
        bool ceilingHit = Physics2D.CapsuleCast
            (_collider.bounds.center,
            _collider.size,
            _collider.direction,
            0f, 
            Vector2.up,
            _collisionData.GrounderDistance,
            _collisionData.CollisionLayer);

        return ceilingHit;
    }
}
