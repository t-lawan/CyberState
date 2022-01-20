using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PurchaseItemStruct
{
    public int itemCode;
    public int cost;
    public Sprite sprite;
}

public class UIPurchasePanel : MonoBehaviour
{
    [SerializeField] private GameObject uiPurchaseObjectPrefab;
    [SerializeField] public PurchaseItemStruct[] bushSprites;
    [SerializeField] public PurchaseItemStruct[] simulationSprites;
    private SceneName sceneName;


    void Awake()
    {
        sceneName = SceneControllerManager.Instance.GetCurrentScene();

    }
    // Start is called before the first frame update
    void Start()
    {
        if (sceneName == SceneName.Scene_Bush)
        {
            for (int i = 0; i < bushSprites.Length; i++)
            {

            }
        }
        Instantiate(uiPurchaseObjectPrefab, new Vector3(), Quaternion.identity, transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
