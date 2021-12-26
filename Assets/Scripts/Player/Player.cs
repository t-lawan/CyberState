using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonMonoBehaviour<Player>
{
    private WaitForSeconds afterUseToolAnimationPause;
    private WaitForSeconds afterLiftToolAnimationPause;
    private AnimationOverrides animationOverrides;

    //    private GridCursor gridCursor;
    // Movement Parameters
    private float xInput;
    private float yInput;

    private bool isWalking;
    private bool isRunning;
    private bool isIdle;
    private bool isCarrying;
    private ToolEffect toolEffect;

    private bool isUsingToolRight;
    private bool isUsingToolLeft;
    private bool isUsingToolUp;
    private bool isUsingToolDown;

    private bool isLiftingToolRight;
    private bool isLiftingToolLeft;
    private bool isLiftingToolUp;
    private bool isLiftingToolDown;

    private bool isPickingRight;
    private bool isPickingLeft;
    private bool isPickingUp;
    private bool isPickingDown;

    private bool isSwingingToolRight;
    private bool isSwingingToolLeft;
    private bool isSwingingToolUp;
    private bool isSwingingToolDown;

    private Camera mainCamera;
    private bool playerToolUseDisabled = false;
    private Rigidbody2D rigidBody2D;
    private WaitForSeconds useToolAnimationPause;
    private WaitForSeconds liftToolAnimationPause;


    private Direction direction;

    private List<CharacterAttribute> characterAttributeCustomisationList;
    private float movementSpeed;

    //    [Tooltip("Should be populated in the prefav with the equipped item sprite renderer")]
    [SerializeField] private SpriteRenderer equippedItemSpriteRenderer = null;

    private CharacterAttribute armsCharacterAttribute;
    private CharacterAttribute toolCharacterAttribute;

    private bool _playerInputIsDisabled = false;
    public bool PlayerInputIsDisabled { get => _playerInputIsDisabled; set => _playerInputIsDisabled = value; }

    protected override void Awake()
    {
        base.Awake();

        rigidBody2D = GetComponent<Rigidbody2D>();

        animationOverrides = GetComponentInChildren<AnimationOverrides>();

        armsCharacterAttribute = new CharacterAttribute(CharacterPartAnimator.arms, PartVariantColour.none, PartVariantType.none);

        characterAttributeCustomisationList = new List<CharacterAttribute>();
        mainCamera = Camera.main;
    }

    //    private void Start()
    //    {
    //        gridCursor = FindObjectOfType<GridCursor>();
    //        useToolAnimationPause = new WaitForSeconds(Settings.useToolAnimationPause);
    //        liftToolAnimationPause = new WaitForSeconds(Settings.liftToolAnimationPause);
    //        afterUseToolAnimationPause = new WaitForSeconds(Settings.afterUseToolAnimationPause);
    //        afterLiftToolAnimationPause = new WaitForSeconds(Settings.afterLiftToolAnimationPause);
    //    }

    private void Update()
    {
        #region Player Input

        if (!PlayerInputIsDisabled)
        {
            ResetAnimationTriggers();
            PlayerMovementInput();
            PlayerWalkInput();
            //PlayerClickInput();

            //PlayerTestInput();
            SendMovementEventToListeners();
        }

        #endregion
    }



    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 move = new Vector2(xInput * movementSpeed * Time.deltaTime, yInput * movementSpeed * Time.deltaTime);

        rigidBody2D.MovePosition(rigidBody2D.position + move);
    }

    private void PlayerMovementInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if (yInput != 0 && xInput != 0)
        {
            xInput = xInput * 0.71f;
            yInput = yInput * 0.71f;

        }

        if (yInput != 0 || xInput != 0)
        {
            isRunning = true;
            isWalking = false;
            isIdle = false;
            movementSpeed = Settings.runningSpeed;

            if (xInput < 0)
            {
                direction = Direction.left;
            }
            else if (xInput > 0)
            {
                direction = Direction.right;
            }

            else if (yInput < 0)
            {
                direction = Direction.down;
            }
            else if (yInput > 0)
            {
                direction = Direction.up;

            }

        }
        else if (xInput == 0 && yInput == 0)
        {
            ResetMovement();
        }


    }

    private void ResetAnimationTriggers()
    {
        isUsingToolRight = false;
        isUsingToolLeft = false;
        isUsingToolUp = false;
        isUsingToolDown = false;

        isLiftingToolRight = false;
        isLiftingToolLeft = false;
        isLiftingToolUp = false;
        isLiftingToolDown = false;

        isPickingRight = false;
        isPickingLeft = false;
        isPickingUp = false;
        isPickingDown = false;

        isSwingingToolRight = false;
        isSwingingToolLeft = false;
        isSwingingToolUp = false;
        isSwingingToolDown = false;

        isCarrying = false;
        toolEffect = ToolEffect.none;
    }

    private void PlayerWalkInput()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            isRunning = true;
            isWalking = false;
            isIdle = false;
            movementSpeed = Settings.runningSpeed;
        }

        else
        {
            isRunning = false;
            isWalking = true;
            isIdle = false;
            movementSpeed = Settings.walkingSpeed;
        }
    }

    //private void PlayerClickInput()
    //{
    //    if (!playerToolUseDisabled)
    //    {
    //        if (Input.GetMouseButton(0))
    //        {

    //            if (gridCursor.CursorIsEnabled)
    //            {
    //                Vector3Int cursorGridPosition = gridCursor.GetGridPositionForCursor();
    //                Vector3Int playerGridPosition = gridCursor.GetGridPositionForPlayer();
    //                ProcessPlayerClickInput(cursorGridPosition, playerGridPosition);
    //            }
    //        }

    //    }

    //}

    //    private void ProcessPlayerClickInput(Vector3Int cursorGridPosition, Vector3Int playerGridPosition)
    //    {
    //        ResetMovement();

    //        Vector3Int playerDirection = GetPlayerClickDirection(cursorGridPosition, playerGridPosition);

    //        GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(cursorGridPosition.x, cursorGridPosition.y);
    //        ItemDetails itemDetails = InventoryManager.Instance.GetSelectedInventoryItemDetails(InventoryLocation.player);

    //        if (itemDetails != null)
    //        {
    //            switch (itemDetails.itemType)
    //            {
    //                case ItemType.Seed:
    //                    if (Input.GetMouseButtonDown(0))
    //                    {
    //                        ProcessPlayerClickInputSeed(itemDetails);
    //                    }
    //                    break;
    //                case ItemType.Commodity:
    //                    if (Input.GetMouseButtonDown(0))
    //                    {
    //                        ProcessPlayerClickInputCommidity(itemDetails);
    //                    }
    //                    break;
    //                case ItemType.Watering_Tool:
    //                    ProcessPlayerClickInputTool(gridPropertyDetails, itemDetails, playerDirection);
    //                    break;
    //                case ItemType.Hoeing_Tool:
    //                    ProcessPlayerClickInputTool(gridPropertyDetails, itemDetails, playerDirection);
    //                    break;
    //                case ItemType.none:
    //                    break;
    //                case ItemType.count:
    //                    break;
    //                default:
    //                    break;
    //            }
    //        }
    //    }

    //    private void ProcessPlayerClickInputTool(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails, Vector3Int playerDirection)
    //    {
    //        switch (itemDetails.itemType)
    //        {
    //            case ItemType.Hoeing_Tool:
    //                if (gridCursor.CursorPositionIsValid)
    //                {
    //                    HoeGroundAtCursor(gridPropertyDetails, playerDirection);
    //                }
    //                break;
    //            case ItemType.Watering_Tool:
    //                if (gridCursor.CursorPositionIsValid)
    //                {
    //                    WaterGroundAtCursor(gridPropertyDetails, playerDirection);
    //                }
    //                break;
    //            default:
    //                break;
    //        }
    //    }

    //    private void WaterGroundAtCursor(GridPropertyDetails gridPropertyDetails, Vector3Int playerDirection)
    //    {
    //        StartCoroutine(WaterGroundAtCursorRoutine(playerDirection, gridPropertyDetails));

    //    }

    //    private IEnumerator WaterGroundAtCursorRoutine(Vector3Int playerDirection, GridPropertyDetails gridPropertyDetails)
    //    {
    //        PlayerInputIsDisabled = true;
    //        playerToolUseDisabled = true;

    //        toolCharacterAttribute.partVariantType = PartVariantType.wateringCan;
    //        characterAttributeCustomisationList.Clear();
    //        characterAttributeCustomisationList.Add(toolCharacterAttribute);
    //        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomisationList);

    //        toolEffect = ToolEffect.watering;

    //        if (playerDirection == Vector3Int.right)
    //        {
    //            isLiftingToolRight = true;
    //        }

    //        if (playerDirection == Vector3Int.left)
    //        {
    //            isLiftingToolLeft = true;
    //        }

    //        if (playerDirection == Vector3Int.up)
    //        {
    //            isLiftingToolUp = true;
    //        }

    //        if (playerDirection == Vector3Int.down)
    //        {
    //            isLiftingToolDown = true;
    //        }

    //        yield return liftToolAnimationPause;

    //        if (gridPropertyDetails.daySinceWatered == -1)
    //        {
    //            gridPropertyDetails.daySinceWatered = 0;
    //        }

    //        GridPropertiesManager.Instance.SetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY, gridPropertyDetails);
    //        GridPropertiesManager.Instance.DisplayWateredGround(gridPropertyDetails);

    //        yield return afterLiftToolAnimationPause;

    //        PlayerInputIsDisabled = false;
    //        playerToolUseDisabled = false;
    //    }

    //    private void HoeGroundAtCursor(GridPropertyDetails gridPropertyDetails, Vector3Int playerDirection)
    //    {
    //        StartCoroutine(HoeGroundAtCursorRoutine(playerDirection, gridPropertyDetails));
    //    }

    //    private IEnumerator HoeGroundAtCursorRoutine(Vector3Int playerDirection, GridPropertyDetails gridPropertyDetails)
    //    {
    //        PlayerInputIsDisabled = true;
    //        playerToolUseDisabled = true;

    //        toolCharacterAttribute.partVariantType = PartVariantType.hoe;
    //        characterAttributeCustomisationList.Clear();
    //        characterAttributeCustomisationList.Add(toolCharacterAttribute);
    //        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomisationList);

    //        if (playerDirection == Vector3Int.right)
    //        {
    //            isUsingToolRight = true;
    //        }

    //        if (playerDirection == Vector3Int.left)
    //        {
    //            isUsingToolLeft = true;
    //        }

    //        if (playerDirection == Vector3Int.up)
    //        {
    //            isUsingToolUp = true;
    //        }

    //        if (playerDirection == Vector3Int.down)
    //        {
    //            isUsingToolDown = true;
    //        }

    //        yield return useToolAnimationPause;

    //        if (gridPropertyDetails.daySinceDug == -1)
    //        {
    //            gridPropertyDetails.daySinceDug = 0;
    //        }

    //        GridPropertiesManager.Instance.SetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY, gridPropertyDetails);
    //        GridPropertiesManager.Instance.DisplayDugGround(gridPropertyDetails);

    //        yield return afterUseToolAnimationPause;

    //        PlayerInputIsDisabled = false;
    //        playerToolUseDisabled = false;


    //    }

    //    private Vector3Int GetPlayerClickDirection(Vector3Int cursorGridPosition, Vector3Int playerGridPosition)
    //    {
    //        if (cursorGridPosition.x > playerGridPosition.x)
    //        {
    //            return Vector3Int.right;
    //        }
    //        else if (cursorGridPosition.x < playerGridPosition.x)
    //        {
    //            return Vector3Int.left;
    //        }
    //        else if (cursorGridPosition.y < playerGridPosition.y)
    //        {
    //            return Vector3Int.up;
    //        }
    //        else
    //        {
    //            return Vector3Int.down;
    //        }
    //    }

    //    private void ProcessPlayerClickInputCommidity(ItemDetails itemDetails)
    //    {
    //        if (itemDetails.canBeDropped && gridCursor.CursorPositionIsValid)
    //        {
    //            EventHandler.CallDropSelectedItemEvent();
    //        }
    //    }

    //    private void ProcessPlayerClickInputSeed(ItemDetails itemDetails)
    //    {
    //        if (itemDetails.canBeDropped && gridCursor.CursorPositionIsValid)
    //        {
    //            EventHandler.CallDropSelectedItemEvent();
    //        }
    //    }

    public void EnablePlayerInput()
    {
        PlayerInputIsDisabled = false;
    }

    public void DisablePlayerInput()
    {
        PlayerInputIsDisabled = true;
    }

    public void DisablePlayerInputAndResetMovement()
    {
        DisablePlayerInput();
        ResetMovement();
        SendMovementEventToListeners();
    }

    //    private void PlayerTestInput()
    //    {
    //        if (Input.GetKey(KeyCode.M))
    //        {
    //            TimeManager.Instance.TestAdvanceGameMinutes();
    //        }

    //        if (Input.GetKey(KeyCode.G))
    //        {
    //            TimeManager.Instance.TestAdvanceGameDay();
    //        }

    //        if (Input.GetKey(KeyCode.L))
    //        {
    //            SceneControllerManager.Instance.FadeAndLoadScene(SceneName.Scene1_Farm.ToString(), transform.position);
    //        }
    //    }

    private void ResetMovement()
    {
        xInput = 0.0f;
        yInput = 0.0f;
        isRunning = false;
        isWalking = false;
        isIdle = true;
    }

    private void SendMovementEventToListeners()
    {
        EventHandler.CallMovementEvent(xInput, yInput, isWalking, isRunning, isIdle, isCarrying, toolEffect, isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown, isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
false, false, false, false);
    }

    public void ClearCarriedItem()
    {
        equippedItemSpriteRenderer.sprite = null;
        equippedItemSpriteRenderer.color = new Color(1f, 1f, 1f, 0f);

        armsCharacterAttribute.partVariantType = PartVariantType.none;
        characterAttributeCustomisationList.Clear();
        characterAttributeCustomisationList.Add(armsCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomisationList);

        isCarrying = false;


    }

    public void ShowCarriedItem(int itemCode)
    {
        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);
        if (itemDetails != null)
        {
            equippedItemSpriteRenderer.sprite = itemDetails.itemSprite;
            equippedItemSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);

            armsCharacterAttribute.partVariantType = PartVariantType.carry;
            characterAttributeCustomisationList.Clear();
            characterAttributeCustomisationList.Add(armsCharacterAttribute);
            animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomisationList);

            isCarrying = true;
        }
    }

    public Vector3 GetPlayerViewportPosition()
    {
        return mainCamera.WorldToViewportPoint(transform.position);
    }

    public Vector3 GetPlayerCenterPosition()
    {
        return new Vector3(transform.position.x, transform.position.y + Settings.playerCentreYOffset, transform.position.z);
    }



}
