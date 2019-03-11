using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowUI : MonoBehaviour
{
    GameManager gameManager;

    // Use this for initialization
    void Start ()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        int currentLevel = gameManager.GetCurrentLevel();
        this.GetComponent<Text>().text = "Ship " + currentLevel.ToString().PadLeft(6, '0');
    }
	
	// Update is called once per frame
	void Update () {
		if (gameManager.GameIsOver())
        {
            Text windowText = this.GetComponent<Text>();
            windowText.color = new Color32(255, 52, 0, 255);
            windowText.text = "Time has run out. Hope is futile. The App button will only perpetuate this desperate cycle.";
            windowText.fontSize = 60;
        }
    }
}
