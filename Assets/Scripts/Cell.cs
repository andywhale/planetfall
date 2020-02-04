using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell
{
    bool visited = false;
    GameObject north;
    GameObject south;
    GameObject east;
    GameObject west;
    GameObject jumpPoint;

    public void SetNorth(GameObject allocatedNorth)
    {
        north = allocatedNorth;
    }

    public GameObject GetNorthWall()
    {
        return north;
    }

    public void SetSouth(GameObject allocatedSouth)
    {
        south = allocatedSouth;
    }

    public void SetEast(GameObject allocatedEast)
    {
        east = allocatedEast;
    }

    public void SetWest(GameObject allocatedWest)
    {
        west = allocatedWest;
    }

    public void SetJumpPoint(GameObject allocatedJumpPoint)
    {
        jumpPoint = allocatedJumpPoint;
    }

    public GameObject GetJumpPoint()
    {
        return jumpPoint;
    }

    public bool IsVisited()
    {
        return visited;
    }

    public bool NotVisited()
    {
        return !visited;
    }

    public void Visited()
    {
        visited = true;
    }

    public GameObject GetWallToBreak(string wallToBreak)
    {
        switch (wallToBreak)
        {
            case "North": return (north);
            case "South": return (south);
            case "East": return (east);
            default: return (west);
        }
    }

    public string getNorthName()
    {
        return north.name;
    }

    public string getSouthName()
    {
        return south.name;
    }

    public string getEastName()
    {
        return east.name;
    }

    public string getWestName()
    {
        return west.name;
    }

}