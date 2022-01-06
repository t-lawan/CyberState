using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PrefabInstatiateStruct
{
    public GameObject prefab;
    public int numberToInstatiateAtBeginning;
    public int minNumberToInstatiateEachDay;
    public int maxNumberToInstatiateEachDay;
    public SceneName sceneName;

}

public class InstatiateItems : SingletonMonoBehaviour<InstatiateItems>
{
    [SerializeField] public SOPrefabInstatiate itemPrefabs = null;
    private Transform parentItem;
    [SerializeField] private SO_GridProperties[] so_GridPropertiesArray = null;
    [SerializeField] public Vector3 minPos;
    [SerializeField] public Vector3 maxPos;
    [SerializeField] public Vector2 margin;



    private void OnEnable()
    {

        EventHandler.AfterSceneLoadEvent += AfterSceneLoad;
        EventHandler.AdvanceGameDayEvent -= AdvanceGameDay;


    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= AfterSceneLoad;
        EventHandler.AdvanceGameDayEvent -= AdvanceGameDay;

    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {

    }

    private void InstantiateSceneItemsEachDay()
    {
        for (int i = 0; i < itemPrefabs.prefabInstatiateList.Count; i++)
        {
            int numberToInstatiate = (int)Random.Range(itemPrefabs.prefabInstatiateList[i].minNumberToInstatiateEachDay, itemPrefabs.prefabInstatiateList[i].maxNumberToInstatiateEachDay);
            for (int j = 0; j < numberToInstatiate; j++)
            {
                Vector3 itemPosition = GetItemPositionInScene(itemPrefabs.prefabInstatiateList[i].sceneName);
                while (!CanPlaceItem(itemPrefabs.prefabInstatiateList[i].prefab.GetComponent<Item>(), itemPosition))
                {
                    itemPosition = GetItemPositionInScene(itemPrefabs.prefabInstatiateList[i].sceneName);
                }
                CreateItemInSpace(itemPrefabs.prefabInstatiateList[i].prefab, itemPosition);
            }
        }
    }

    private void InstantiateSceneItemsAtStartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string currentSceneName = currentScene.name;

        //SceneName currentScene = SceneControllerManager.Instance.;
        for (int i = 0; i < itemPrefabs.prefabInstatiateList.Count; i++)
        {

            for (int j = 0; j < itemPrefabs.prefabInstatiateList[i].numberToInstatiateAtBeginning; j++)
            {
                if (ShouldBeCreatedInScene(itemPrefabs.prefabInstatiateList[i], currentSceneName))
                {
                    Vector3 itemPosition = GetItemPositionInScene(itemPrefabs.prefabInstatiateList[i].sceneName);
                    
                    while (
                        !CanPlaceItem(itemPrefabs.prefabInstatiateList[i].prefab.GetComponent<Item>(), itemPosition) &&
                        !IsThereAnItemAtTheLocation(itemPrefabs.prefabInstatiateList[i].prefab, itemPosition)
                        )
                    {
                        itemPosition = GetItemPositionInScene(itemPrefabs.prefabInstatiateList[i].sceneName);
                    }
                    CreateItemInSpace(itemPrefabs.prefabInstatiateList[i].prefab, itemPosition);
                }


            }


        }

    }



    private Vector2 GetSizeOfPrefab(GameObject prefab)
    {
        BoxCollider2D collider = prefab.GetComponent<BoxCollider2D>();
        return collider.size;
    }

    private bool IsThereAnItemAtTheLocation(GameObject prefab, Vector3 itemPosition)
    {
        Vector2 point = new Vector2(itemPosition.x, itemPosition.y);
        Vector2 size = GetSizeOfPrefab(prefab);

        Item[] items = HelperMethods.GetComponentsAtBoxLocationNonAlloc<Item>(1, point, size, 0); ;
        return false;
        //return items.Length > 0;
    }

    private bool ShouldBeCreatedInScene(PrefabInstatiateStruct prefabInstatiateStruct, string sceneName)
    {
        //if(SceneControllerManager.Get)
        if(prefabInstatiateStruct.sceneName.ToString() == sceneName)
        {
            return true;
        }

        //prefabInstatiateStruct 
        return false;
    }

    private void CreateItemInSpace(GameObject prefab, Vector3 itemPosition)
    {
        GameObject itemGameObject = Instantiate(prefab, itemPosition, Quaternion.identity, parentItem);

    }

    private bool CanPlaceItem(Item item, Vector3 pos)
    {
        bool canPlaceItem = false;

        // Get Grid Property Details for Pos
        GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails((int)pos.x, (int)pos.y);

        if (gridPropertyDetails != null)
        {
            // Get Item Details from Item
            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(item.ItemCode);
            if(itemDetails != null)
            {
                //Debug.Log(itemDetails.itemCode);
                // Check if item type allows it to be dropped with conditions
                switch (itemDetails.itemType)
                {
                    case ItemType.Building:
                        if (gridPropertyDetails.canPlaceBuilding)
                        {
                            canPlaceItem = true;
                        }
                        break;
                    case ItemType.Commodity:
                        if (gridPropertyDetails.canDropItem)
                        {
                            canPlaceItem = true;
                        }
                        break;
                    case ItemType.Reapable_scenery:
                        if (gridPropertyDetails.canDropItem)
                        {
                            canPlaceItem = true;
                        }
                        break;
                    case ItemType.Seed:
                        if (gridPropertyDetails.canDropItem)
                        {
                            canPlaceItem = true;
                        }
                        break;
                    default:
                        canPlaceItem = false;
                        break;
                }
            }

        }

        //if (!canPlaceItem)
        //{
        //    Debug.Log(item.name);

        //    Debug.Log(canPlaceItem.ToString());

        //}


        return canPlaceItem;
    }

    private bool DoesItemInLocationExistAlready(ItemDetails itemDetails, Vector3 position)
    {
        Vector2 point = new Vector2(itemDetails.itemUseRadius, itemDetails.itemUseRadius);
        Vector2 size = new Vector2(itemDetails.itemUseRadius, itemDetails.itemUseRadius);

        Item[] itemArray = HelperMethods.GetComponentsAtBoxLocationNonAlloc<Item>(1, point, size, 0f);
        return itemArray.Length > 0;
    }

    private Vector3 GetRandomPosition()
    {

        return new Vector3(
             Random.Range(minPos.x, maxPos.x),
            Random.Range(minPos.y, maxPos.y),
            0);
    }

    private Vector3 GetItemPositionInScene(SceneName scene)
    {
        Vector2Int gridDimensions;
        Vector2Int gridOrigin;

        bool hasFound = GridPropertiesManager.Instance.GetGridDimensions(scene,out  gridDimensions, out gridOrigin);
        if (!hasFound)
        {
            GetRandomPosition();
        }


        return new Vector3(
            Random.Range(gridOrigin.x + margin.x, gridOrigin.x + gridDimensions.x - margin.x),
            Random.Range(gridOrigin.y + margin.y, gridOrigin.y + gridDimensions.y - margin.y),
            0
            );

    }

    private void AdvanceGameDay(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        Debug.Log("HI");
        //InstantiateSceneItemsEachDay();
    }

    private void AfterSceneLoad()
    {
        parentItem = GameObject.FindGameObjectWithTag(Tags.ItemsParentTransform).transform;
        InstantiateSceneItemsAtStartGame();

    }
}
