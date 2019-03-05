using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour {

    Vector3 pastPosition;
    Quaternion pastRotation;
    GameObject gameManager;
    const float POWERUPTIME = 30.0f;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pastPosition = collision.gameObject.GetComponent<Transform>().position;
            pastRotation = collision.gameObject.GetComponent<Transform>().rotation;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            collision.gameObject.GetComponent<Transform>().position = pastPosition;
            collision.gameObject.GetComponent<Transform>().rotation = pastRotation;
            gameManager.GetComponent<GameManager>().PowerUpFound();
        }
    }
}
