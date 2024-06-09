using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Components
    private Rigidbody2D _body;
    private Animator _animator;
    private Movement _movement;
    private CollisionSensor _sensor;
    #endregion

    #region Input
    private InputHandler _input;

    private Vector2 _movementInput;
    private bool _jumpPress;
    private bool _jumpRelease;
    #endregion

    #region Jump Variables

    [SerializeField] private float _jumpBufferTime = 0.2f;
    [SerializeField] private float _coyoteTime = 0.15f;
    private float _timeLeftGround;

    private bool _bufferedJump;
    private bool _coyoteJump;
    private bool _earlyJumpEnd;

    private bool BufferedJumpAvailable => _bufferedJump && Time.time < _input.JumpStart + _jumpBufferTime;
    private bool CoyoteJumpAvailable => _coyoteJump && !_grounded && Time.time < _timeLeftGround + _coyoteTime;
    #endregion

    private bool _grounded;
    [SerializeField] private LayerMask _interactMask;
    [SerializeField] private StatsDataSO _stats;

    private List<IInteract> _interactables = new List<IInteract>();

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _input = GetComponent<InputHandler>();
        _movement = GetComponent<Movement>();
        _sensor = GetComponent<CollisionSensor>();
    }

    private void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => CheckInteractable())
            .Subscribe(_ => CollectInteractable());

        this.UpdateAsObservable()
            .Where(_ => _stats.PlayerDead == true)
            .Subscribe(_ => _input.DisableControls());

        this.UpdateAsObservable()
            .Where(_ => _stats.PlayerDead == true)
            .Subscribe(_ => _movement.HandleDeath());
    }

    private void Update()
    {
        _movementInput = _input.Movement;
        _jumpPress = _input.JumpPress;
        _jumpRelease = _input.JumpRelease;
    }

    private void FixedUpdate()
    {
        CheckCollisions();

        CheckJump();

        _movement.HandleMovement(_movementInput, _grounded);
        _movement.HandleFall(_grounded, _earlyJumpEnd);
    }

    private void CheckJump()
    {
        if (!_earlyJumpEnd && !_grounded && _input.JumpDuration < 0.4f && _jumpRelease && _body.velocity.y > 0f)
            _earlyJumpEnd = true;

        if ((!_jumpPress || !BufferedJumpAvailable)) return;

        if ((_grounded || CoyoteJumpAvailable))
        {
            _coyoteJump = false;
            _bufferedJump = false;
            _earlyJumpEnd = false;

            _movement.HandleJump();
        }
    }

    private void CheckCollisions()
    {
        bool groundHit = _sensor.CheckGround();
        bool ceilingHit = _sensor.CheckCeiling();

        if (ceilingHit)
        {
            _movement.HandleCeilingHit();
        }

        if (!_grounded && groundHit)
        {
            _grounded = true;
            _coyoteJump = true;
            _bufferedJump = true;
            _earlyJumpEnd = false;

            _movement.HandleLand();
        }

        else if (_grounded && !groundHit)
        {
            _grounded = false;
            _timeLeftGround = Time.time;
        }
    }

    private bool CheckInteractable()
    {
        return _input.Interact && _interactables != null;
    }

    private void CollectInteractable()
    {
        if (_interactables == null) return;

        foreach (IInteract interactable in _interactables)
        {
            interactable.OnInteract();

            if (_interactables.Count <= 0) break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((_interactMask.value & (1 << collision.gameObject.layer)) > 0)
        {
            IInteract interactObject = collision.gameObject.GetComponent<IInteract>();
            
            if (interactObject != null && !_interactables.Contains(interactObject))
                _interactables.Add(interactObject);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((_interactMask.value & (1 << collision.gameObject.layer)) > 0)
        {
            IInteract interactObject = collision.gameObject.GetComponent<IInteract>();

            if (interactObject != null)
                _interactables.Remove(interactObject);
        }
    }
}
