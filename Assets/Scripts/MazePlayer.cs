using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using Gvr.Internal;

public class MazePlayer : MonoBehaviour {

    Rigidbody rb;
    Transform playerTransform;
    GameObject mainCamera;
    GameObject player;
    GameObject maze;
    GameObject gameManager;
    const float SPEED = 1f;

    void Start()
    {
        maze = GameObject.Find("Maze");
        mainCamera = GameObject.Find("PlayerCamera");
        player = GameObject.Find("PlayerContainer");
        rb = GetComponent<Rigidbody>();
        playerTransform = GetComponent<Transform>();
        gameManager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        player.GetComponent<Transform>().position = new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z);
        player.GetComponent<Transform>().rotation = playerTransform.rotation;

        if (GvrControllerInput.ClickButton)
        {
            JustMove();
        }
        else
        {
            ResetMovement();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Goal")
        {
            gameManager.GetComponent<GameManager>().NextLevel();
        }
    }

    private void JustMove()
    {
        playerTransform.position = playerTransform.position + mainCamera.GetComponent<Transform>().transform.forward * SPEED * Time.deltaTime;
    }

    public void ResetMovement()
    {
        playerTransform.position = playerTransform.position;
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }

}
