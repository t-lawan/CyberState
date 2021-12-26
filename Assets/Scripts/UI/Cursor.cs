
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    private Canvas canvas;
    private Camera mainCamera;

    [SerializeField] private Image cursorImage = null;
    [SerializeField] private RectTransform cursorRectTransform = null;
    [SerializeField] private Sprite greenCursorSprite = null;
    [SerializeField] private Sprite transparentCursorSprite = null;
    [SerializeField] private GridCursor gridCursor = null;

    private bool _cursorIsEnabled = false;
    public bool CursorIsEnabled { get => _cursorIsEnabled; set => _cursorIsEnabled = value; }

    private bool _cursorPositionIsValid = false;
    public bool CursorPositionIsValid { get => _cursorPositionIsValid; set => _cursorPositionIsValid = value; }

    private ItemType _selectedItemType;
    public ItemType SelectedItemType { get => _selectedItemType; set => _selectedItemType = value; }

    private float _itemUseRadius = 0.0f;
    public float ItemUseRadius { get => _itemUseRadius; set => _itemUseRadius = value; }

    // Start is called before the first frame update
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

    private void DisplayCursor()
    {

        Vector3 cursorWorldPosition = GetWorldPositionForCursor();

        SetCursorValidity(cursorWorldPosition, Player.Instance.GetPlayerCenterPosition());

        cursorRectTransform.position = GetRectTransformPositionForCursor();


    }

    private Vector2 GetRectTransformPositionForCursor()
    {
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        return RectTransformUtility.PixelAdjustPoint(screenPosition, cursorRectTransform, canvas);

    }

    public Vector3 GetWorldPositionForCursor()
    {
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

        return worldPosition;
    }


    private void SetCursorValidity(Vector3 cursorPosition, Vector3 playerPosition)
    {
        SetCursorToValid();

        if (
            cursorPosition.x > (playerPosition.x + ItemUseRadius / 2f) && cursorPosition.y > (playerPosition.y + ItemUseRadius / 2f) ||
            cursorPosition.x > (playerPosition.x - ItemUseRadius / 2f) && cursorPosition.y > (playerPosition.y + ItemUseRadius / 2f) ||
            cursorPosition.x < (playerPosition.x - ItemUseRadius / 2f) && cursorPosition.y < (playerPosition.y - ItemUseRadius / 2f) ||
            cursorPosition.x < (playerPosition.x + ItemUseRadius / 2f) && cursorPosition.y < (playerPosition.y - ItemUseRadius / 2f)
            )
        {
            SetCursorToInValid();
            return;
        }

        if (Mathf.Abs(cursorPosition.x - playerPosition.x) > ItemUseRadius ||
            Mathf.Abs(cursorPosition.y - playerPosition.y) > ItemUseRadius)
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


        switch (itemDetails.itemType)
        {
            case ItemType.Watering_Tool:

            case ItemType.Breaking_Tool:
            case ItemType.Fixing_Tool:
            case ItemType.Hoeing_Tool:
            case ItemType.Reaping_Tool:
            case ItemType.Collecting_Tool:
                if (!SetCursorValidityTool(cursorPosition, playerPosition, itemDetails))
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

    private bool SetCursorValidityTool(Vector3 cursorPosition, Vector3 playerPosition, ItemDetails itemDetails)
    {
        switch (itemDetails.itemType)
        {
            case ItemType.Reaping_Tool:
                return SetCursorValidityReapingTool(cursorPosition, playerPosition, itemDetails);
            default:
                return false;
        }
    }

    private bool SetCursorValidityReapingTool(Vector3 cursorPosition, Vector3 playerPosition, ItemDetails itemDetails)
    {
        List<Item> itemList = new List<Item>();

        if (HelperMethods.GetComponentAtCursorLocation<Item>(out itemList, cursorPosition))
        {
            if (itemList.Count != 0)
            {
                foreach (Item item in itemList)
                {
                    if (InventoryManager.Instance.GetItemDetails(item.ItemCode).itemType == ItemType.Reapable_scenery)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void SetCursorToInValid()
    {
        cursorImage.sprite = transparentCursorSprite;
        CursorPositionIsValid = false;
        gridCursor.EnableCursor();

    }

    private void SetCursorToValid()
    {
        cursorImage.sprite = greenCursorSprite;
        CursorPositionIsValid = true;
        gridCursor.DisableCursor();
    }

}
