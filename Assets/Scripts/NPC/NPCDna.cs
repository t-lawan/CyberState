using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDna : MonoBehaviour
{
    public float value;
    // Start is called before the first frame update
    void Awake()
    {
        value = Random.Range(0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
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
