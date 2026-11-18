using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;

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
        _animator.SetFloat(AnimatorMoveXHash, 0f);
        _animator.SetFloat(AnimatorMoveYHash, -1f);
        _animator.SetBool(AnimatorIsWalkingHash, false);
    }

    private void Update()
    {
        _movementInput = _moveAction.ReadValue<Vector2>();

        if (_movementInput.sqrMagnitude > 1f)
            _movementInput.Normalize();

        bool isWalking = _movementInput.sqrMagnitude > 0.01f;
        _animator.SetBool(AnimatorIsWalkingHash, isWalking);

        if (isWalking)
        {
            _animator.SetFloat(AnimatorMoveXHash, _movementInput.x);
            _animator.SetFloat(AnimatorMoveYHash, _movementInput.y);
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
}
