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
        Debug.Log(currentSceneName);

        //SceneName currentScene = SceneControllerManager.Instance.;
        for (int i = 0; i < itemPrefabs.prefabInstatiateList.Count; i++)
        {

            for (int j = 0; j < itemPrefabs.prefabInstatiateList[i].numberToInstatiateAtBeginning; j++)
            {
                if (ShouldBeCreatedInScene(itemPrefabs.prefabInstatiateList[i], currentSceneName))
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

    private bool ItemAlreadyExistsAtPosition(Vector3 pos)
    {
        if (parentItem == null)
        {
            return false;
        }

        bool itemExists = false;

        //gridPropertyDetails
        //for(int i=0; i < parentItem.childCount; i++)
        //{
        //    //parentItem.GetC
        //}

        return itemExists;
    }

    private bool CanPlaceItem(Item item, Vector3 pos)
    {
        bool canPlaceItem = true;

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
                    case ItemType.Reapable_scenery:
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



        return canPlaceItem;
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
