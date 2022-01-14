using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDna : MonoBehaviour
{
    public float value;
    public float currentAge;
    public bool isDead = false;
    public void OnEnable()
    {
        EventHandler.AdvanceGameDayEvent += AdvanceGameDayEvent;
    }

    public void OnDisable()
    {
        EventHandler.AdvanceGameDayEvent -= AdvanceGameDayEvent;
    }

    private void AdvanceGameDayEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        currentAge = currentAge - 1.0f;
    }

    // Start is called before the first frame update
    void Awake()
    {
        value = Random.Range(0.0f, 1.0f);
        currentAge = Mathf.Lerp(Settings.minAge, Settings.maxAge, value);
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentAge < 0)
        {
            isDead = true;
        } 
    }

   public void Mutate()
    {
        float difference = Random.Range(-0.1f, 0.1f);
        value = value + difference;
        value = Mathf.Clamp(value, 0.0f, 1.0f);
    }

    //NPCDna(float val1, float val2)
    //{
    //    value = Random.Range(val1, val2);

    //}
}
