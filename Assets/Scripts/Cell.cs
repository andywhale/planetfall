using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    bool visited = false;
    GameObject north;
    GameObject south;
    GameObject east;
    GameObject west;

    public void SetNorth(GameObject allocatedNorth)
    {
        north = allocatedNorth;
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
            case "North": return (north); break;
            case "South": return (south); break;
            case "East": return (east); break;
            default: return (west); break;
        }
    }

}