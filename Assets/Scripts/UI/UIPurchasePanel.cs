using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private PurchaseItemStruct[] sprites;
    private SceneName sceneName;
    private int asheItemCode = 10019;


    void Awake()
    {
        SetCurrentScene();
    }
    // Start is called before the first frame update
    void Start()
    {
            SetCurrentScene();

            for (int i = 0; i < sprites.Length; i++)
            {
                GameObject prefab = Instantiate(uiPurchaseObjectPrefab, new Vector3(), Quaternion.identity, transform);

                Image img = prefab.GetComponentInChildren<Image>();
                img.sprite = bushSprites[i].sprite;

                Button button = prefab.GetComponentInChildren<Button>();

                TextMeshProUGUI costText =  prefab.GetComponentInChildren<TextMeshProUGUI>();
                costText.text = bushSprites[i].cost.ToString() + " ashe";

                int itemCode = bushSprites[i].itemCode;
                button.onClick.AddListener(() => OnClick(itemCode));
            }
    }

    private void OnClick(int itemCode)
    {
        int numberOfCoins = InventoryManager.Instance.FindItemInInventory(InventoryLocation.player, asheItemCode); // Ashe
        if (numberOfCoins > 0)
        {
            InventoryManager.Instance.AddItem(InventoryLocation.player, itemCode);

            Debug.Log(numberOfCoins);
            InventoryManager.Instance.RemoveItem(InventoryLocation.player, asheItemCode);
        }


    }

    // Update is called once per frame
    private void SetCurrentScene()
    {
        sceneName = SceneControllerManager.Instance.GetCurrentScene();
        Debug.Log(sceneName.ToString());
        switch (sceneName)
        {
            case SceneName.Scene_Astral:
                sprites = simulationSprites;
                break;
            case SceneName.Scene_Bush:
                sprites = bushSprites;

                break;
            case SceneName.Scene_Simulator:
                sprites = simulationSprites;
                break;
        }
    }
}
