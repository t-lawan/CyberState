using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridCursor : MonoBehaviour
{
    private Canvas canvas;
    private Grid grid;
    private Camera mainCamera;

    [SerializeField] private Image cursorImage = null;
    [SerializeField] private RectTransform cursorRectTransform = null;
    [SerializeField] private Sprite greenCursorSprite = null;
    [SerializeField] private Sprite redCursorSprite = null;

    private bool _cursorPositionIsValid = false;
    public bool CursorPositionIsValid { get => _cursorPositionIsValid; set => _cursorPositionIsValid = value; }

    private int _itemUseGridRadius = 0;
    public int ItemUseGridRadius { get => _itemUseGridRadius; set => _itemUseGridRadius = value; }

    private ItemType _selectedItemType;
    public ItemType SelectedItemType { get => _selectedItemType; set => _selectedItemType = value; }

    private bool _cursorIsEnabled = false;
    public bool CursorIsEnabled { get => _cursorIsEnabled; set => _cursorIsEnabled = value; }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= SceneLoaded;
    }

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += SceneLoaded;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        canvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        if (CursorIsEnabled)
        {
            DisplayCursor();
        }
    }

    private Vector3Int DisplayCursor()
    {
        if (grid != null)
        {
            Vector3Int gridPosition = GetGridPositionForCursor();

            Vector3Int playerGridPosition = GetGridPositionForPlayer();

            SetCursorValidity(gridPosition, playerGridPosition);

            cursorRectTransform.position = GetRectTransformPositionForCursor(gridPosition);

            return gridPosition;

        }
        else
        {
            return Vector3Int.zero;

        }
    }

    public void DisableCursor()
    {
        cursorImage.color = Color.clear;
        CursorIsEnabled = false;
    }

    public void EnableCursor()
    {
        cursorImage.color = new Color(1f, 1f, 1f, 1f);
        CursorIsEnabled = true;
    }



    private void SetCursorValidity(Vector3Int gridPosition, Vector3Int playerGridPosition)
    {
        SetCursorToValid();

        if (
            Mathf.Abs(gridPosition.x - playerGridPosition.x) > ItemUseGridRadius ||
            Mathf.Abs(gridPosition.y - playerGridPosition.y) > ItemUseGridRadius)
        {
            SetCursorToInValid();
            return;
        }

        ItemDetails itemDetails = InventoryManager.Instance.GetSelectedInventoryItemDetails(InventoryLocation.player);
        if (itemDetails == null)
        {
            SetCursorToInValid();
            return;
        }

        GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(gridPosition.x, gridPosition.y);

        if (gridPropertyDetails != null)
        {
            switch (itemDetails.itemType)
            {
                case ItemType.Seed:
                    if (!IsCursorValidForSeed(gridPropertyDetails))
                    {
                        SetCursorToInValid();
                        return;
                    }
                    break;
                case ItemType.Commodity:
                    if (!IsCursorValidForCommodity(gridPropertyDetails))
                    {
                        SetCursorToInValid();
                        return;
                    }
                    break;
                case ItemType.Breaking_Tool:
                case ItemType.Fixing_Tool:
                case ItemType.Hoeing_Tool:
                case ItemType.Reaping_Tool:
                case ItemType.Collecting_Tool:
                case ItemType.Watering_Tool:
                    if (!IsCursorValidForTool(gridPropertyDetails, itemDetails))
                    {
                        SetCursorToInValid();
                        return;
                    }
                    break;
                case ItemType.none:
                    break;
                case ItemType.count:
                    break;
                default:
                    break;

            }
        }
        else
        {
            SetCursorToInValid();
        }
    }


    private bool IsCursorValidForCommodity(GridPropertyDetails gridPropertyDetails)
    {
        return gridPropertyDetails.canDropItem;
    }

    private bool IsCursorValidForSeed(GridPropertyDetails gridPropertyDetails)
    {
        return gridPropertyDetails.canDropItem;

    }

    private bool IsCursorValidForTool(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails)
    {

        switch (itemDetails.itemType)
        {
            case ItemType.Hoeing_Tool:
                if (gridPropertyDetails.isDiggable == true && gridPropertyDetails.daySinceDug == -1)
                {
                    Vector3 cursorWorldPosition = new Vector3(GetWorldPositionForCursor().x + 0.5f, GetWorldPositionForCursor().y + 0.5f, 0f);

                    List<Item> itemList = new List<Item>();

                    HelperMethods.GetComponentsAtBoxLocation<Item>(out itemList, cursorWorldPosition, Settings.cursorSize, 0f);

                    bool foundRepeable = false;

                    foreach (Item item in itemList)
                    {
                        if (InventoryManager.Instance.GetItemDetails(item.ItemCode).itemType == ItemType.Reapable_scenery)
                        {
                            foundRepeable = true;
                            break;
                        }
                    }

                    if (foundRepeable)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            case ItemType.Watering_Tool:
                if (gridPropertyDetails.daySinceDug > -1 && gridPropertyDetails.daySinceWatered == -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            default:
                return false;
        }
        return gridPropertyDetails.canDropItem;
    }


    private void SetCursorToInValid()
    {
        cursorImage.sprite = redCursorSprite;
        CursorPositionIsValid = false;
    }

    private void SetCursorToValid()
    {
        cursorImage.sprite = greenCursorSprite;
        CursorPositionIsValid = true;
    }

    public Vector3Int GetGridPositionForPlayer()
    {
        return grid.WorldToCell(Player.Instance.transform.position);
    }

    public Vector3Int GetGridPositionForCursor()
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
        return grid.WorldToCell(worldPosition);
    }

    public Vector2 GetRectTransformPositionForCursor(Vector3Int gridPosition)
    {
        Vector3 gridWorldPosition = grid.CellToWorld(gridPosition);
        Vector2 gridScreenPosition = mainCamera.WorldToScreenPoint(gridWorldPosition);
        return RectTransformUtility.PixelAdjustPoint(gridScreenPosition, cursorRectTransform, canvas);
    }

    private Vector3 GetWorldPositionForCursor()
    {
        return grid.CellToWorld(GetGridPositionForCursor());

    }

    private void SceneLoaded()
    {
        grid = GameObject.FindObjectOfType<Grid>();
    }
}
