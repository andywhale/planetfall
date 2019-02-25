using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using Gvr.Internal;

public class MazePlayer : MonoBehaviour {

    Rigidbody rb;
    Transform transform;
    Quaternion startCameraRotation;
    GameObject mainCamera;
    GameObject playerCamera;
    GameObject birdseyeCamera;
    GameObject maze;
    const int THRUST = 5;
    const float SPEED = 0.02f;
    const float ROTATIONSPEED = 0.005f;
    bool birdseye = false;

    bool moveUp = false;
    bool moveDown = false;
    bool moveLeft = false;
    bool moveRight = false;

    string moveDirection = "forward";

    float playerRotation = 0.0f;
    bool moveForward = false;

    // Use this for initialization
    void Start()
    {
        maze = GameObject.Find("Maze");
        mainCamera = GameObject.Find("PlayerCamera");
        startCameraRotation = mainCamera.GetComponent<Transform>().rotation;
        playerCamera = GameObject.Find("PlayerContainer");
        birdseyeCamera = GameObject.Find("BirdseyeCamera");
        rb = GetComponent<Rigidbody>();
        transform = GetComponent<Transform>();
    }

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
            //SwitchCamera();
        //StartRotation(currentRotation.eulerAngles.y - startCameraRotation.eulerAngles.y);
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
        //TranslateKeysToMovement();
        playerCamera.GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        playerCamera.GetComponent<Transform>().rotation = transform.rotation;
        //if (birdseye)
        //{
        //    BirdseyeMovement();
        //}
        //else
        //{
            if (!Mathf.Approximately(playerRotation, 0.0f))
                RotatePlayer();
            if (moveForward)
                JustMove();
            else
                ResetMovement();
            //FirstPersonMovement();
        //}
    }

    void TranslateKeysToMovement()
    {
        if (Input.GetKeyDown("up"))
        {
            SetMoveUp();
        } else if (Input.GetKeyDown("down"))
        {
            SetMoveDown();
        } else if (Input.GetKeyDown("left"))
        {
            SetMoveLeft();
        } else if (Input.GetKeyDown("right"))
        {
            SetMoveRight();
        }
        else if (Input.GetKeyUp("up") || Input.GetKeyUp("down") || Input.GetKeyUp("left") || Input.GetKeyUp("right"))
        {
            ResetMove();
        }
    }

    public void SetMoveUp()
    {
        ResetMove();
        moveUp = true;
    }

    public void SetMoveDown()
    {
        ResetMove();
        if (birdseye)
            moveDown = true;
        else
            moveUp = true;
    }

    public void SetMoveLeft()
    {
        ResetMove();
        moveLeft = true;
    }

    public void SetMoveRight()
    {
        ResetMove();
        moveRight = true;
    }

    public void ResetMove()
    {

        moveUp = false;
        moveDown = false;
        moveLeft = false;
        moveRight = false;
    }

    void BirdseyeMovement()
    {
        if (moveUp)
        {
            BirdseyeMoveUp();
        }
        else if (moveDown)
        {
            BirdseyeMoveDown();
        }
        else if (moveLeft)
        {
            BirdseyeMoveLeft();
        }
        else if (moveRight)
        {
            BirdseyeMoveRight();
        }
        else
        {
            ResetMovement();
        }
    }

    void FirstPersonMovement()
    {
        if (moveUp)
        {
            MoveUp();
        }
        if (moveDown)
        {
            MoveDown();
        }
        if (moveLeft)
        {
            TurnLeft();
        }
        if (moveRight)
        {
            TurnRight();
        }
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
        if (!birdseye)
        {
            birdseye = false;
            //SwitchCamera();
        }
        if (Random.Range(0, 2) == 0)
        {
            maze.GetComponent<MazeScript>().increaseXSize();
        }
        else
        {
            maze.GetComponent<MazeScript>().increaseYSize();
        }
        GameObject.Find("Restart").GetComponent<Restart>().DelayedRestartGame();
    }

    public void JustMove()
    {
        //Quaternion currentRotation = mainCamera.GetComponent<Transform>().rotation;
        //float angle = currentRotation.eulerAngles.y;
        //transform.position += (transform.TransformDirection(0, angle, 0) * SPEED);
        switch (moveDirection)
        {
            case "forward":
                transform.position -= (transform.forward * SPEED);
                break;
            case "back":
                transform.position += (transform.forward * SPEED);
                break;
            case "left":
                transform.position -= (transform.right * SPEED);
                break;
            case "right":
                transform.position += (transform.right * SPEED);
                break;
        }
    }

    public void IdentifyDirection()
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

    public void MoveUp()
    {
        transform.position += (transform.forward * SPEED);
    }

    public void MoveDown()
    {
        transform.position -= (transform.forward * SPEED);
    }

    public void TurnLeft()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y - SPEED * 50, transform.rotation.z);
    }

    public void TurnRight()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y + SPEED * 50, transform.rotation.z);
    }

    public void StartMoveForward()
    {
        moveForward = true;
    }

    public void EndMoveForward()
    {
        moveForward = false;
    }

    public void StartRotation(float pointerPlayerRotation)
    {
        playerRotation = pointerPlayerRotation;
    }

    public void EndRotation()
    {
        playerRotation = 0.0f;
    }

    public void RotatePlayer()
    {
        Debug.Log(transform.eulerAngles.y);
        Debug.Log(playerRotation);
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y + playerRotation * ROTATIONSPEED, transform.rotation.z);
        EndRotation();
    }

    public void BirdseyeMoveUp()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, THRUST);
    }

    public void BirdseyeMoveDown()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, -THRUST);
    }

    public void BirdseyeMoveLeft()
    {
        rb.velocity = new Vector3(-THRUST, rb.velocity.y, 0);
    }

    public void BirdseyeMoveRight()
    {
        rb.velocity = new Vector3(THRUST, rb.velocity.y, 0);
    }

    public void ResetMovement()
    {
        transform.position = transform.position;
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }
    public void SwitchCamera()
    {
        if (birdseye)
        {
            birdseye = false;
            birdseyeCamera.GetComponent<Camera>().enabled = false;
            playerCamera.GetComponent<Camera>().enabled = true;
        }
        else
        {
            birdseye = true;
            birdseyeCamera.GetComponent<Camera>().enabled = true;
            playerCamera.GetComponent<Camera>().enabled = false;
        }
    }
}
