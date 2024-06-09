using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : BaseComponent
{
    [Header("Data")]
    [SerializeField] private MovementDataSO _moveData;

    private Vector2 _workspace;
    private bool _canApply;
    private bool _facingRight;

    private float _jumpVelocity;

    private void Start()
    {
        _canApply = true;
        _facingRight = true;

        _jumpVelocity = ((2f * _moveData.JumpHeight) / Mathf.Pow(_moveData.TimeUntilPeak, 2f));
    }

    private void FixedUpdate()
    {
        
    }

    public void HandleMovement(Vector2 direction, bool grounded)
    {
        if (direction != Vector2.zero)
        {
            if (grounded)
                _animator.SetBool("isMoving", true);

            _workspace.x = Mathf.MoveTowards
                (_workspace.x, 
                direction.x * _moveData.MaxMoveSpeed, 
                _moveData.Acceleration * Time.fixedDeltaTime);
        }
        else
        {
            if (grounded)
                _animator.SetBool("isMoving", false);

            float targetDeceleration = grounded ? _moveData.GroundDeceleration : _moveData.AirDeceleration;

            _workspace.x = Mathf.MoveTowards
                (_workspace.x,
                0f,
                targetDeceleration * Time.fixedDeltaTime);
        }

        CheckFlip(direction);
        ApplyVelocity();
    }

    public void HandleJump()
    {
        _animator.SetBool("isJumping", true);
        _workspace.y = _jumpVelocity;
    }

    public void HandleCeilingHit()
    {
        _workspace.y = Mathf.Min(0, _workspace.y);

        ApplyVelocity();
    }

    public void HandleFall(bool grounded, bool earlyJumpEnd)
    {
        if (grounded && _workspace.y <= 0f)
        {
            _workspace.y = _moveData.GroundedForce;
        }
        else
        {
            if (!grounded && _body.velocity.y < (_moveData.GroundedForce * 1.1f))
            {
                _animator.SetBool("isJumping", false);
                _animator.SetBool("hasLanded", false);
                _animator.SetBool("isFalling", true);
            }


            float fallSpeed = _moveData.FallAcceleration;

            if (earlyJumpEnd && _workspace.y > 0) 
                fallSpeed *= _moveData.EarlyJumpMultiplier;

            _workspace.y = Mathf.MoveTowards(_workspace.y, _moveData.MaxFallSpeed, fallSpeed * Time.fixedDeltaTime);

            if (_workspace.y < _moveData.MaxFallSpeed)
                _workspace.y = _moveData.MaxFallSpeed;
        }

        ApplyVelocity();
    }

    public void HandleLand()
    {
        _animator.SetBool("hasLanded", true);
        _animator.SetBool("isFalling", false);
    }

    public void HandleDeath()
    {
        //_animator.SetBool("isDead", true);
        _animator.Play("Death");
    }

    private void ApplyVelocity()
    {
        if (!_canApply) return;

        _body.velocity = _workspace;
    }

    private void CheckFlip(Vector2 direction)
    {
        if ((_facingRight && direction == Vector2.left) || (!_facingRight && direction == Vector2.right))
            HandleFlip();
    }

    private void HandleFlip()
    {
        _facingRight = !_facingRight;

        transform.Rotate(0f, 180f, 0f);
    }
}
