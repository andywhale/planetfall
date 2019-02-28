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
    GameObject gameManager;
    const float SPEED = 1f;

    void Start()
    {
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
            if (gameManager.GetComponent<GameManager>().GameIsOver())
                gameManager.GetComponent<GameManager>().RestartCurrentLevel();
            else
                JustMove();
        }
        else if (GvrControllerInput.AppButton)
        {
            gameManager.GetComponent<GameManager>().ResetGame();
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
        Vector3 newPosition = playerTransform.position + mainCamera.GetComponent<Transform>().transform.forward * SPEED * Time.deltaTime;
        playerTransform.position = new Vector3(newPosition.x, playerTransform.position.y, newPosition.z);
    }

    public void ResetMovement()
    {
        playerTransform.position = playerTransform.position;
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }

}
