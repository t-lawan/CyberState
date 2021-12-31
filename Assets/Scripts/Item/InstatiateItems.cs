using UnityEngine;

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
    [SerializeField] public Vector3 minPos;
    [SerializeField] public Vector3 maxPos;


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

    private void InstantiateSceneItemsAtStartGame()
    {
        for (int i = 0; i < itemPrefabs.prefabInstatiateList.Count; i++)
        {

            for (int j = 0; j < itemPrefabs.prefabInstatiateList[i].numberToInstatiateAtBeginning; j++)
            {

                Vector3 itemPosition = GetItemPosition();
                while (!CanPlaceItem(itemPrefabs.prefabInstatiateList[i].prefab.GetComponent<Item>(), itemPosition))
                {
                    itemPosition = GetItemPosition();
                }
                CreateItemInSpace(itemPrefabs.prefabInstatiateList[i].prefab, itemPosition);

            }


        }

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
        bool canPlaceItem = false;

        // Get Grid Property Details for Pos
        GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails((int)pos.x, (int)pos.y);

        // Get Item Details from Item
        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(item.ItemCode);

        // Check if item type allows it to be dropped with conditions
        //switch (itemDetails.itemType)
        //{
        //    case ItemType.Building:
        //        //if (gridPropertyDetails.canPlaceBuilding)
        //        //{
        //        //    canPlaceItem = true;
        //        //}
        //        break;
        //    case ItemType.Commodity:
        //        //if (gridPropertyDetails.canDropItem)
        //        //{
        //        //    canPlaceItem = true;

        //        //}
        //        break;
        //    default:
        //        canPlaceItem = false;
        //        break;
        //}


        return true;
    }

    private void InstantiateSceneItemsEachDay()
    {
        for (int i = 0; i < itemPrefabs.prefabInstatiateList.Count; i++)
        {
            int numberToInstatiate = (int) Random.Range(itemPrefabs.prefabInstatiateList[i].minNumberToInstatiateEachDay, itemPrefabs.prefabInstatiateList[i].maxNumberToInstatiateEachDay);
            for (int j = 0; j < numberToInstatiate; j++)
            {
                Vector3 itemPosition = GetItemPosition();
                GameObject itemGameObject = Instantiate(itemPrefabs.prefabInstatiateList[i].prefab, itemPosition, Quaternion.identity, parentItem);
            }
        }
    }

    private Vector3 GetItemPosition()
    {
        return new Vector3(
             Random.Range(minPos.x, maxPos.x),
            Random.Range(minPos.y, maxPos.y),
            0);
    }

    private void AdvanceGameDay(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        InstantiateSceneItemsEachDay();
    }

    private void AfterSceneLoad()
    {
        parentItem = GameObject.FindGameObjectWithTag(Tags.ItemsParentTransform).transform;
        InstantiateSceneItemsAtStartGame();

    }
}
