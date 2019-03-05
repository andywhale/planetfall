using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeScript : MonoBehaviour
{
    [SerializeField] GameObject[] walls = new GameObject[4];
    [SerializeField] GameObject outerWall;
    [SerializeField] GameObject outerWallCorner;
    [SerializeField] GameObject player;
    [SerializeField] GameObject goal;
    [SerializeField] GameObject floor;
    [SerializeField] GameObject roof;
    [SerializeField] GameObject start;
    [SerializeField] GameObject powerUp;
    float wallLength = 1.0f;
    private float initialYPos = 0f;
    const float CORNERHEIGHT = -0.384f;
    int xSize = 3;
    int ySize = 3;
    Vector3 initialPos;
    GameObject wallHolder;
    GameObject gameManager;
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
        gameManager = GameObject.Find("GameManager");
        xSize = gameManager.GetComponent<GameManager>().GetNextLevelXSize();
        ySize = gameManager.GetComponent<GameManager>().GetNextLevelYSize();
        CreateWalls();
        AddPowerUp();
    }

    private void FixedUpdate()
    {
        if (gameManager.GetComponent<GameManager>().GameIsOver())
        {
            if (!teleporterSwitchedOff)
            {
                Destroy(goal);
                goal = Instantiate(start, new Vector3(initialPos.x + (xSize * wallLength) - wallLength, -0.4f, initialPos.z + (ySize * wallLength) - (wallLength * 1.5f)), Quaternion.identity) as GameObject;
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
        powerUp = Instantiate(powerUp, new Vector3(initialPos.x + (Random.Range(0, xSize) * wallLength), 0f, initialPos.z + (Random.Range(0, ySize) * wallLength)), Quaternion.identity) as GameObject;
    }

    void CreateWalls()
    {
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
            }
        }

        // Top right
        Instantiate(outerWallCorner, new Vector3(initialPos.x - wallLength / 2, CORNERHEIGHT, initialPos.z - wallLength), Quaternion.identity);
        // Top left
        Instantiate(outerWallCorner, new Vector3(initialPos.x + (xSize * wallLength - wallLength / 2), CORNERHEIGHT, initialPos.z - wallLength), Quaternion.Euler(0.0f, 270f, 0.0f));
        // Bottom right
        Instantiate(outerWallCorner, new Vector3(initialPos.x - wallLength / 2, CORNERHEIGHT, initialPos.z + (ySize * wallLength - wallLength)), Quaternion.Euler(0.0f, 90f, 0.0f));
        // Bottom left
        Instantiate(outerWallCorner, new Vector3(initialPos.x + (xSize * wallLength - wallLength / 2), CORNERHEIGHT, initialPos.z + (ySize * wallLength - wallLength)), Quaternion.Euler(0.0f, 180f, 0.0f));


        floor = Instantiate(floor, new Vector3(initialPos.x + ((xSize * wallLength - wallLength)/2), -0.4f, initialPos.z + ((ySize * wallLength) / 2 - wallLength)), Quaternion.identity);
        Transform floorTransform = floor.GetComponent<Transform>();
        floorTransform.localScale = new Vector3((xSize * wallLength) / 10, floorTransform.localScale.y, (ySize * wallLength) / 10);

        roof = Instantiate(roof, new Vector3(initialPos.x + ((xSize * wallLength - wallLength) / 2), 0.4f, initialPos.z + ((ySize * wallLength) / 2 - wallLength)), Quaternion.identity);
        Transform roofTransform = roof.GetComponent<Transform>();
        roofTransform.localScale = new Vector3((xSize * wallLength) / 10, floorTransform.localScale.y, (ySize * wallLength) / 10);

        CreateCells();
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
            cells[cellProcess].SetNorth(allWalls[(childProcess + (xSize + 1) * ySize) + xSize - 1]);
        }

        totalCells = xSize * ySize;

        CreateMaze();
    }

    void CreateMaze()
    {
        while (visitedCells < totalCells)
        {
            if (startedBuilding)
            {
                FindNeighbour();
                if (cells[currentNeighbour].NotVisited() && cells[currentCell].IsVisited())
                {
                    BreakWall();
                    cells[currentNeighbour].Visited();
                    visitedCells++;
                    lastCells.Add(currentCell);
                    currentCell = currentNeighbour;
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
                goal = Instantiate(goal, new Vector3(initialPos.x + (xSize * wallLength) - wallLength, -0.4f, initialPos.z + (ySize * wallLength) - (wallLength * 1.5f)), Quaternion.identity) as GameObject;
                goal.name = "Goal";
                start = Instantiate(start, new Vector3(initialPos.x + (1 * wallLength) - wallLength, -0.4f, initialPos.z + (1 * wallLength) - (wallLength * 1.5f)), Quaternion.identity) as GameObject;
            }
        }
    }

    void BreakWall()
    {
        Destroy(cells[currentCell].GetWallToBreak(wallToBreak));
    }

    void FindNeighbour()
    {
        int length = 0;
        int[] neighbours = new int[4];
        int check = 0;
        check = ((currentCell + 1) / xSize);
        check -= 1;
        check *= xSize;
        check += xSize;
        string[] wallsToBreak = new string[4];

        // East
        if (currentCell + 1 < totalCells && (currentCell + 1) != check)
        {
            if (cells[currentCell + 1].NotVisited())
            {
                neighbours[length] = currentCell + 1;
                wallsToBreak[length] = "East";
                length++;
            }
        }

        // West
        if (currentCell - 1 >= 0 && currentCell != check)
        {
            if (cells[currentCell - 1].NotVisited())
            {
                neighbours[length] = currentCell - 1;
                wallsToBreak[length] = "West";
                length++;
            }
        }

        // North
        if (currentCell + xSize < totalCells)
        {
            if (cells[currentCell + xSize].NotVisited())
            {
                neighbours[length] = currentCell + xSize;
                wallsToBreak[length] = "North";
                length++;
            }
        }

        // South
        if (currentCell - xSize >= 0)
        {
            if (cells[currentCell - xSize].NotVisited())
            {
                neighbours[length] = currentCell - xSize;
                wallsToBreak[length] = "South";
                length++;
            }
        }



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
