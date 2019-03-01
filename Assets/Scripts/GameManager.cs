﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    private int nextXSize = 4;
    private int nextYSize = 4;

    private float TIMERMAX = 90.0f;
    private float timer = 80.0f;
    private int TOTALSHIPS = 5;
    private int currentships = 5;
    private int level = 1;

    private bool gameOver = false;

    private float levelStartTime;

    private void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameIsOver())
            UpdateTimer();
    }

    public void RestartCurrentLevel()
    {
        timer = levelStartTime;
        gameOver = false;
        ReloadMaze();
    }

    public int GetCurrentLevel()
    {
        return level;
    }

    public bool GameIsOver()
    {
        return gameOver;
    }

    public void NextLevel()
    {
        if (GameIsOver())
            return;
        level++;

        GameObject[] glassPanelsLevelText = GameObject.FindGameObjectsWithTag("LevelUIText");
        Debug.Log(glassPanelsLevelText.Length);
        for (var i = 0; i < glassPanelsLevelText.Length; i++)
        {
            glassPanelsLevelText[i].GetComponent<Text>().text = "Ship " + level.ToString().PadLeft(4, '0');
        }
        IncreaseLevelSize();
        IncreaseTime();
        levelStartTime = timer;
        ReloadMaze();
    }

    private void UpdateTimer()
    {
        if (Mathf.Approximately(timer, 0.0f) || timer < 0.0f)
        {
            gameOver = true;
            SetGameOverUIOn();
        }
        else
        {
            timer -= Time.deltaTime;
            UpdateTimerUI();
        }
    }

    void IncreaseTime()
    {
        timer += Random.Range(10, 40);
        if (timer > TIMERMAX)
            timer = TIMERMAX;
        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        GameObject[] timers = GameObject.FindGameObjectsWithTag("TimerUI");
        for (var i = 0; i < timers.Length; i++)
        {
            timers[i].GetComponent<TimerScript>().UpdateTimer(timer, TIMERMAX);
        }
    }

    public void ReloadMaze()
    {
        SetGameOverUIOff();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void ResetGame()
    {
        gameOver = false;
        nextXSize = 4;
        nextYSize = 4;
        timer = TIMERMAX;
        level = 1;
        ReloadMaze();
    }

    public int GetNextLevelXSize()
    {
        return nextXSize;
    }

    public int GetNextLevelYSize()
    {
        return nextYSize;
    }

    public void IncreaseLevelSize()
    {
        if (Random.Range(0, 2) == 0)
        {
            IncreaseXSize();
        }
        else
        {
            IncreaseYSize();
        }
    }

    private void IncreaseYSize()
    {
        nextYSize++;
    }

    private void IncreaseXSize()
    {
        nextXSize++;
    }

    private void SetGameOverUIOn()
    {
        GameObject[] glassPanels = GameObject.FindGameObjectsWithTag("GameOverUI");
        for (var i = 0; i < glassPanels.Length; i++)
        {
            glassPanels[i].GetComponent<Canvas>().enabled = true;
        }
        GameObject[] glassPanelsLevel = GameObject.FindGameObjectsWithTag("LevelUI");
        for (var i = 0; i < glassPanels.Length; i++)
        {
            glassPanelsLevel[i].GetComponent<Canvas>().enabled = false;
        }

    }

    private void SetGameOverUIOff()
    {
        GameObject[] glassPanels = GameObject.FindGameObjectsWithTag("GameOverUI");
        for (var i = 0; i < glassPanels.Length; i++)
        {
            glassPanels[i].GetComponent<Canvas>().enabled = false;
        }
        GameObject[] glassPanelsLevel = GameObject.FindGameObjectsWithTag("LevelUI");
        for (var i = 0; i < glassPanels.Length; i++)
        {
            glassPanelsLevel[i].GetComponent<Canvas>().enabled = true;
        }
    }

}