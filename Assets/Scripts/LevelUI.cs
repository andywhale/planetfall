using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour {

	GameManager gameManager;

	void Start () {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		int currentLevel = gameManager.GetCurrentLevel();
		this.GetComponent<Text>().text = "Level " + currentLevel.ToString();
	}
}
