using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egregore : MonoBehaviour
{
    public bool hasBeenTouched = false;



    public void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.gameObject.GetComponent<Item>();
        if(!hasBeenTouched && item != null && item.ItemCode == 10015)
        {
            hasBeenTouched = true;
            Scene_Astral.Instance.ActivateEgregore();
        }

    }

    //public void OnTriggerStay2D(Collider2D collision)
    //{
    //    Debug.Log(collision.gameObject.name);
    //    Debug.Log("Triggered STay");
    //}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
