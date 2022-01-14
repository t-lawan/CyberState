using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    [SerializeField] PointOfInterestType pointOfInterestTypeBush;
    // Start is called before the first frame update
    void Awake()
    {
        //InstatiateItems.Instance.
        PointOfInterestManager.Instance.AddPointOfInterest(pointOfInterestTypeBush, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
