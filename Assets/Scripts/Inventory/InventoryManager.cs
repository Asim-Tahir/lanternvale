using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour, InputSystem_Actions.IInventoryActions
{
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private UIDialogController _inventoryDialog;
    [SerializeField] private GameObject _hotbar;

    public static InventoryManager Instance { get; private set; }
    private readonly InventoryPrefManager _prefManager = InventoryPrefManager.GetInstance();
    private InputSystem_Actions _actions;
    private InputSystem_Actions.InventoryActions _inventoryActions;

    /// <summary>
    /// Number of inventory columns.
    /// </summary>
    [SerializeField] private int _slotColumnCount = 5;
    /// <summary>
    /// Number of inventory rows above the hotbar.
    /// </summary>
    [SerializeField] private int _slotRowCount = 3;

    /// <summary>
    /// Fixed number of rows at the bottom of the slot grid reserved for the hotbar.
    /// </summary>
    private const int _hotbarSlotRowCount = 1;

    /// <summary>
    /// Total row count: inventory rows plus hotbar rows.
    /// </summary>
    private int _slotRowCountPlusHotbar => _slotRowCount + _hotbarSlotRowCount;

    /// <summary>
    /// Contains <see cref="_slotRowCount"/> inventory rows at the top and
    /// <see cref="_hotbarSlotRowCount"/> hotbar rows at the bottom; for example,
    /// values of 3 inventory rows and 1 hotbar row create 4 total rows.
    /// </summary>
    private InventorySlot[,] _slots;
    private InventorySlotUI[] _slotUIs;

    public InventorySlot ActiveSlot { get; private set; }

    public event Action InventoryChanged;
    public event Action<bool> IsDialogOpenChanged;

    public bool IsOpen => _inventoryDialog != null && _inventoryDialog.IsOpen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (_actions == null)
        {
            _actions = new InputSystem_Actions();
            _inventoryActions = _actions.Inventory;
            _inventoryActions.AddCallbacks(this);
        }

        _slots = new InventorySlot[_slotRowCountPlusHotbar, _slotColumnCount];

        _slotUIs = _inventoryPanel.GetComponentsInChildren<InventorySlotUI>();

        InventoryChanged += OnInventoryChanged;
        OnInventoryChanged();

        if (_inventoryDialog != null)
        {
            _inventoryDialog.IsOpenChanged += OnInventoryDialogOpenChanged;
        }
    }

    private void OnDestroy()
    {
        InventoryChanged -= OnInventoryChanged;

        if (_inventoryDialog != null)
        {
            _inventoryDialog.IsOpenChanged -= OnInventoryDialogOpenChanged;
        }

        _actions?.Dispose();
    }

    private void OnInventoryChanged()
    {
        Debug.Log("OnInventoryChanged player inventory");
        foreach (var slotUI in _slotUIs)
        {
            slotUI.Refresh();
        }
    }

    private void OnInventoryDialogOpenChanged(bool isOpen)
    {
        if (_actions != null)
        {
            if (isOpen)
            {
                _inventoryActions.Enable();
            }
            else
            {
                _inventoryActions.Disable();
            }
        }

        IsDialogOpenChanged?.Invoke(isOpen);
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        // throw new NotImplementedException();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (_inventoryDialog != null)
        {
            _inventoryDialog.SafeClose();
        }
    }

    public void OnSelectSlot1(InputAction.CallbackContext context)
    {
        // throw new NotImplementedException();
    }

    public void OnSelectSlot2(InputAction.CallbackContext context)
    {
        // throw new NotImplementedException();
    }

    public void OnSelectSlot3(InputAction.CallbackContext context)
    {
        // throw new NotImplementedException();
    }

    public void OnSelectSlot4(InputAction.CallbackContext context)
    {
        // throw new NotImplementedException();
    }

    public void OnSelectSlot5(InputAction.CallbackContext context)
    {
        // throw new NotImplementedException();
    }

    public bool AddItem(Item item, int quantity = 1)
    {
        bool isInventoryChanged = false;

        // Try stacking onto existing slots first
        if (quantity > 0 && item.IsStackable)
        {
            for (int rowIndex = 0; rowIndex < _slotRowCountPlusHotbar; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < _slotColumnCount; columnIndex++)
                {
                    var slot = _slots[rowIndex, columnIndex];

                    if (slot.Item == item && slot.Quantity < item.StackCapacity)
                    {
                        int space = item.StackCapacity - slot.Quantity;
                        int toAdd = Mathf.Min(quantity, space);
                        _slots[rowIndex, columnIndex].Quantity += toAdd;
                        quantity -= toAdd;
                        isInventoryChanged |= toAdd > 0;

                        if (quantity <= 0)
                        {
                            if (isInventoryChanged)
                            {
                                _prefManager.ReplaceSaved(_slots);
                            }

                            InventoryChanged?.Invoke();
                            return true;
                        }
                    }
                }
            }
        }

        // Place remainder in empty slots
        for (int rowIndex = 0; rowIndex < _slotRowCountPlusHotbar; rowIndex++)
        {
            for (int columnIndex = 0; columnIndex < _slotColumnCount; columnIndex++)
            {
                var slot = _slots[rowIndex, columnIndex];

                if (quantity > 0 && slot.Item == null)
                {
                    int toAdd = item.IsStackable ? Math.Min(quantity, item.StackCapacity) : 1;
                    _slots[rowIndex, columnIndex] = new(new Vector2Int(rowIndex, columnIndex), item, toAdd);
                    quantity -= toAdd;
                    isInventoryChanged |= toAdd > 0;
                    if (quantity <= 0)
                    {
                        _prefManager.ReplaceSaved(_slots);

                        InventoryChanged?.Invoke();
                        return true;
                    }
                }
            }
        }

        if (isInventoryChanged)
        {
            _prefManager.ReplaceSaved(_slots);
        }

        InventoryChanged?.Invoke();
        return quantity <= 0; // false = inventory full, some items not added
    }

    public void RemoveItemAt(Vector2Int slotPosition, int quantity = 1)
    {
        if (
            slotPosition.x < 0
            || slotPosition.x >= _slotRowCountPlusHotbar
            || slotPosition.y < 0
            || slotPosition.y >= _slotColumnCount
        ) return;

        var slot = _slots[slotPosition.x, slotPosition.y];
        if (slot.Item != null && quantity > 0)
        {
            slot.Quantity -= quantity;
            if (slot.Quantity <= 0)
            {
                slot = default;
            }

            _slots[slotPosition.x, slotPosition.y] = slot;
            _prefManager.ReplaceSaved(_slots);
        }

        InventoryChanged?.Invoke();
    }

    public void SwapSlots(Vector2Int slotPositionA, Vector2Int slotPositionB)
    {
        if (
            slotPositionA == slotPositionB
            || slotPositionA.x < 0
            || slotPositionA.x >= _slotRowCountPlusHotbar
            || slotPositionA.y < 0
            || slotPositionA.y >= _slotColumnCount
            || slotPositionB.x < 0
            || slotPositionB.x >= _slotRowCountPlusHotbar
            || slotPositionB.y < 0
            || slotPositionB.y >= _slotColumnCount

        )
            return;

        (_slots[slotPositionA.x, slotPositionA.y], _slots[slotPositionB.x, slotPositionB.y]) = (_slots[slotPositionB.x, slotPositionB.y], _slots[slotPositionA.x, slotPositionA.y]);

        var slotA = _slots[slotPositionA.x, slotPositionA.y];
        slotA.Position = slotPositionA;
        _slots[slotPositionA.x, slotPositionA.y] = slotA;
        var slotB = _slots[slotPositionB.x, slotPositionB.y];
        slotB.Position = slotPositionB;
        _slots[slotPositionB.x, slotPositionB.y] = slotB;

        _prefManager.ReplaceSaved(_slots);

        InventoryChanged?.Invoke();
    }

    public void Activate(Vector2Int slotPosition)
    {
        if (
            slotPosition.x < 0
            || slotPosition.x >= _slotRowCountPlusHotbar
            || slotPosition.y < 0
            || slotPosition.y >= _slotColumnCount
        ) return;

        ActiveSlot = _slots[slotPosition.x, slotPosition.y];

        InventoryChanged?.Invoke();
    }

    public void Open()
    {
        if (_inventoryDialog != null)
        {
            _inventoryDialog.Open();
        }
    }

    public void SafeOpen()
    {
        if (_inventoryDialog != null)
        {
            _inventoryDialog.SafeOpen();
        }
    }

    public void Close()
    {
        if (_inventoryDialog != null)
        {
            _inventoryDialog.Close();
        }
    }

    public void SafeClose()
    {
        if (_inventoryDialog != null)
        {
            _inventoryDialog.SafeClose();
        }
    }

    public void Toggle()
    {
        if (_inventoryDialog != null)
        {
            _inventoryDialog.Toggle();
        }
    }
}
