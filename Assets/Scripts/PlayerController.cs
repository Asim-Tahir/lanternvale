using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;

    public event Action<Vector3Int> ArrivedAction;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private InputAction _moveAction;
    private InputAction _attackAction;
    private Vector2 _movementInput;

    private const string ANIMATOR_MOVE_X_PARAM = "MoveX";
    private const string ANIMATOR_MOVE_Y_PARAM = "MoveY";
    private const string ANIMATOR_IS_WALKING_PARAM = "IsWalking";
    private const string ANIMATOR_ATTACK_TRIGGER = "Attack";

    private static readonly int AnimatorMoveXHash = Animator.StringToHash(ANIMATOR_MOVE_X_PARAM);
    private static readonly int AnimatorMoveYHash = Animator.StringToHash(ANIMATOR_MOVE_Y_PARAM);
    private static readonly int AnimatorIsWalkingHash = Animator.StringToHash(ANIMATOR_IS_WALKING_PARAM);
    private static readonly int AnimatorAttackTriggerHash = Animator.StringToHash(ANIMATOR_ATTACK_TRIGGER);

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _moveAction = new InputAction("Move", InputActionType.Value);
        _moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/rightArrow")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        _attackAction = new InputAction("Attack", InputActionType.Button);
        _attackAction.AddBinding("<Keyboard>/space");
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _attackAction.Enable();
        _attackAction.performed += OnAttackPerformed;
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _attackAction.Disable();
        _attackAction.performed -= OnAttackPerformed;
    }

    private void Start()
    {
        _navMeshAgent.enabled = false;
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;

        _animator.SetFloat(AnimatorMoveXHash, 0f);
        _animator.SetFloat(AnimatorMoveYHash, -1f);
        _animator.SetBool(AnimatorIsWalkingHash, false);
    }

    private void Update()
    {
        Vector2 direction;
        bool isWalking;

        if (_navMeshAgent.enabled)
        {
            var desiredVelocity = _navMeshAgent.desiredVelocity;
            direction = new(desiredVelocity.x, desiredVelocity.y);
            isWalking = _navMeshAgent.velocity.sqrMagnitude > 0.01f;
        }
        else
        {
            _movementInput = _moveAction.ReadValue<Vector2>();
            if (_movementInput.sqrMagnitude > 1f)
                _movementInput.Normalize();

            direction = _movementInput;
            isWalking = direction.sqrMagnitude > 0.01f;
        }

        _animator.SetBool(AnimatorIsWalkingHash, isWalking);

        if (direction.sqrMagnitude > 0.01f)
        {
            direction.Normalize();

            _animator.SetFloat(AnimatorMoveXHash, direction.x);
            _animator.SetFloat(AnimatorMoveYHash, direction.y);
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = _movementInput * _moveSpeed;
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        _animator.SetTrigger(AnimatorAttackTriggerHash);
    }

    /// <summary>
    /// Starts NavMesh movement toward the destination and raises <see cref="ArrivedAction"/>
    /// with the associated cell when the player arrives.
    /// </summary>
    /// <param name="worldDestination">The destination position in world space.</param>
    /// <param name="cellDestination">The tile cell associated with the destination.</param>
    public void AutoMoveTowards(Vector3 worldDestination, Vector3Int cellDestination)
    {
        _moveAction.Disable();
        _attackAction.Disable();

        _movementInput = Vector2.zero;
        _rigidbody.linearVelocity = Vector2.zero;
        _navMeshAgent.enabled = true;

        if (_navMeshAgent.SetDestination(worldDestination))
            StartCoroutine(WaitForArrival(cellDestination));
    }

    private IEnumerator WaitForArrival(Vector3Int cellDestination)
    {
        while (_navMeshAgent.pathPending)
            yield return null;

        if (_navMeshAgent.pathStatus != NavMeshPathStatus.PathComplete)
            yield break;

        while (_navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
            yield return null;

        ArrivedAction?.Invoke(cellDestination);

        _navMeshAgent.enabled = false;

        _moveAction.Enable();
        _attackAction.Enable();
    }
}
