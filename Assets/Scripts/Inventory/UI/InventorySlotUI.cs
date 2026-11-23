using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private InventorySlot _slot;

    [SerializeField] private Image _slotImage;
    [SerializeField] private Image _emptyImage;
    [SerializeField] private Image _frameImage;
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _quantityText;

    private Button _button;

    private bool _hasItem => _slot.Item != null;

    private void Awake()
    {
        _button = GetComponent<Button>();

        Refresh();
    }

    public void Refresh()
    {
        if (_button != null)
        {
            _button.interactable = _hasItem;
        }

        _slotImage.gameObject.SetActive(_hasItem);
        _itemImage.gameObject.SetActive(_hasItem);
        _emptyImage.gameObject.SetActive(!_hasItem);
        _quantityText.gameObject.SetActive(_hasItem && _slot.Quantity > 1);

        if (_hasItem)
        {
            _itemImage.sprite = _slot.Item.Sprite;
            _quantityText.SetText("{0:0}", _slot.Quantity);
        }
    }
}
