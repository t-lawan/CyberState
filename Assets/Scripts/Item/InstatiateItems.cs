﻿using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PrefabInstatiateStruct
{
    public GameObject prefab;
    public int numberToInstatiateAtBeginning;
    public int minNumberToInstatiateEachDay;
    public int maxNumberToInstatiateEachDay;
    public SceneName sceneName;
    public InstatiateLocation location;

}

public class InstatiateItems : SingletonMonoBehaviour<InstatiateItems>
{
    [SerializeField] public SOPrefabInstatiate itemPrefabs = null;
    private Transform parentItem;
    private Transform npcItem;
    [SerializeField] private SO_GridProperties[] so_GridPropertiesArray = null;
    [SerializeField] private GameObject humanNoidPrefab;
    [SerializeField] private GameObject ashePrefab;
    [SerializeField] private GameObject eggPrefab;
    [SerializeField] private int numberOfHumanoids;
    [SerializeField] public Vector3 minPos;
    [SerializeField] public Vector3 maxPos;
    [SerializeField] public Vector2 margin;

    private bool sceneBushHasLoaded = false;
    private bool sceneAstralHasLoaded = false;
    private bool sceneSimulationHasLoaded = false;



    private void OnEnable()
    {

        EventHandler.AfterSceneLoadEvent += AfterSceneLoad;
        EventHandler.AdvanceGameDayEvent += AdvanceGameDay;


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


    private void InstantiateSceneItemsEachDay()
    {
        string currentSceneName = GetSceneString();

        for (int i = 0; i < itemPrefabs.prefabInstatiateList.Count; i++)
        {
            int numberToInstatiate = (int)Random.Range(itemPrefabs.prefabInstatiateList[i].minNumberToInstatiateEachDay, itemPrefabs.prefabInstatiateList[i].maxNumberToInstatiateEachDay);
            for (int j = 0; j < numberToInstatiate; j++)
            {
                InstatiateItem(itemPrefabs.prefabInstatiateList[i], currentSceneName);
            }
        }
    }

    private void InstantiateSceneItemsAtStartGame()
    {
        string currentSceneName = GetSceneString();

        //SceneName currentScene = SceneControllerManager.Instance.;
        for (int i = 0; i < itemPrefabs.prefabInstatiateList.Count; i++)
        {
            for (int j = 0; j < itemPrefabs.prefabInstatiateList[i].numberToInstatiateAtBeginning; j++)
            {
                InstatiateItem(itemPrefabs.prefabInstatiateList[i], currentSceneName);
            }
        }
    }

    private string GetSceneString()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        return currentScene.name;
    }

    private void InstatiateItem(PrefabInstatiateStruct obj, string currentSceneName)
    {

        if (ShouldBeCreatedInScene(obj, currentSceneName))
        {

            Vector3 itemPosition = GetItemPositionInScene(obj.sceneName);
            switch (obj.location)
            {
                case InstatiateLocation.space:
                    while (
    !CanPlaceItem(obj.prefab.GetComponent<Item>(), itemPosition) &&
    !IsThereAnItemAtTheLocation(obj.prefab, itemPosition)
    )
                    {
                        itemPosition = GetItemPositionInScene(obj.sceneName);
                    }
                    CreateItemInSpace(obj.prefab, itemPosition);
                    break;
                case InstatiateLocation.inventory:
                    InventoryManager.Instance.AddItem(InventoryLocation.player, obj.prefab.GetComponent<Item>());
                    break;
            }

        }
    }

    private bool IsThereAnItemAtTheLocation(GameObject prefab, Vector3 itemPosition)
    {
        Vector2 point = new Vector2(itemPosition.x, itemPosition.y);
        Vector2 size = GetSizeOfPrefab(prefab);

        Item[] items = HelperMethods.GetComponentsAtBoxLocationNonAlloc<Item>(1, point, size, 0); ;
        return false;
        //return items.Length > 0;
    }

    private Vector2 GetSizeOfPrefab(GameObject prefab)
    {
        BoxCollider2D collider = prefab.GetComponent<BoxCollider2D>();
        return collider.size;
    }

    private bool ShouldBeCreatedInScene(PrefabInstatiateStruct prefabInstatiateStruct, string sceneName)
    {
        //if(SceneControllerManager.Get)
        if(prefabInstatiateStruct.sceneName.ToString() == sceneName)
        {
            //Debug.Log(prefabInstatiateStruct.sceneName.ToString());
            return true;
        }

        //prefabInstatiateStruct 
        return false;
    }

    private GameObject CreateItemInSpace(GameObject prefab, Vector3 itemPosition)
    {
        return Instantiate(prefab, itemPosition, Quaternion.identity, parentItem);

    }

    private GameObject CreateItemInNPCs(GameObject prefab, Vector3 itemPosition)
    {
        return Instantiate(prefab, itemPosition, Quaternion.identity, npcItem);

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
                    case ItemType.NPC:
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
        InstantiateSceneItemsEachDay();
    }

    private void AfterSceneLoad()
    {

        parentItem = GameObject.FindGameObjectWithTag(Tags.ItemsParentTransform).transform;
        npcItem = GameObject.FindGameObjectWithTag(Tags.NPCParentTransform).transform;

        SceneName currentSceneName = SceneControllerManager.Instance.GetCurrentScene();
        switch (currentSceneName)
        {
            case SceneName.Scene_Bush:
                if (!sceneBushHasLoaded)
                {
                    LoadItemsAndNPCs();
                    sceneBushHasLoaded = true;
                }
                break;
            case SceneName.Scene_Astral:
                if (!sceneAstralHasLoaded)
                {
                    LoadItemsAndNPCs();
                    sceneAstralHasLoaded = true;
                }
                break;
            case SceneName.Scene_Simulator:
                if (!sceneSimulationHasLoaded)
                {
                    LoadItemsAndNPCs();
                    sceneSimulationHasLoaded = true;
                }
                break;
        }

    }

    private void LoadItemsAndNPCs()
    {
        InstantiateSceneItemsAtStartGame();
        InstatiateHumanoidNPCs();
    }


    public void InstatiateHumanoidNPC()
    {
        Vector2Int vector2 = PointOfInterestManager.Instance.GetCoordinateForPointOfInterest(PointOfInterestType.Exo_Womb);
            Vector3 pos = new Vector3(vector2.x, vector2.y);
            CreateItemInNPCs(humanNoidPrefab, pos);

    }

    public void InstatiateAshe(Vector2 pos)
    {
        float x = Random.Range(-2.0f, 2.0f);
        float y = Random.Range(-2.0f, 2.0f);
        Vector3 position = new Vector3(pos.x + x, pos.y + y, 0);

        CreateItemInSpace(ashePrefab, position);
    }

    public void InstatiatePlantEgg(Vector2 pos)
    {
        float x = Random.Range(-4.0f, 4.0f);
        float y = Random.Range(-4.0f, 4.0f);
        Vector3 position = new Vector3(pos.x + x, pos.y + y, 0);

        CreateItemInSpace(eggPrefab, position);
    }


    private void InstatiateHumanoidNPCs()
    {
        for (int i = 0; i < numberOfHumanoids; i++)
        {
            InstatiateHumanoidNPC();
        }
    }
}
