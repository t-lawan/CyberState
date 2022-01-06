using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egregore : MonoBehaviour
{
    public bool hasBeenTouched = false;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        Debug.Log("Collided");
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        Debug.Log("Triggered");
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
