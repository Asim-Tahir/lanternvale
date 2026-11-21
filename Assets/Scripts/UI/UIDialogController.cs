using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIDialogController : MonoBehaviour, InputSystem_Actions.IDialogActions
{
    [SerializeField] private bool _isDialogOpen = false;
    public bool IsDialogOpen => _isDialogOpen;
    [SerializeField] private Button _dialogCloseButton;
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
        _dialogCloseButton.onClick.AddListener(OnDialogCloseClicked);

        foreach (var dialogActionButton in _dialogActionButtons)
        {
            dialogActionButton.onClick.AddListener(OnDialogActionClicked);
        }
    }

    private void OnDestroy()
    {
        _actions.Dispose();

        _dialogCloseButton.onClick.RemoveListener(OnDialogCloseClicked);

        foreach (var dialogActionButton in _dialogActionButtons)
        {
            dialogActionButton.onClick.RemoveListener(OnDialogActionClicked);
        }
    }

    private void OnEnable()
    {
        _dialogActions.Enable();
    }

    private void OnDisable()
    {
        _dialogActions.Disable();
    }

    public void OnCloseDialog(InputAction.CallbackContext context)
    {
        SafeCloseDialog();
    }

    private void OnDialogActionClicked()
    {
    }

    private void OnDialogCloseClicked()
    {
        SafeCloseDialog();
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
