using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    const int DAMAGEAMOUNT = 5;
    const float SPEED = 0.4f;
    float enemyAngle = 0.0f;
    float enemyTurnAroundAndFlee = 180f;

    // Use this for initialization
    void Start () {
        enemyAngle = Random.Range(90, 180);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 newPosition = transform.position + transform.forward * SPEED * Time.deltaTime;
        transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
    }

    public int GetDamageAmount()
    {
        return DAMAGEAMOUNT;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + enemyAngle, transform.eulerAngles.z);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + enemyTurnAroundAndFlee, transform.eulerAngles.z);
        }
    }
}
