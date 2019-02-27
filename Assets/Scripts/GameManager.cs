using UnityEngine;
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
    private float timer = 30.0f;
    private int level = 1;

    private bool active = true;

    private float levelStartTime;

    private void Start()
    {
        timerUI = GameObject.Find("Timer").GetComponent<Text>();
        timerBackground = GameObject.Find("TimerBackground").GetComponent<Image>();
        levelUI = GameObject.Find("Level").GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateTimer();
    }

    public void RestartCurrentLevel()
    {
        timer = levelStartTime;
        ReloadMaze();
    }

    public void NextLevel()
    {
        if (!active)
            return;
        level++;
        levelUI.text = "Ship " + level.ToString().PadLeft(3, '0');
        if (Random.Range(0, 2) == 0)
        {
            IncreaseXSize();
        }
        else
        {
            IncreaseYSize();
        }
        IncreaseTime();
        levelStartTime = timer;
        ReloadMaze();
    }

    private void UpdateTimer()
    {
        if (!active)
            return;
        if (Mathf.Approximately(timer, 0.0f) || timer < 0.0f)
        {
            active = false;
            GameObject.Find("GameOverCanvas").GetComponent<Canvas>().enabled = true;
            Invoke("RestartGame", 5.0f);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void RestartGame()
    {
        active = true;
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

    public void IncreaseYSize()
    {
        nextYSize++;
    }

    public void IncreaseXSize()
    {
        nextXSize++;
    }

}