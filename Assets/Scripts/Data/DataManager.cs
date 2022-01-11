using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingletonMonoBehaviour<DataManager>
{
    private int numberOfInteractions;
    private int numberOfSeedsPlanted;

    public void Awake()
    {
        numberOfInteractions = 0;
        numberOfSeedsPlanted = 0;
    }
    // Number of Collisions per minute
    // Number of Seeds planted per minute
    // Number of Ores collected per minute
    // Number of Births per minute
    // Number of Deaths per minute
    // Number of ores converted per minute

    public void OnEnable()
    {
        EventHandler.DataIncrementNumberOfInteractions += IncrementNumberOfInteractions;
        EventHandler.DataIncrementNumberOfSeedsPlanted += IncrementNumberOfSeedsPlanted;

    }

    public void OnDisable()
    {
        EventHandler.DataIncrementNumberOfInteractions -= IncrementNumberOfInteractions;
        EventHandler.DataIncrementNumberOfSeedsPlanted += IncrementNumberOfSeedsPlanted;


    }

    private void IncrementNumberOfInteractions()
    {
        numberOfInteractions++;
    }

    private void IncrementNumberOfSeedsPlanted()
    {
        numberOfSeedsPlanted++;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
