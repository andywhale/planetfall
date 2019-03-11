using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIScript : MonoBehaviour
{
    GameManager gameManager;

    // Use this for initialization
    void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }
	
	// Update is called once per frame
	void Update () {
        int currentScore = gameManager.GetCurrentScore();
        this.GetComponent<Text>().text = "Score " + currentScore.ToString().PadLeft(9, '0');
    }
}
