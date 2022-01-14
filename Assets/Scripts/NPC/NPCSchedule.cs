using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NPCPath))]
public class NPCSchedule : MonoBehaviour
{
    [SerializeField] private SO_NPCScheduleEventList so_NPCScheduleEventList = null;
    [SerializeField] private SO_NPCScheduleEventList[] so_NPCScheduleEventList_Bush = null;
    [SerializeField] private SO_NPCScheduleEventList[] so_NPCScheduleEventList_Simulation = null;
    private SortedSet<NPCScheduleEvent> npcScheduleEventSet;
    private NPCPath npcPath;

    private void Awake()
    {
        // Load NPC schedule event list into a sorted set
        LoadScheduleEvents();
        // Get NPC Path Component
        npcPath = GetComponent<NPCPath>();

    }

    private void LoadScheduleEvents()
    {
        npcScheduleEventSet = new SortedSet<NPCScheduleEvent>(new NPCScheduleEventSort());
        SceneName sceneName = SceneControllerManager.Instance.GetCurrentScene();

        SO_NPCScheduleEventList eventList = so_NPCScheduleEventList;
        switch (sceneName)
        {
            case SceneName.Scene_Bush:
                int index = Random.Range(0, so_NPCScheduleEventList_Bush.Length);
                eventList = so_NPCScheduleEventList_Bush[index];
                break;
            case SceneName.Scene_Simulator:
                int ind = Random.Range(0, so_NPCScheduleEventList_Simulation.Length);
                //Debug.Log(so_NPCScheduleEventList_Simulation.Length);
                eventList = so_NPCScheduleEventList_Simulation[ind];
                break;
        }

        if(eventList != null)
        {
            foreach (NPCScheduleEvent npcScheduleEvent in eventList.npcScheduleEventList)
            {
                npcScheduleEventSet.Add(npcScheduleEvent);
            }
        }



    }

    private void OnEnable()
    {
        EventHandler.AdvanceGameMinuteEvent += GameTimeSystem_AdvanceMinute;
        EventHandler.AdvanceGameDayEvent += AdvanceGameDayEvent;
    }

    private void OnDisable()
    {
        EventHandler.AdvanceGameMinuteEvent -= GameTimeSystem_AdvanceMinute;
        EventHandler.AdvanceGameDayEvent -= AdvanceGameDayEvent;

    }

    private void AdvanceGameDayEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        LoadScheduleEvents();
    }

    private void GameTimeSystem_AdvanceMinute(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        int time = (gameHour * 100) + gameMinute;
        //Attempt to get matching schedule

        NPCScheduleEvent matchingNPCScheduleEvent = null;

        foreach (NPCScheduleEvent npcScheduleEvent in npcScheduleEventSet)
        {
            //Debug.Log(npcScheduleEvent.ToString());

            if (npcScheduleEvent.Time == time)
            {
                // Time match now check if parameters match
                if (npcScheduleEvent.day != 0 && npcScheduleEvent.day != gameDay)
                    continue;

                if (npcScheduleEvent.season != Season.none && npcScheduleEvent.season != gameSeason)
                    continue;

                if (npcScheduleEvent.weather != Weather.none && npcScheduleEvent.weather != GameManager.Instance.currentWeather)
                    continue;

                //Debug.Log(npcScheduleEvent.ToString());
                //Schdule matches
                // Debug.Log("Schedule Matches! " + npcScheduleEvent);
                matchingNPCScheduleEvent = npcScheduleEvent;
                break;
            }
            else if (npcScheduleEvent.Time > time)
            {
                break;
            }
        }

        // Now test is matchingSchedule!=null and do something;
        if (matchingNPCScheduleEvent != null)
        {
            //Debug.Log("BUILDING");
            // Build path for matching schedule
            npcPath.BuildPath(matchingNPCScheduleEvent);
        }
    }
}