using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataClass
{
    public int numberOfInteractions;
    public int numberOfSeedsPlanted;
    public int numberOfHumanoidsBorn;
    public int numberOfHumanoidsDead;
    public DataClass()
    {
        this.numberOfInteractions = 0;
        this.numberOfSeedsPlanted = 0;
        this.numberOfHumanoidsBorn = 0;
        this.numberOfHumanoidsDead = 0;

    }
    public void IncrementNumberOfInteractions()
    {
        numberOfInteractions++;
    }

    public void IncrementNumberOfSeedsPlanted()
    {
        numberOfSeedsPlanted++;
    }

    public void IncrementNumberOfHumansBorn()
    {
        numberOfHumanoidsBorn++;
    }

    public void IncrementNumberOfHumansDead()
    {
        numberOfHumanoidsDead++;
    }
}

public class DataManager : SingletonMonoBehaviour<DataManager>
{
    private DataClass bushData;
    private DataClass simulationData;
    private SceneName sceneName;

    public void Awake()
    {
        base.Awake();
        bushData = new DataClass();
        simulationData = new DataClass();
        simulationData.numberOfInteractions = 0;

    }
    // Number of Collisions per minute
    // Number of Seeds planted per minute
    // Number of Ores collected per minute
    // Number of Births per minute
    // Number of Deaths per minute
    // Number of ores converted per minute

    public void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += AfterSceneLoadEvent;

        EventHandler.DataIncrementNumberOfInteractions += IncrementNumberOfInteractions;
        EventHandler.DataIncrementNumberOfSeedsPlanted += IncrementNumberOfSeedsPlanted;
        EventHandler.NPCHasBeenBornEvent += IncrementNumberOfHumansBorn;
        EventHandler.NPCHasDiedEvent += IncrementNumberOfHumansDead;
    }

    public void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= AfterSceneLoadEvent;

        EventHandler.DataIncrementNumberOfInteractions -= IncrementNumberOfInteractions;
        EventHandler.DataIncrementNumberOfSeedsPlanted -= IncrementNumberOfSeedsPlanted;
        EventHandler.NPCHasBeenBornEvent -= IncrementNumberOfHumansBorn;
        EventHandler.NPCHasDiedEvent -= IncrementNumberOfHumansDead;
    }

    public string GetData(DataType dataType)
    {
        string text = "";
        switch (dataType)
        {
            case DataType.interactions:
                text = GetNumberOfInteractions().ToString();
                break;
            case DataType.births:
                text = GetNumberOfHumansBorn().ToString();

                break;
            case DataType.deaths:
                text = GetNumberOfHumansDead().ToString();

                break;
            case DataType.seeds_planted:
                text = GetNumberOfSeedsPlanted().ToString();

                break;
        }


        return text;
    }

    private int GetNumberOfInteractions()
    {
        int val = 0;
        switch (sceneName)
        {
            case SceneName.Scene_Bush:
                val = bushData.numberOfInteractions;
                break;
            case SceneName.Scene_Simulator:
                val = simulationData.numberOfInteractions;
                break;
        }

        return val;
    }

    private void IncrementNumberOfInteractions()
    {
        switch (sceneName)
        {
            case SceneName.Scene_Bush:
                bushData.IncrementNumberOfInteractions();
                break;
            case SceneName.Scene_Simulator:
                simulationData.IncrementNumberOfInteractions();
                break;
        }
    }


    private int GetNumberOfSeedsPlanted()
    {
        int val = 0;
        switch (sceneName)
        {
            case SceneName.Scene_Bush:
                val = bushData.numberOfSeedsPlanted;
                break;
            case SceneName.Scene_Simulator:
                val = simulationData.numberOfSeedsPlanted;
                break;
        }

        return val;
    }

    private void IncrementNumberOfSeedsPlanted()
    {
        switch (sceneName)
        {
            case SceneName.Scene_Bush:
                bushData.IncrementNumberOfSeedsPlanted();
                break;
            case SceneName.Scene_Simulator:
                simulationData.IncrementNumberOfSeedsPlanted();
                break;
        }
    }

    private int GetNumberOfHumansBorn()
    {
        int val = 0;
        switch (sceneName)
        {
            case SceneName.Scene_Bush:
                val = bushData.numberOfHumanoidsBorn;
                break;
            case SceneName.Scene_Simulator:
                val = simulationData.numberOfHumanoidsBorn;
                break;
        }

        return val;
    }

    private void IncrementNumberOfHumansBorn(string id)
    {
        switch (sceneName)
        {
            case SceneName.Scene_Bush:
                bushData.IncrementNumberOfHumansBorn();
                break;
            case SceneName.Scene_Simulator:
                simulationData.IncrementNumberOfHumansBorn();
                break;
        }
    }

    private int GetNumberOfHumansDead()
    {
        int val = 0;
        switch (sceneName)
        {
            case SceneName.Scene_Bush:
                val = bushData.numberOfHumanoidsDead;
                break;
            case SceneName.Scene_Simulator:
                val = simulationData.numberOfHumanoidsDead;
                break;
        }

        return val;
    }
    private void IncrementNumberOfHumansDead(string id)
    {
        switch (sceneName)
        {
            case SceneName.Scene_Bush:
                bushData.IncrementNumberOfHumansDead();
                break;
            case SceneName.Scene_Simulator:
                simulationData.IncrementNumberOfHumansDead();
                break;
        }
    }

    private void AfterSceneLoadEvent()
    {
        sceneName = SceneControllerManager.Instance.GetCurrentScene();
    }
}
