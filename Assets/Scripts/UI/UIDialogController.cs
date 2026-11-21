using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIDialogController : MonoBehaviour, InputSystem_Actions.IDialogActions
{
    [SerializeField] private bool _isDialogOpen = false;
    public bool IsDialogOpen => _isDialogOpen;
    [SerializeField] private List<Button> _dialogActionButtons;

    private InputSystem_Actions _actions;
    private InputSystem_Actions.DialogActions _dialogActions;

    private void Awake()
    {
        InitializeActions();
    }

    private void InitializeActions()
    {
        if (_actions != null)
        {
            return;
        }

        _actions = new InputSystem_Actions();
        _dialogActions = _actions.Dialog;
        _dialogActions.AddCallbacks(this);
    }

    private void Start()
    {
        foreach (var dialogActionButton in _dialogActionButtons)
        {
            dialogActionButton.onClick.AddListener(OnDialogActionClicked);
        }
    }

    private void OnDestroy()
    {
        if (_actions != null)
        {
            _actions.Dispose();
        }

        foreach (var dialogActionButton in _dialogActionButtons)
        {
            dialogActionButton.onClick.RemoveListener(OnDialogActionClicked);
        }
    }

    private void OnEnable()
    {
        if (_actions != null)
        {
            _dialogActions.Enable();
        }
    }

    private void OnDisable()
    {
        if (_actions != null)
        {
            _dialogActions.Disable();
        }
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        SafeCloseDialog();
    }

    private void OnDialogActionClicked()
    {
    }

    public void OpenDialog()
    {
        _isDialogOpen = true;
        gameObject.SetActive(true);
    }

    public void SafeOpenDialog()
    {
        if (!IsDialogOpen)
        {
            OpenDialog();
        }
    }

    public void CloseDialog()
    {
        _isDialogOpen = false;
        gameObject.SetActive(false);
    }

    public void SafeCloseDialog()
    {
        if (IsDialogOpen)
        {
            CloseDialog();
        }
    }

    public void ToggleDialog()
    {
        if (IsDialogOpen)
        {
            CloseDialog();
        }
        else
        {
            OpenDialog();
        }
    }
}
