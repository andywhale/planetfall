using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using Gvr.Internal;

public class MazePlayer : MovingThing {

    Transform playerTransform;
    GameObject mainCamera;
    GameObject player;
    GameManager gameManager;

    void Start()
    {
        mainCamera = GameObject.Find("PlayerCamera");
        player = GameObject.Find("PlayerContainer");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        player.GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        player.GetComponent<Transform>().rotation = transform.rotation;

        if (GvrControllerInput.ClickButton)
            MoveForward();
        else if (GvrControllerInput.AppButton)
            gameManager.RestartCurrentLevel();
        else
            ResetMovement();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Goal")
        {
            gameManager.NextLevel();
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Damaged(collision.gameObject.GetComponent<EnemyThing>().GetDamageAmount());
            Invoke("DamageOver", 0.2f);
        }
    }

    private void Damaged(int damageTime)
    {
        gameManager.ReduceTime(damageTime);
        player.GetComponent<Light>().enabled = true;
    }

    private void DamageOver()
    {
        player.GetComponent<Light>().enabled = false;
    }

    override protected Vector3 GetForward()
    {
        return mainCamera.GetComponent<Transform>().transform.forward;
    }

    public void ResetMovement()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    }

}
