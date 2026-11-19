using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlaceableTileManager : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;

#nullable enable annotations
    [SerializeField] private Animator? _playerAnimator;
    [SerializeField] private AudioClip? _placeSound;
#nullable disable annotations
    [SerializeField] private Tile[] _tiles;

    private Tile _selectedTile;
    private SpriteRenderer _ghostPreviewRenderer;
    private Camera _camera;
    private InputAction _clickAction;
    private InputAction _cancelAction;

    private const string GHOST_PREVIEW_RENDERER_NAME = "PlacePreview_Ghost";
    private const string GHOST_PREVIEW_SORTING_LAYER_NAME = "Placeable";

    private const string ANIMATOR_PLACE_TRIGGER = "Place";
    private static readonly int AnimatorPlaceTriggerHash = Animator.StringToHash(ANIMATOR_PLACE_TRIGGER);

    private void Awake()
    {
        _camera = Camera.main;

        GameObject ghostPreviewObj = new(GHOST_PREVIEW_RENDERER_NAME);
        _ghostPreviewRenderer = ghostPreviewObj.AddComponent<SpriteRenderer>();
        _ghostPreviewRenderer.sortingLayerName = GHOST_PREVIEW_SORTING_LAYER_NAME;
        _ghostPreviewRenderer.sortingOrder = 999;
        ghostPreviewObj.SetActive(false);

        _clickAction = new InputAction("Click", binding: "<Mouse>/leftButton");
        _clickAction.AddBinding("<Touchscreen>/primaryTouch/tap");

        _cancelAction = new InputAction("Cancel", binding: "<Keyboard>/escape");
        _cancelAction.AddBinding("<Keyboard>/rightButton");
    }

    private void OnEnable()
    {
        _clickAction.Enable();
        _clickAction.performed += OnClickPerformed;

        _cancelAction.Enable();
        _cancelAction.performed += OnCancelPerformed;
    }

    private void OnDisable()
    {
        _clickAction.performed -= OnClickPerformed;
        _clickAction.Disable();

        _cancelAction.performed -= OnCancelPerformed;
        _cancelAction.Disable();
    }

    private void Update()
    {
        if (_selectedTile != null && _tilemap != null)
        {
            if (IsPointerOverUI())
            {
                _ghostPreviewRenderer.gameObject.SetActive(false);
                return;
            }

            _ghostPreviewRenderer.gameObject.SetActive(true);
            Vector3Int cellPosition = GetMouseCellPosition();
            _ghostPreviewRenderer.transform.position = _tilemap.GetCellCenterWorld(cellPosition);

            bool isOccupied = _tilemap.HasTile(cellPosition);
            _ghostPreviewRenderer.color = isOccupied
                ? new Color(1f, 0.2f, 0.2f, 0.5f)
                : new Color(0.2f, 1f, 0.2f, 0.5f);
        }
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        TryPlaceTile();
    }

    private void OnCancelPerformed(InputAction.CallbackContext context)
    {
        ClearSelectionTile();
    }

    public bool IsPointerOverUI()
    {
        if (EventSystem.current == null || Pointer.current == null)
            return false;

        PointerEventData eventData = new(EventSystem.current)
        {
            position = Pointer.current.position.ReadValue(),
        };

        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }

    public Vector3Int GetMouseCellPosition()
    {
        Vector2 screenPosition = Pointer.current != null
            ? Pointer.current.position.ReadValue()
            : Vector2.zero;
        Vector3 worldPosition = _camera.ScreenToWorldPoint(
            new Vector3(
                screenPosition.x,
                screenPosition.y,
                Mathf.Abs(_camera.transform.position.z)
            )
        );

        return _tilemap.WorldToCell(worldPosition);
    }

    public void SelectTile(int index)
    {
        if (index >= 0 && index < _tiles.Length)
        {
            _selectedTile = _tiles[index];
            _ghostPreviewRenderer.sprite = _selectedTile.sprite;
            _ghostPreviewRenderer.gameObject.SetActive(true);
        }
    }

    public void ClearSelectionTile()
    {
        _selectedTile = null;

        if (_ghostPreviewRenderer != null)
        {
            _ghostPreviewRenderer.gameObject.SetActive(false);
        }
    }

    public void TryPlaceTile()
    {
        if (_selectedTile == null || _tilemap == null) return;

        if (IsPointerOverUI()) return;

        Vector3Int cellPosition = GetMouseCellPosition();

        if (_tilemap.HasTile(cellPosition)) return;

        _tilemap.SetTile(cellPosition, _selectedTile);

        if (_playerAnimator != null)
        {
            _playerAnimator.SetTrigger(AnimatorPlaceTriggerHash);
        }

        if (AudioManager.Instance != null && _placeSound != null)
        {
            AudioManager.Instance.PlaySFX(_placeSound);
        }
    }
}
