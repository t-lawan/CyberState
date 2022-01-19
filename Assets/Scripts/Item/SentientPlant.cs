using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentientPlant : MonoBehaviour
{
    private void OnEnable()
    {
        EventHandler.AdvanceGameMinuteEvent += UpdateGameTime;
    }



    private void OnDisable()
    {
        EventHandler.AdvanceGameMinuteEvent -= UpdateGameTime;

    }

    private void UpdateGameTime(int arg1, Season arg2, int arg3, string arg4, int arg5, int arg6, int arg7)
    {
        float val = UnityEngine.Random.Range(0.0f, 1.0f);

        if (val < 0.01f)
        {
            //Debug.Log("InstatiateAshe");
            InstatiateItems.Instance.InstatiateAshe(transform.position);
        } else if(val > 0.99f)
        {
            //Debug.Log("InstatiateEgg");
            InstatiateItems.Instance.InstatiatePlantEgg(transform.position);

        }


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
