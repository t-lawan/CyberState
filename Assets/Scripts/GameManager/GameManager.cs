using System;
using UnityEngine;


public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public Weather currentWeather;
    private int numOfDaysPassedSinceChange = 0;
    private int maxNumberOfDays = 10;
    private SceneName scene;

    public void OnEnable()
    {
        EventHandler.AdvanceGameDayEvent += AdvanceGameDayEvent;
        EventHandler.AfterSceneLoadEvent += AfterSceneLoadEvent;
    }



    public void OnDisable()
    {
        EventHandler.AdvanceGameDayEvent -= AdvanceGameDayEvent;
        EventHandler.AfterSceneLoadEvent -= AfterSceneLoadEvent;


    }

    private void AdvanceGameDayEvent(int arg1, Season arg2, int arg3, string arg4, int arg5, int arg6, int arg7)
    {
        numOfDaysPassedSinceChange++;
        if(numOfDaysPassedSinceChange == maxNumberOfDays)
        {
            ChangeScene();
        } 
    }

    private void ChangeScene()
    {
        switch (scene)
        {
            case SceneName.Scene_Astral:
                SceneControllerManager.Instance.FadeAndLoadScene(SceneName.Scene_Simulator.ToString(), new Vector3());
                break;
            case SceneName.Scene_Bush:
                SceneControllerManager.Instance.FadeAndLoadScene(SceneName.Scene_Simulator.ToString(), new Vector3());
                break;
            case SceneName.Scene_Simulator:
                SceneControllerManager.Instance.FadeAndLoadScene(SceneName.Scene_Bush.ToString(), new Vector3());
                break;
        }

        numOfDaysPassedSinceChange = 0;
    }

    private void AfterSceneLoadEvent()
    {
        scene = SceneControllerManager.Instance.GetCurrentScene();
    }


    protected override void Awake()
    {
        base.Awake();

        //TODO: Need a resolution settings options screen
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow, 0);

        // Set starting weather
        currentWeather = Weather.dry;


    }


}
