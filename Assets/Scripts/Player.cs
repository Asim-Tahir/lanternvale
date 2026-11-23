using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NavMeshAgent))]
public class Player : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [SerializeField] private float _moveSpeed = 5f;

    public event Action<Vector3Int> ArrivedAction;

    private InventoryManager _inventoryManager => InventoryManager.Instance;
    private GameManager _gameManager => GameManager.Instance;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private InputSystem_Actions _actions;
    private InputSystem_Actions.PlayerActions _playerActions;
    private Vector2 _movementInput;
    private bool IsInventoryOpen => _inventoryManager != null && _inventoryManager.IsOpen;

    private const string ANIMATOR_MOVE_X_PARAM = "MoveX";
    private const string ANIMATOR_MOVE_Y_PARAM = "MoveY";
    private const string ANIMATOR_IS_WALKING_PARAM = "IsWalking";
    private const string ANIMATOR_IS_WATERING_PARAM = "IsWatering";
    private const string ANIMATOR_USE_TRIGGER = "Use";

    private static readonly int AnimatorMoveXHash = Animator.StringToHash(ANIMATOR_MOVE_X_PARAM);
    private static readonly int AnimatorMoveYHash = Animator.StringToHash(ANIMATOR_MOVE_Y_PARAM);
    private static readonly int AnimatorIsWalkingHash = Animator.StringToHash(ANIMATOR_IS_WALKING_PARAM);
    private static readonly int AnimatorIsWateringHash = Animator.StringToHash(ANIMATOR_IS_WATERING_PARAM);
    private static readonly int AnimatorUseTriggerHash = Animator.StringToHash(ANIMATOR_USE_TRIGGER);

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _actions = new InputSystem_Actions();
        _playerActions = _actions.Player;
        _playerActions.AddCallbacks(this);
    }

    private void OnEnable()
    {
        Enable();

        if (_inventoryManager != null)
        {
            _inventoryManager.IsDialogOpenChanged += OnInventoryDialogOpenChanged;
            OnInventoryDialogOpenChanged(_inventoryManager.IsOpen);
        }
    }

    private void OnDisable()
    {
        Disable();

        if (_inventoryManager != null)
        {
            _inventoryManager.IsDialogOpenChanged -= OnInventoryDialogOpenChanged;
        }
    }

    private void OnInventoryDialogOpenChanged(bool isOpen)
    {
        if (isOpen)
        {
            _movementInput = Vector2.zero;
            _rigidbody.linearVelocity = Vector2.zero;

            Disable();
        }
        else
        {
            Enable();
        }
    }

    private void OnDestroy()
    {
        _actions?.Dispose();
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
        _rigidbody.linearVelocity = IsInventoryOpen ? Vector2.zero : _movementInput * _moveSpeed;
    }

    /// <summary>
    /// Disable player inputs
    /// </summary>
    public void Disable()
    {
        if (_actions != null)
        {
            _playerActions.Disable();
        }
    }

    /// <summary>
    /// Enable player inputs
    /// </summary>
    public void Enable()
    {
        if (_actions != null)
        {
            _playerActions.Enable();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (_gameManager != null)
        {
            _gameManager.TogglePause();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    public void OnUse(InputAction.CallbackContext context)
    {
        if (context.performed && !IsInventoryOpen)
        {
            _animator.SetTrigger(AnimatorUseTriggerHash);
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (!context.performed || _inventoryManager == null)
        {
            return;
        }

        _inventoryManager.Toggle();
    }

    /// <summary>
    /// Starts NavMesh movement toward the destination and raises <see cref="ArrivedAction"/>
    /// with the associated cell when the player arrives.
    /// </summary>
    /// <param name="worldDestination">The destination position in world space.</param>
    /// <param name="cellDestination">The tile cell associated with the destination.</param>
    public void AutoMoveTowards(Vector3 worldDestination, Vector3Int cellDestination)
    {
        Disable();

        _movementInput = Vector2.zero;
        _rigidbody.linearVelocity = Vector2.zero;
        _navMeshAgent.enabled = true;

        if (_navMeshAgent.SetDestination(worldDestination))
        {
            StartCoroutine(WaitForArrival(cellDestination));
        }
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

        Enable();
    }
}
