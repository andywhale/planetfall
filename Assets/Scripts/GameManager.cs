using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    private int nextXSize = 4;
    private int nextYSize = 4;

    private float TIMERMAX = 180.0f;
    private float timer = 60.0f;
    private int level = 1;
    private int currentScore;

    const int SCOREPOWERUP = 500;
    const int SCORENEXTLEVEL = 1000;

    private bool gameOver = false;

    private float levelStartTime;

    private void Start()
    {
        levelStartTime = timer;
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

    public void JumpPointSelected(GameObject obj, PointerEventData pointerEventData)
    {
        Debug.Log(pointerEventData.pointerEnter);
        //obj.GetComponent<JumpScript>().Highlight();

    }

    public int GetCurrentLevel()
    {
        return level;
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public bool GameIsOver()
    {
        return gameOver;
    }

    public void PowerUpFound()
    {
        IncreaseTime();
        currentScore += SCOREPOWERUP;
    }

    public void NextLevel()
    {
        if (GameIsOver())
            return;
        level++;
        currentScore += level * SCORENEXTLEVEL * Mathf.RoundToInt(timer);
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
        }
        else
        {
            timer -= Time.deltaTime;
        }
        UpdateTimerUI();
    }

    public void ReduceTime(int timeToReduceBy)
    {
        timer -= timeToReduceBy;
    }

    public void IncreaseTime()
    {
        timer += (nextXSize * (nextYSize - 1));
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void ResetGame()
    {
        gameOver = false;
        nextXSize = 4;
        nextYSize = 4;
        timer = TIMERMAX;
        level = 1;
        currentScore = 0;
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