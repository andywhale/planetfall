using UnityEngine;
using System.Collections;

public class MovingNPC : MovingThing
{
    new float SPEED = 0.4f;
    float wallBounceAngle;

    void Start()
    {
        wallBounceAngle = Random.Range(90, 180);
    }

    void FixedUpdate()
    {
        MoveForward();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + wallBounceAngle, transform.eulerAngles.z);
        }
    }
}
