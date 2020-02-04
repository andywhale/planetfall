using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeScript : MonoBehaviour
{
    [SerializeField] GameObject[] walls = new GameObject[4];
    [SerializeField] GameObject outerWall;
    [SerializeField] GameObject outerWallCorner = null;
    [SerializeField] GameObject player;
    [SerializeField] GameObject goal = null;
    [SerializeField] GameObject floor;
    [SerializeField] GameObject roof = null;
    [SerializeField] GameObject start = null;
    [SerializeField] GameObject powerUp = null;
    [SerializeField] GameObject enemy = null;
    [SerializeField] GameObject move = null;
    string[] activeWalls;
    int wallCount;
    float wallLength = 1.0f;
    private float initialYPos = 0f;
    const float CORNERHEIGHT = -0.384f;
    const float FLOORHEIGHT = -0.41f;
    const float ROOFHEIGHT = 0.4f;
    int xSize = 3;
    int ySize = 3;
    Vector3 initialPos;
    GameObject wallHolder;
    GameManager gameManager;
    GameObject[] enemies = new GameObject[8];
    Cell[] cells;
    private int currentCell = 12;
    private int totalCells = 0;
    private int visitedCells = 0;
    private bool startedBuilding = false;
    private int currentNeighbour = 0;
    private List<int> lastCells;
    private int backingUp = 0;
    string wallToBreak;
    bool teleporterSwitchedOff = false;


    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        xSize = gameManager.GetNextLevelXSize();
        ySize = gameManager.GetNextLevelYSize();
        CreateWalls();
        CreateFloor();
        CreateRoof();
    }

    private void FixedUpdate()
    {
        if (gameManager.GameIsOver())
        {
            if (!teleporterSwitchedOff)
            {
                Destroy(goal);
                goal = Instantiate(start, new Vector3(initialPos.x + (xSize * wallLength) - wallLength, FLOORHEIGHT, initialPos.z + (ySize * wallLength) - (wallLength * 1.5f)), Quaternion.identity) as GameObject;
                teleporterSwitchedOff = true;
            }
        }
        else
        {
            teleporterSwitchedOff = false;
        }
    }

    void AddPowerUp()
    {
        if (powerUp != null)
        {
            powerUp = Instantiate(powerUp, new Vector3(initialPos.x + (Random.Range(0, xSize) * wallLength) / 2, 0f, initialPos.z + (Random.Range(0, ySize) * wallLength) / 2), Quaternion.identity) as GameObject;
        }
    }

    void AddEnemies()
    {
        if (enemy != null)
        {
            int numberOfEnemies = Mathf.RoundToInt(gameManager.GetCurrentLevel() / 3);
            if (numberOfEnemies > enemies.Length)
                numberOfEnemies = enemies.Length;
            for (int i = 0; i <= numberOfEnemies; i++)
            {
                enemies[i] = Instantiate(enemy, new Vector3(initialPos.x + (Random.Range(0, xSize) * wallLength / 2), FLOORHEIGHT, initialPos.z + (Random.Range(0, ySize) * wallLength / 2)), Quaternion.identity) as GameObject;
            }
        }
    }

    void CreateWalls()
    {
        activeWalls = new string[xSize * ySize * 4];
        wallHolder = new GameObject();
        wallHolder.name = "Maze";

        initialPos = new Vector3((-xSize / 2) + wallLength / 2, initialYPos, (-ySize / 2) + wallLength / 2);
        Vector3 myPos = initialPos;
        GameObject tempWall;
        GameObject wallType;

        for (int i = 0; i < ySize; i++)
        {
            for (int j = 0; j <= xSize; j++)
            {
                if (j == 0 || j == xSize)
                    wallType = outerWall;
                else
                    wallType = walls[Random.Range(0, walls.Length)];
                myPos = new Vector3(initialPos.x + (j * wallLength) - wallLength / 2, initialYPos, initialPos.z + (i * wallLength) - wallLength / 2);
                Quaternion wallRotation = Quaternion.identity;
                if (j == xSize)
                    wallRotation = Quaternion.Euler(0.0f, 180f, 0.0f);
                tempWall = Instantiate(wallType, myPos, wallRotation) as GameObject;
                tempWall.transform.parent = wallHolder.transform;
                tempWall.name = "Wall_" + i + "_" + j + "_R";
                activeWalls[wallCount] = tempWall.name;
                wallCount++;
            }
        }

        for (int i = 0; i <= ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                if (i == 0 || i == ySize)
                    wallType = outerWall;
                else
                    wallType = walls[Random.Range(0, walls.Length)];
                myPos = new Vector3(initialPos.x + (j * wallLength), initialYPos, initialPos.z + (i * wallLength) - wallLength);
                Quaternion wallRotation = Quaternion.Euler(0.0f, 90f, 0.0f);
                if (i == 0)
                    wallRotation = Quaternion.Euler(0.0f, 270f, 0.0f);
                tempWall = Instantiate(wallType, myPos, wallRotation) as GameObject;
                tempWall.transform.parent = wallHolder.transform;
                tempWall.name = "Wall_" + i + "_" + j;
                activeWalls[wallCount] = tempWall.name;
                wallCount++;
            }
        }

        CreateCells();
    }

    void CreateCorners()
    {
        if (outerWallCorner != null)
        {
            // Top right
            Instantiate(outerWallCorner, new Vector3(initialPos.x - wallLength / 2, CORNERHEIGHT, initialPos.z - wallLength), Quaternion.identity);
            // Top left
            Instantiate(outerWallCorner, new Vector3(initialPos.x + (xSize * wallLength - wallLength / 2), CORNERHEIGHT, initialPos.z - wallLength), Quaternion.Euler(0.0f, 270f, 0.0f));
            // Bottom right
            Instantiate(outerWallCorner, new Vector3(initialPos.x - wallLength / 2, CORNERHEIGHT, initialPos.z + (ySize * wallLength - wallLength)), Quaternion.Euler(0.0f, 90f, 0.0f));
            // Bottom left
            Instantiate(outerWallCorner, new Vector3(initialPos.x + (xSize * wallLength - wallLength / 2), CORNERHEIGHT, initialPos.z + (ySize * wallLength - wallLength)), Quaternion.Euler(0.0f, 180f, 0.0f));
        }
    }

    void CreateFloor()
    {
        if (floor != null)
        {
            floor = Instantiate(floor, new Vector3(initialPos.x + ((xSize * wallLength - wallLength) / 2), FLOORHEIGHT, initialPos.z + ((ySize * wallLength) / 2 - wallLength)), Quaternion.identity);
            Transform floorTransform = floor.GetComponent<Transform>();
            floorTransform.localScale = new Vector3((xSize * wallLength) / 10, floorTransform.localScale.y, (ySize * wallLength) / 10);
        }
    }

    void CreateRoof()
    {
        if (roof != null)
        {
            roof = Instantiate(roof, new Vector3(initialPos.x + ((xSize * wallLength - wallLength) / 2), 0.4f, initialPos.z + ((ySize * wallLength) / 2 - wallLength)), Quaternion.identity);
            Transform roofTransform = roof.GetComponent<Transform>();
            roofTransform.localScale = new Vector3((xSize * wallLength) / 10, roofTransform.localScale.y, (ySize * wallLength) / 10);
        }
    }

    void CreateCells()
    {
        lastCells = new List<int>();
        lastCells.Clear();
        cells = new Cell[xSize * ySize];

        int children = wallHolder.transform.childCount;
        GameObject[] allWalls = new GameObject[children];
        int eastWestProcess = 0;
        int childProcess = 0;
        int termCount = 0;

        for (int i = 0; i < children; i++)
        {
            allWalls[i] = wallHolder.transform.GetChild(i).gameObject;
        }

        for (int cellProcess = 0; cellProcess < cells.Length; cellProcess++)
        {
            cells[cellProcess] = new Cell();
            cells[cellProcess].SetWest(allWalls[eastWestProcess]);
            cells[cellProcess].SetSouth(allWalls[childProcess + (xSize + 1) * ySize]);

            if (termCount == xSize)
            {
                eastWestProcess += 2;
                termCount = 0;
            }
            else
                eastWestProcess++;

            termCount++;
            childProcess++;

            cells[cellProcess].SetEast(allWalls[eastWestProcess]);
            GameObject northWall = allWalls[(childProcess + (xSize + 1) * ySize) + xSize - 1];
            cells[cellProcess].SetNorth(northWall);
            cells[cellProcess].SetJumpPoint(Instantiate(move, new Vector3(northWall.transform.position.x, 0, northWall.transform.position.z - (wallLength / 2)), Quaternion.identity));
        }

        totalCells = xSize * ySize;

        CreateMaze();
    }

    void CreateMaze()
    {
        string[] removedWalls = new string[xSize * ySize];
        while (visitedCells < totalCells)
        {
            if (startedBuilding)
            {
                FindNeighbour();
                if (cells[currentNeighbour].NotVisited() && cells[currentCell].IsVisited())
                {
                    // Destroy the neighbouring wall.
                    GameObject wallToDestroy = cells[currentCell].GetWallToBreak(wallToBreak);
                    RemoveActiveWallByName(wallToDestroy.name);
                    Destroy(wallToDestroy);
                    // Mark the neighbour as visited.
                    cells[currentNeighbour].Visited();
                    visitedCells++;
                    // Add the current cell to the list of visited cells.
                    lastCells.Add(currentCell);
                    // Move to the neighbour.
                    currentCell = currentNeighbour;
                    // If all cells are visited then back-up.
                    if (lastCells.Count > 0)
                    {
                        backingUp = lastCells.Count - 1;
                    }
                }
            }
            else
            {
                currentCell = Random.Range(0, totalCells);
                // myPos = new Vector3(initialPos.x + (j * wallLength) - wallLength / 2, 0.0f, initialPos.z + (i * wallLength) - wallLength / 2);
                player = Instantiate(player, new Vector3(initialPos.x + wallLength / 2, initialPos.y, initialPos.z - wallLength / 2), Quaternion.identity);
                cells[currentCell].Visited();
                visitedCells++;
                startedBuilding = true;
                goal = Instantiate(goal, new Vector3(initialPos.x + (xSize * wallLength) - wallLength, FLOORHEIGHT, initialPos.z + (ySize * wallLength) - (wallLength * 1.5f)), Quaternion.identity) as GameObject;
                goal.name = "Goal";
                start = Instantiate(start, new Vector3(initialPos.x + (1 * wallLength) - wallLength, FLOORHEIGHT, initialPos.z + (1 * wallLength) - (wallLength * 1.5f)), Quaternion.identity) as GameObject;
            }
        }

        for (int i = 0; i < cells.Length; i++)
        {
            Debug.Log("Cell: " + i);
            if (IsACorridor(cells[i]))
            {
                Destroy(cells[i].GetJumpPoint());
            }
        }
    }

    public bool IsACorridor(Cell cell)
    {
        bool northExists = ActiveWallExists(cell.getNorthName());
        Debug.Log("N:" + northExists);
        bool southExists = ActiveWallExists(cell.getSouthName());
        Debug.Log("S:" + southExists);
        bool westExists = ActiveWallExists(cell.getWestName());
        Debug.Log("W:" + westExists);
        bool eastExists = ActiveWallExists(cell.getEastName());
        Debug.Log("E:" + eastExists);
        Debug.Log((northExists && southExists && !eastExists && !westExists) ||
            (!northExists && !southExists && eastExists && westExists));
        return ((northExists && southExists && !eastExists && !westExists) ||
            (!northExists && !southExists && eastExists && westExists));
    }

    public void RemoveActiveWallByName(string wallName)
    {
        string[] tempActiveWalls = new string[activeWalls.Length];
        int i = 0;
        foreach (string x in activeWalls)
        {
            if (wallName != x)
            {
                tempActiveWalls[i] = x;
                i++;
            }
        }
        activeWalls = tempActiveWalls;
    }

    public bool ActiveWallExists(string wallName)
    {
        Debug.Log("Checking: " + wallName);
        foreach (string x in activeWalls)
        {
            if (wallName == x)
            {
                return true;
            }
        }
        return false;
    }

    void FindNeighbour()
    {
        int length = 0;
        int[] neighbours = new int[4];
        int currentRowLastCell = 0;
        currentRowLastCell = ((currentCell + 1) / xSize);
        currentRowLastCell -= 1;
        currentRowLastCell *= xSize;
        currentRowLastCell += xSize;

        string[] wallsToBreak = new string[4];

        // Break the East Wall
        // If the next cell is less than the total cells and its not the last on the row
        if (currentCell + 1 < totalCells && (currentCell + 1) != currentRowLastCell)
        {
            if (cells[currentCell + 1].NotVisited())
            {
                neighbours[length] = currentCell + 1;
                wallsToBreak[length] = "East";
                length++;
            }
        }

        // Break the West Wall
        // If the previous cell is greater than 0 and not the last cell on the previous row
        if (currentCell - 1 >= 0 && currentCell != currentRowLastCell)
        {
            if (cells[currentCell - 1].NotVisited())
            {
                neighbours[length] = currentCell - 1;
                wallsToBreak[length] = "West";
                length++;
            }
        }

        // Break the North Wall
        // If the current cell + the length of a row is not greater than the total cells
        if (currentCell + xSize < totalCells)
        {
            if (cells[currentCell + xSize].NotVisited())
            {
                neighbours[length] = currentCell + xSize;
                wallsToBreak[length] = "North";
                length++;
            }
        }

        // Break the South Wall
        // If the current cell - the length of a row is not less than 0
        if (currentCell - xSize >= 0)
        {
            if (cells[currentCell - xSize].NotVisited())
            {
                neighbours[length] = currentCell - xSize;
                wallsToBreak[length] = "South";
                length++;
            }
        }

        // Pick one of the available walls to break at random and break it
        // and then move into it
        if (length > 0)
        {
            int nextCell = Random.Range(0, length);
            currentNeighbour = neighbours[nextCell];
            wallToBreak = wallsToBreak[nextCell];
        }
        else
        {
            if (backingUp > 0)
            {
                currentCell = lastCells[backingUp];
                backingUp--;
            }
        }

    }
}
