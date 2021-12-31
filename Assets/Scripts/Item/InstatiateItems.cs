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
    [SerializeField] public int numOfPrefabs = 20;
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

                GameObject itemGameObject = Instantiate(itemPrefabs.prefabInstatiateList[i].prefab, itemPosition, Quaternion.identity, parentItem);
            }

            //Item item = itemGameObject.GetComponent<Item>();

        }

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

            //Item item = itemGameObject.GetComponent<Item>();

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
