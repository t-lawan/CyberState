using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScheduleManager : SingletonMonoBehaviour<NPCScheduleManager>
{
    List<string> npcIDs;
    //Dictionary<PointOfInterestType_Bush, V>
    protected override void Awake()
    {
        base.Awake();

        npcIDs = new List<string>();
    }
    public void OnEnable()
    {
        EventHandler.NPCHasBeenBornEvent += NPCHasBeenBornEvent;
        EventHandler.NPCHasDiedEvent += NPCHasDiedEvent;
    }

    public void OnDisable()
    {
        EventHandler.NPCHasBeenBornEvent -= NPCHasBeenBornEvent;
        EventHandler.NPCHasDiedEvent -= NPCHasDiedEvent;


    }

    private void NPCHasBeenBornEvent()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NPCHasBeenBornEvent(string id)
    {
        npcIDs.Add(id);
    }

    public void NPCHasDiedEvent(string id)
    {
        npcIDs.Remove(id);
    }
}
