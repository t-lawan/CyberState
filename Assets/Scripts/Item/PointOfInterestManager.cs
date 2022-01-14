using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterestManager : SingletonMonoBehaviour<PointOfInterestManager>
{
    public Dictionary<PointOfInterestType, List<Vector3>> pointsOfInterest;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        pointsOfInterest = new Dictionary<PointOfInterestType, List<Vector3>>();
    }

    public void AddPointOfInterest(PointOfInterestType pointOfInterestType, Vector3 position)
    {
        AddPointOfInterestInBush(pointOfInterestType, position);
    }

    //public void AddPointOfInterestInBush(PointOfInterestType_Bush pointOfInterestType, Vector3 position)
    //{
    //    // If it doesn't already exist
    //    if (!pointsOfInterestInBush.ContainsKey(pointOfInterestType))
    //    {
    //        // Createe empty List
    //        List<Vector3> vectors = new List<Vector3>();
    //        vectors.Add(position);
    //        pointsOfInterestInBush.Add(pointOfInterestType, vectors);
    //    }
    //    else
    //    {
    //        List<Vector3> vectors;
    //        if (pointsOfInterestInBush.TryGetValue(pointOfInterestType, out vectors))
    //        {
    //            vectors.Add(position);
    //            pointsOfInterestInBush[pointOfInterestType] = vectors;
    //        }
    //    }
    //}

    public void AddPointOfInterestInBush(PointOfInterestType pointOfInterestType, Vector3 position)
    {
        
        // If it doesn't already exist
        if (!pointsOfInterest.ContainsKey(pointOfInterestType))
        {
            // Createe empty List
            List<Vector3> vectors = new List<Vector3>();
                vectors.Add(position);
            pointsOfInterest.Add(pointOfInterestType, vectors);
        }
        else
        {
            List<Vector3> vectors;
            if(pointsOfInterest.TryGetValue(pointOfInterestType, out vectors))
            {
                vectors.Add(position);
                pointsOfInterest[pointOfInterestType] = vectors;
            }
        }
    }

    public Vector2Int GetCoordinateForPointOfInterest(PointOfInterestType pointOfInterestType)
    {
        if (pointsOfInterest.ContainsKey(pointOfInterestType))
        {
            List<Vector3> vectors;

            pointsOfInterest.TryGetValue(pointOfInterestType, out vectors);
            int index = Random.Range(0, vectors.Count);
            Vector3 pos = vectors[index];
            int x = Random.Range(-3, 3) + (int)pos.x;
            int y = Random.Range(-2, 2) +(int) pos.y;
            Vector2Int vector2Int = new Vector2Int(x,y);
            return vector2Int;
        }
        return new Vector2Int(0, 0);

    }
}