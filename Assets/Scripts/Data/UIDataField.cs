using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIDataField : MonoBehaviour
{
    [SerializeField] private DataType dataType;
    [SerializeField] private TextMeshProUGUI text =null;

    public void Awake()
    {
        UpdateData();
    }
    //public void OnEnable()
    //{

    //    EventHandler.DataIncrementNumberOfInteractions += UpdateData;
    //    EventHandler.DataIncrementNumberOfSeedsPlanted += UpdateData;
    //    EventHandler.NPCHasBeenBornEvent += UpdateData;
    //    EventHandler.NPCHasDiedEvent += UpdateData;
    //}

    //public void OnDisable()
    //{

    //    EventHandler.DataIncrementNumberOfInteractions -= UpdateData;
    //    EventHandler.DataIncrementNumberOfSeedsPlanted -= UpdateData;
    //    EventHandler.NPCHasBeenBornEvent -= UpdateData;
    //    EventHandler.NPCHasDiedEvent -= UpdateData;
    //}

    // Update is called once per frame
    void Update()
    {
    }

    void UpdateData(string i)
    {
        UpdateData();
    }

    void UpdateData()
    {
        switch (dataType)
        {
            case DataType.interactions:
                text.SetText(DataManager.Instance.GetData(dataType));
                break;
            case DataType.births:
                text.SetText(DataManager.Instance.GetData(dataType));

                break;
            case DataType.deaths:
                text.SetText(DataManager.Instance.GetData(dataType));

                break;
            case DataType.seeds_planted:
                text.SetText(DataManager.Instance.GetData(dataType));

                break;
        }
    }
}
