using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIDialogController : MonoBehaviour, InputSystem_Actions.IDialogActions
{
    [SerializeField] private bool _isOpen = false;
    public bool IsOpen => _isOpen;
    public event Action<bool> IsOpenChanged;
    [SerializeField] private List<Button> _dialogActionButtons;

    private InputSystem_Actions _actions;
    private InputSystem_Actions.DialogActions _dialogActions;

    private void Awake()
    {
        _actions = new InputSystem_Actions();
        _dialogActions = _actions.Dialog;
        _dialogActions.AddCallbacks(this);
    }

    private void Start()
    {
        foreach (var dialogActionButton in _dialogActionButtons)
        {
            dialogActionButton.onClick.AddListener(OnActionClicked);
        }
    }

    private void OnDestroy()
    {
        _actions?.Dispose();

        foreach (var dialogActionButton in _dialogActionButtons)
        {
            dialogActionButton.onClick.RemoveListener(OnActionClicked);
        }
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        SafeClose();
    }

    private void OnActionClicked()
    {
    }

    public void Open()
    {
        bool wasOpen = _isOpen;
        _isOpen = true;
        gameObject.SetActive(true);

        if (!wasOpen)
        {
            IsOpenChanged?.Invoke(_isOpen);
        }

        if (_actions != null)
        {
            _dialogActions.Enable();
        }
    }

    public void SafeOpen()
    {
        if (!IsOpen)
        {
            Open();
        }
    }

    public void Close()
    {
        bool wasOpen = _isOpen;
        _isOpen = false;
        gameObject.SetActive(false);

        if (wasOpen)
        {
            IsOpenChanged?.Invoke(_isOpen);
        }

        if (_actions != null)
        {
            _dialogActions.Disable();
        }
    }

    public void SafeClose()
    {
        if (IsOpen)
        {
            Close();
        }
    }

    public void Toggle()
    {
        if (IsOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
}
