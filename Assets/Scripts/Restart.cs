using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Restart : MonoBehaviour
{

    private int nextXSize = 4;
    private int nextYSize = 4;

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void DelayedRestartGame()
    {
        Invoke("RestartGame", 0.2f);
    }

    public int GetNextLevelXSize()
    {
        return nextXSize;
    }

    public int GetNextLevelYSize()
    {
        return nextYSize;
    }

    public void increaseYSize()
    {
        nextYSize++;
    }

    public void increaseXSize()
    {
        nextXSize++;
    }



}