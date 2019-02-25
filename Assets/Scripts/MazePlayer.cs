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
    const float SPEED = 0.02f;

    bool moveForward = false;
    string moveDirection = "forward";

    void Start()
    {
        maze = GameObject.Find("Maze");
        mainCamera = GameObject.Find("PlayerCamera");
        player = GameObject.Find("PlayerContainer");
        rb = GetComponent<Rigidbody>();
        playerTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GvrControllerInput.ClickButton)
        {
            IdentifyDirection();
            StartMoveForward();
        }
        if (GvrControllerInput.ClickButtonUp)
        {
            EndMoveForward();
        }
        player.GetComponent<Transform>().position = new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z);
        player.GetComponent<Transform>().rotation = playerTransform.rotation;
        if (moveForward)
            JustMove();
        else
            ResetMovement();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Goal")
        {
            NextLevel();
        }
    }

    void NextLevel()
    {
        GameObject restartObject = GameObject.Find("Restart");
        if (Random.Range(0, 2) == 0)
        {
            restartObject.GetComponent<Restart>().increaseXSize();
        }
        else
        {
            restartObject.GetComponent<Restart>().increaseYSize();
        }
        restartObject.GetComponent<Restart>().DelayedRestartGame();
    }

    private void IdentifyDirection()
    {
        Quaternion currentRotation = mainCamera.GetComponent<Transform>().rotation;
        float angle = currentRotation.eulerAngles.y;
        if (angle > 290)
        {
            moveDirection = "back";
        }
        else if (angle > 230)
        {
            moveDirection = "left";
        }
        else if (angle > 120)
        {
            moveDirection = "forward";
        }
        else if (angle > 50)
        {
            moveDirection = "right";
        }
        else
        {
            moveDirection = "back";
        }
    }

    private void JustMove()
    {
        //Quaternion currentRotation = mainCamera.GetComponent<Transform>().rotation;
        //float angle = currentRotation.eulerAngles.y;
        //playerTransform.position += (playerTransform.TransformDirection(0, angle, 0) * SPEED);
        switch (moveDirection)
        {
            case "forward":
                playerTransform.position -= (playerTransform.forward * SPEED);
                break;
            case "back":
                playerTransform.position += (playerTransform.forward * SPEED);
                break;
            case "left":
                playerTransform.position -= (playerTransform.right * SPEED);
                break;
            case "right":
                playerTransform.position += (playerTransform.right * SPEED);
                break;
        }
    }

    public void StartMoveForward()
    {
        moveForward = true;
    }

    public void EndMoveForward()
    {
        moveForward = false;
    }

    public void ResetMovement()
    {
        playerTransform.position = playerTransform.position;
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }

}
