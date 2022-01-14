using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
//[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NPCPath))]
//[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class NPCMovement : MonoBehaviour
{
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

    private Direction direction;

    public SceneName npcCurrentScene;
    [HideInInspector] public SceneName npcTargetScene;
    [HideInInspector] public Vector3Int npcCurrentGridPosition;
    [HideInInspector] public Vector3Int npcTargetGridPosition;
    [HideInInspector] public Vector3 npcTargetWorldPosition;
    public Direction npcFacingDirectionAtDestination;

    private SceneName npcPreviousMovementStepScene;
    private Vector3Int npcNextGridPosition;
    private Vector3 npcNextWorldPosition;

    [Header("NPC Movement")]
    public float npcNormalSpeed = 2f;

    [SerializeField] private float npcMinSpeed = 1f;
    [SerializeField] private float npcMaxSpeed = 3f;
    private bool npcIsMoving = false;

    [HideInInspector] public AnimationClip npcTargetAnimationClip;
    [HideInInspector] public AnimationType npcAnimationType = AnimationType.idle;


    [Header("NPC Animation")]
    [SerializeField] private AnimationClip blankAnimation = null;

    private Grid grid;
    private Rigidbody2D rigidBody2D;
    private BoxCollider2D boxCollider2D;
    private WaitForFixedUpdate waitForFixedUpdate;
    //private Animator animator;
    private NPCMovementAnimationParameters[] npcMovementAnimationParameterControl;
    private AnimationOverrides animationOverrides;
    private AnimatorOverrideController animatorOverrideController;
    private int lastMoveAnimationParameter;
    private NPCPath npcPath;
    private bool npcInitialised = false;
    //private SpriteRenderer spriteRenderer;
    [HideInInspector] public bool npcActiveInScene = false;

    private WaitForSeconds afterUseToolAnimationPause;
    private WaitForSeconds afterLiftToolAnimationPause;
    private WaitForSeconds useToolAnimationPause;
    private WaitForSeconds liftToolAnimationPause;

    private CharacterAttribute armsCharacterAttribute;
    private CharacterAttribute toolCharacterAttribute;
    private List<CharacterAttribute> characterAttributeCustomisationList;

    private bool sceneLoaded = false;

    private Coroutine moveToGridPositionRoutine;

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += AfterSceneLoad;
        EventHandler.BeforeSceneUnloadEvent += BeforeSceneUnloaded;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= AfterSceneLoad;
        EventHandler.BeforeSceneUnloadEvent -= BeforeSceneUnloaded;
    }

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        grid = GameObject.FindObjectOfType<Grid>();
        //animator = GetComponent<Animator>();
        npcPath = GetComponent<NPCPath>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        animationOverrides = GetComponentInChildren<AnimationOverrides>();
        armsCharacterAttribute = new CharacterAttribute(CharacterPartAnimator.arms, PartVariantColour.none, PartVariantType.none);
        characterAttributeCustomisationList = new List<CharacterAttribute>();
        //npcMovementAnimationParameterControl = new List<NPCMovementAnimationParameterControl>();

        //animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        //animator.runtimeAnimatorController = animatorOverrideController;
        npcMovementAnimationParameterControl = GetComponentsInChildren<NPCMovementAnimationParameters>();
        npcCurrentScene = SceneControllerManager.Instance.GetCurrentScene();
        // Initialise target world position, target grid position & target scene to current
        npcTargetScene = npcCurrentScene;
        npcTargetGridPosition = npcCurrentGridPosition;
        npcTargetWorldPosition = transform.position;

        if (!npcInitialised)
        {
            InitialiseNPC();
            npcInitialised = true;
        }

        sceneLoaded = true;
    }

    // Start is called before the first frame update
    private void Start()
    {
        waitForFixedUpdate = new WaitForFixedUpdate();
        useToolAnimationPause = new WaitForSeconds(Settings.useToolAnimationPause);
        liftToolAnimationPause = new WaitForSeconds(Settings.liftToolAnimationPause);
        afterUseToolAnimationPause = new WaitForSeconds(Settings.afterUseToolAnimationPause);
        afterLiftToolAnimationPause = new WaitForSeconds(Settings.afterLiftToolAnimationPause);
        SetIdleAnimation();
    }

    private void FixedUpdate()
    {
        if (sceneLoaded)
        {
            //Debug.Log("sceneLoaded");

            if (npcIsMoving == false)
            {

                // set npc current and next grid position - to take into account the npc might be animating
                npcCurrentGridPosition = GetGridPosition(transform.position);
                npcNextGridPosition = npcCurrentGridPosition;

                if (npcPath.npcMovementStepStack.Count > 0)
                {
                    NPCMovementStep npcMovementStep = npcPath.npcMovementStepStack.Peek();

                    npcCurrentScene = npcMovementStep.sceneName;

                    // If NPC is about the move to a new scene reset position to starting point in new scene and update the step times
                    if (npcCurrentScene != npcPreviousMovementStepScene)
                    {
                        //isIdle = true;
                        npcCurrentGridPosition = (Vector3Int)npcMovementStep.gridCoordinate;
                        npcNextGridPosition = npcCurrentGridPosition;
                        transform.position = GetWorldPosition(npcCurrentGridPosition);
                        npcPreviousMovementStepScene = npcCurrentScene;
                        npcPath.UpdateTimesOnPath();
                    }


                    // If NPC is in current scene then set NPC to active to make visible, pop the movement step off the stack and then call method to move NPC
                    if (npcCurrentScene.ToString() == SceneManager.GetActiveScene().name)
                    {
                        SetNPCActiveInScene();
                        //isWalking = true;
                        npcMovementStep = npcPath.npcMovementStepStack.Pop();

                        npcNextGridPosition = (Vector3Int)npcMovementStep.gridCoordinate;

                        TimeSpan npcMovementStepTime = new TimeSpan(npcMovementStep.hour, npcMovementStep.minute, npcMovementStep.second);

                        MoveToGridPosition(npcNextGridPosition, npcMovementStepTime, TimeManager.Instance.GetGameTime());
                    }

                    // else if NPC is not in current scene then set NPC to inactive to make invisible
                    // - once the movement step time is less than game time (in the past) then pop movement step off the stack and set NPC position to movement step position
                    else
                    {
                        SetNPCInactiveInScene();

                        npcCurrentGridPosition = (Vector3Int)npcMovementStep.gridCoordinate;
                        npcNextGridPosition = npcCurrentGridPosition;
                        transform.position = GetWorldPosition(npcCurrentGridPosition);

                        TimeSpan npcMovementStepTime = new TimeSpan(npcMovementStep.hour, npcMovementStep.minute, npcMovementStep.second);

                        TimeSpan gameTime = TimeManager.Instance.GetGameTime();

                        if (npcMovementStepTime < gameTime)
                        {
                            npcMovementStep = npcPath.npcMovementStepStack.Pop();

                            npcCurrentGridPosition = (Vector3Int)npcMovementStep.gridCoordinate;
                            npcNextGridPosition = npcCurrentGridPosition;
                            transform.position = GetWorldPosition(npcCurrentGridPosition);
                        }
                    }

                }
                // else if no more NPC movement steps
                else
                {
                    //Debug.Log("NPC HAS REACHED DESTINATION");
                    ResetMoveAnimation();

                    SetNPCFacingDirection();

                    SetNPCEventAnimation();
                }
            }
        }
    }

    public void SetScheduleEventDetails(NPCScheduleEvent npcScheduleEvent)
    {
        //Debug.Log("SetScheduleEventDetails");

        npcTargetScene = npcScheduleEvent.toSceneName;
        npcTargetGridPosition = (Vector3Int)npcScheduleEvent.toGridCoordinate;
        npcTargetWorldPosition = GetWorldPosition(npcTargetGridPosition);
        //npcFacingDirectionAtDestination = (Direction) UnityEngine.Random.Range(0, 5);
        npcFacingDirectionAtDestination = npcScheduleEvent.npcFacingDirectionAtDestination;
        npcTargetAnimationClip = npcScheduleEvent.animationAtDestination;

        npcAnimationType = npcScheduleEvent.animationType;
        ClearNPCEventAnimation();
    }

    private void SetNPCEventAnimation()
    {

            ResetIdleAnimation();
            switch (npcAnimationType)
            {
                case AnimationType.idle:
                    isIdle = true;
                    break;
                case AnimationType.holdTool:

                    break;
                case AnimationType.liftTool:
                    switch (direction)
                    {
                        case Direction.up:
                            isLiftingToolUp = true;
                            break;
                        case Direction.down:
                            isLiftingToolDown = true;
                            break;
                        case Direction.left:
                            isLiftingToolLeft = true;
                            break;
                        case Direction.right:
                            isLiftingToolRight = true;
                            break;
                    }
                    break;
                case AnimationType.pickUp:
                    switch (direction)
                    {
                        case Direction.up:
                            isPickingUp = true;
                            break;
                        case Direction.down:
                            isPickingDown = true;
                            break;
                        case Direction.left:
                            isPickingLeft = true;
                            break;
                        case Direction.right:
                            isPickingRight = true;
                            break;
                    }
                    break;
                case AnimationType.swingTool:
                    switch (direction)
                    {
                        case Direction.up:
                            isSwingingToolUp = true;
                            break;
                        case Direction.down:
                            isSwingingToolDown = true;
                            break;
                        case Direction.left:
                            isSwingingToolLeft = true;
                            break;
                        case Direction.right:
                            isSwingingToolRight = true;
                            break;
                    }
                    break;
                case AnimationType.useTool:
                    switch (direction)
                    {
                        case Direction.up:
                            isUsingToolUp = true;
                            break;
                        case Direction.down:
                            isUsingToolDown = true;
                            break;
                        case Direction.left:
                            isUsingToolLeft = true;
                            break;
                        case Direction.right:
                            isUsingToolRight = true;
                            break;
                    }
                    break;
                case AnimationType.walk:
                    isWalking = true;
                    break;

            }
            TriggerMovementAnimationParameterControl(0, 0);




    }

    public void ClearNPCEventAnimation()
    {

        // Clear any rotation on npc
        transform.rotation = Quaternion.identity;
    }

    private void SetNPCFacingDirection()
    {
        ResetIdleAnimation();
        ResetMoveAnimation();

        switch (npcFacingDirectionAtDestination)
        {
            case Direction.up:
                direction = Direction.up;
                break;

            case Direction.down:
                direction = Direction.down;

                break;

            case Direction.left:
                direction = Direction.left;

                break;

            case Direction.right:
                direction = Direction.right;

                break;

            case Direction.none:
                direction = Direction.none;

                break;

            default:
                break;
        }
    }

    public void SetNPCActiveInScene()
    {
        //spriteRenderer.enabled = true;
        boxCollider2D.enabled = true;
        npcActiveInScene = true;
    }

    public void SetNPCInactiveInScene()
    {
        //spriteRenderer.enabled = false;
        boxCollider2D.enabled = false;
        npcActiveInScene = false;
    }

    private void AfterSceneLoad()
    {
        //grid = GameObject.FindObjectOfType<Grid>();

        if (!npcInitialised)
        {
            InitialiseNPC();
            npcInitialised = true;
        }

        sceneLoaded = true;
    }

    private void BeforeSceneUnloaded()
    {
        sceneLoaded = false;
    }

    /// <summary>
    /// returns the grid position given the worldPosition
    /// </summary>
    private Vector3Int GetGridPosition(Vector3 worldPosition)
    {
        if (grid != null)
        {
            return grid.WorldToCell(worldPosition);
        }
        else
        {
            return Vector3Int.zero;
        }
    }

    /// <summary>
    ///  returns the world position (centre of grid square) from gridPosition
    /// </summary>
    public Vector3 GetWorldPosition(Vector3Int gridPosition)
    {
        Vector3 worldPosition = grid.CellToWorld(gridPosition);

        // Get centre of grid square
        return new Vector3(worldPosition.x + Settings.gridCellSize / 2f, worldPosition.y + Settings.gridCellSize / 2f, worldPosition.z);
    }

    public void CancelNPCMovement()
    {
        npcPath.ClearPath();
        npcNextGridPosition = Vector3Int.zero;
        npcNextWorldPosition = Vector3.zero;
        npcIsMoving = false;

        if (moveToGridPositionRoutine != null)
        {
            StopCoroutine(moveToGridPositionRoutine);
        }

        // Reset move animation
        ResetMoveAnimation();

        // Clear event animation
        ClearNPCEventAnimation();
        npcTargetAnimationClip = null;

        // Reset idle animation
        ResetIdleAnimation();

        // Set idle animation
        SetIdleAnimation();
    }


    private void InitialiseNPC()
    {
        // Active in scene
        if (npcCurrentScene.ToString() == SceneManager.GetActiveScene().name)
        {
            SetNPCActiveInScene();
        }
        else
        {
            SetNPCInactiveInScene();
        }

        npcPreviousMovementStepScene = npcCurrentScene;

        // Get NPC Current Grid Position
        npcCurrentGridPosition = GetGridPosition(transform.position);

        // Set Next Grid Position and Target Grid Position to current Grid Position
        npcNextGridPosition = npcCurrentGridPosition;
        npcTargetGridPosition = npcCurrentGridPosition;
        npcTargetWorldPosition = GetWorldPosition(npcTargetGridPosition);

        // Get NPC WorldPosition
        npcNextWorldPosition = GetWorldPosition(npcCurrentGridPosition);
    }

    private void MoveToGridPosition(Vector3Int gridPosition, TimeSpan npcMovementStepTime, TimeSpan gameTime)
    {


        moveToGridPositionRoutine = StartCoroutine(MoveToGridPositionRoutine(gridPosition, npcMovementStepTime, gameTime));
    }

    private IEnumerator MoveToGridPositionRoutine(Vector3Int gridPosition, TimeSpan npcMovementStepTime, TimeSpan gameTime)
    {
        npcIsMoving = true;
        isWalking = true;
        SetMoveAnimation(gridPosition);

        npcNextWorldPosition = GetWorldPosition(gridPosition);

        // If movement step time is in the future, otherwise skip and move NPC immediately to position
        if (npcMovementStepTime > gameTime)
        {
            //calculate time difference in seconds
            float timeToMove = (float)(npcMovementStepTime.TotalSeconds - gameTime.TotalSeconds);

            // Calculate speed
            float npcCalculatedSpeed = Mathf.Max(npcMinSpeed, Vector3.Distance(transform.position, npcNextWorldPosition) / timeToMove / Settings.secondsPerGameSecond);

            //// If speed is at least npc min speed and less than npc max speed  then process, otherwise skip and move NPC immediately to position
            if (npcCalculatedSpeed <= npcMaxSpeed)
            {
                while (Vector3.Distance(transform.position, npcNextWorldPosition) > Settings.pixelSize)
                {
                    Vector3 unitVector = Vector3.Normalize(npcNextWorldPosition - transform.position);
                    Vector2 move = new Vector2(unitVector.x * npcCalculatedSpeed * Time.fixedDeltaTime, unitVector.y * npcCalculatedSpeed * Time.fixedDeltaTime);

                    rigidBody2D.MovePosition(rigidBody2D.position + move);

                    yield return waitForFixedUpdate;
                }
            }
        }

        //ResetMoveAnimation();
        //ResetIdleAnimation();
        //Debug.Log(isWalking);
        //Debug.Log("---------");

        //Debug.Log(isIdle);
        rigidBody2D.position = npcNextWorldPosition;
        npcCurrentGridPosition = gridPosition;
        npcNextGridPosition = npcCurrentGridPosition;

        npcIsMoving = false;
    }

    private void SetMoveAnimation(Vector3Int gridPosition)
    {


        // get world position
        Vector3 toWorldPosition = GetWorldPosition(gridPosition);

        // get vector
        Vector3 directionVector = toWorldPosition - transform.position;

        if (Mathf.Abs(directionVector.x) >= Mathf.Abs(directionVector.y))
        {
            // Use left/right animation
            if (directionVector.x > 0)
            {
                direction = Direction.right;
                TriggerMovementAnimationParameterControl(directionVector.x, directionVector.y);
                //animator.SetBool(Settings.walkRight, true);
            }
            else
            {
                direction = Direction.left;
                TriggerMovementAnimationParameterControl(directionVector.x, directionVector.y);

                //animator.SetBool(Settings.walkLeft, true);
            }
        }
        else
        {
            //Use up/down animation
            if (directionVector.y > 0)
            {
                direction = Direction.up;
                TriggerMovementAnimationParameterControl(directionVector.x, directionVector.y);

                //animator.SetBool(Settings.walkUp, true);
            }
            else
            {
                direction = Direction.down;
                TriggerMovementAnimationParameterControl(directionVector.x, directionVector.y);

                //animator.SetBool(Settings.walkDown, true);
            }
        }

        // Reset idle animation
        //ResetIdleAnimation();

        // Reset move animation
        //ResetMoveAnimation();
    }

    private void TriggerMovementAnimationParameterControl(float x, float y)
    {
        for (int i = 0; i < npcMovementAnimationParameterControl.Length; i++)
        {
            npcMovementAnimationParameterControl[i].SetAnimationParameters(
    x, y, isWalking, isRunning, isIdle, isCarrying, toolEffect, isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown, isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
false, false, false, false
    );
        }
    }

    private void SetIdleAnimation()
    {
        //isIdle = true;
    }

    private void ResetMoveAnimation()
    {
        ResetMovement();
    }

    private void ResetMovement()
    {
        isRunning = false;
        //isWalking = false;
        //isIdle = true;
        TriggerMovementAnimationParameterControl(0, 0);
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

    private void ResetIdleAnimation()
    {
        ResetAnimationTriggers();
        TriggerMovementAnimationParameterControl(0, 0);


    }
}