using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour {

    Transform cameraTransform; // find the camera transform in some way at startup, it depends on your scene setup

    // Use this for initialization
    void Start () {
        cameraTransform = GameObject.Find("PlayerCamera").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        //transform.position = cameraTransform.position + Vector3.down * 5; // change 5 to whatever distance you want the canvas to sit from the camera
        //transform.rotation = Quaternion.Euler(90 /* face-up */, 0, -cameraTransform.rotation.y /* rotate to match heading of camera */);
	}
}
