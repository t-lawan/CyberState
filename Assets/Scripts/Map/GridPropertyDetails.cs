﻿[System.Serializable]
public class GridPropertyDetails
{
    public int gridX;
    public int gridY;
    public bool isDiggable = false;
    public bool canDropItem = false;
    public bool canPlaceBuilding = false;
    public bool isPath = false;
    public bool isNPCObstacle = false;

    public int daySinceDug = -1;
    public int daySinceWatered = -1;
    public int seedItemCode = -1;
    public int growthDays = -1;
    public int daysSinceLastHarvest = -1;

    public GridPropertyDetails()
    {

    }

}
