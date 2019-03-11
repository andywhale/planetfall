using UnityEngine;
using System.Collections;

public class MovingThing : MonoBehaviour
{
    protected const float SPEED = 1.0f;

    protected void MoveForward()
    {
        Vector3 newPosition = transform.position + GetForward() * SPEED * Time.deltaTime;
        transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
    }

    virtual protected Vector3 GetForward()
    {
        return transform.forward;
    }
}
