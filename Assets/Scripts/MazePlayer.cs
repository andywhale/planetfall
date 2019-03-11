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
    GameManager gameManager;
    const float SPEED = 1f;
    const int ENEMYTIMERDAMAGE = 10;

    void Start()
    {
        mainCamera = GameObject.Find("PlayerCamera");
        player = GameObject.Find("PlayerContainer");
        rb = GetComponent<Rigidbody>();
        playerTransform = GetComponent<Transform>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        player.GetComponent<Transform>().position = new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z);
        player.GetComponent<Transform>().rotation = playerTransform.rotation;

        if (GvrControllerInput.ClickButton)
            JustMove();
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
            Damaged(collision.gameObject.GetComponent<EnemyScript>().GetDamageAmount());
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
