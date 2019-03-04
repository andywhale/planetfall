using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenOverheadCameraScript : MonoBehaviour {

    GameObject overheadCamera;

    // Use this for initialization
    void Start () {
        overheadCamera = GameObject.Find("OverheadCamera");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
