﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    Text levelUI;
    Text timerUI;
    Image timerBackground;

    private int nextXSize = 4;
    private int nextYSize = 4;

    private float TIMERMAX = 90.0f;
    private float timer = 40.0f;
    private int TOTALSHIPS = 5;
    private int currentships = 5;
    private int level = 1;

    private bool gameOver = false;

    private float levelStartTime;

    private void Start()
    {
        levelUI = GameObject.Find("Level").GetComponent<Text>();
        timerUI = GameObject.Find("Timer").GetComponent<Text>();
        timerBackground = GameObject.Find("TimerBackground").GetComponent<Image>();
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

    public bool GameIsOver()
    {
        return gameOver;
    }

    public void NextLevel()
    {
        if (GameIsOver())
            return;
        level++;
        levelUI.text = "Ship " + level.ToString().PadLeft(3, '0');
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
            GameObject.Find("GameOverCanvas").GetComponent<Canvas>().enabled = true;
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
        timerUI.text = Mathf.Round(timer).ToString();
        timerBackground.fillAmount = timer / TIMERMAX;
    }

    public void ReloadMaze()
    {
        GameObject.Find("GameOverCanvas").GetComponent<Canvas>().enabled = false;
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

}