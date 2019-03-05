using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIScript : MonoBehaviour
{
    GameObject gameManager;

    // Use this for initialization
    void Start () {
        gameManager = GameObject.Find("GameManager");

    }
	
	// Update is called once per frame
	void Update () {
        int currentScore = gameManager.GetComponent<GameManager>().GetCurrentScore();
        this.GetComponent<Text>().text = "Score " + currentScore.ToString().PadLeft(9, '0');
    }
}
