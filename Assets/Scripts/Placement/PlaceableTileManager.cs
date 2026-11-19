using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlaceableTileManager : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;

    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private AudioClip _placeSound;
    [SerializeField] private AudioClip _displaceSound;

    [SerializeField] private Tile[] _tiles;

    private bool _isDisplaceMode = false;

    private Tile _selectedTile;
    private SpriteRenderer _ghostPreviewRenderer;
    private Camera _camera;
    private InputAction _clickAction;
    private InputAction _cancelAction;

    private readonly PlacementPrefManager _placementPrefManager = new();

    private const string GHOST_PREVIEW_RENDERER_NAME = "PlacePreview_Ghost";
    private const string GHOST_PREVIEW_SORTING_LAYER_NAME = "Placeable";

    private const string ANIMATOR_PLACE_TRIGGER = "Place";
    private const string ANIMATOR_DISPLACE_TRIGGER = "Displace";
    private static readonly int AnimatorPlaceTriggerHash = Animator.StringToHash(ANIMATOR_PLACE_TRIGGER);
    private static readonly int AnimatorDisplaceTriggerHash = Animator.StringToHash(ANIMATOR_DISPLACE_TRIGGER);

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

        _playerController.ArrivedAction += OnPlayerArrived;
    }

    private void OnDisable()
    {
        _clickAction.performed -= OnClickPerformed;
        _clickAction.Disable();

        _cancelAction.performed -= OnCancelPerformed;
        _cancelAction.Disable();

        _playerController.ArrivedAction -= OnPlayerArrived;
    }

    private void Start()
    {
        LoadPlacement();
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
        if (_isDisplaceMode)
        {
            TryDisplaceTile();
        }
        else
        {
            TryPlaceTile();
        }
    }

    private void OnCancelPerformed(InputAction.CallbackContext context)
    {
        ClearSelectedTile();
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
        _isDisplaceMode = false;

        if (index >= 0 && index < _tiles.Length)
        {
            _selectedTile = _tiles[index];
            _ghostPreviewRenderer.sprite = _selectedTile.sprite;
            _ghostPreviewRenderer.gameObject.SetActive(true);
        }
    }

    public void ClearSelectedTile()
    {
        _selectedTile = null;
    }

    public void TryPlaceTile()
    {
        if (_selectedTile == null || _tilemap == null || IsPointerOverUI()) return;

        Vector3Int cellDestination = GetMouseCellPosition();

        if (_tilemap.HasTile(cellDestination)) return;

        Vector3 worldDestination = _tilemap.GetCellCenterWorld(cellDestination);

        _playerController.AutoMoveTowards(worldDestination, cellDestination);
    }

    public void ToggleDisplaceMode()
    {
        ClearSelectedTile();
        _isDisplaceMode = !_isDisplaceMode;
    }

    public void TryDisplaceTile()
    {
        if (_tilemap == null || IsPointerOverUI()) return;

        Vector3Int cellDestination = GetMouseCellPosition();

        if (!_tilemap.HasTile(cellDestination)) return;

        Vector3 worldDestination = _tilemap.GetCellCenterWorld(cellDestination);

        _playerController.AutoMoveTowards(worldDestination, cellDestination);
    }

    private void OnPlayerArrived(Vector3Int cellPosition)
    {
        if (_isDisplaceMode)
        {
            DisplaceWhenPlayerArrived(cellPosition);
        }
        else
        {
            PlaceWhenPlayerArrived(cellPosition);
        }
    }

    private void PlaceWhenPlayerArrived(Vector3Int cellPosition)
    {
        _tilemap.SetTile(cellPosition, _selectedTile);
        SavePlacement(cellPosition);

        if (_playerAnimator != null)
        {
            _playerAnimator.SetTrigger(AnimatorPlaceTriggerHash);
        }

        if (AudioManager.Instance != null && _placeSound != null)
            AudioManager.Instance.PlaySFX(_placeSound);
    }

    private void DisplaceWhenPlayerArrived(Vector3Int cellPosition)
    {
        _tilemap.SetTile(cellPosition, null);
        SaveDisplacement(cellPosition);

        if (_playerAnimator != null)
        {
            _playerAnimator.SetTrigger(AnimatorDisplaceTriggerHash);
        }

        if (AudioManager.Instance != null && _displaceSound != null)
        {
            AudioManager.Instance.PlaySFX(_displaceSound);
        }
    }

    public void LoadPlacement()
    {
        _placementPrefManager.Load();

        foreach (var tileData in _placementPrefManager.Tiles)
        {
            _tilemap.SetTile(tileData.position, _tiles[tileData.index]);
        }
    }

    private void SavePlacement(Vector3Int cellPosition)
    {
        int selectedTileIndex = Array.IndexOf(_tiles, _selectedTile);
        _placementPrefManager.SavedAdd(new PlacementPrefManager.PlacedTilePrefData
        {
            position = cellPosition,
            index = selectedTileIndex
        });
    }

    public void SaveDisplacement(Vector3Int cellPosition)
    {
        _placementPrefManager.SavedRemoveAll((tileData) => tileData.position.Equals(cellPosition));
    }
}
