using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThing : MovingNPC
{
    const int DAMAGEAMOUNT = 5;
    readonly float enemyTurnAroundAndFlee = 180f;

    public int GetDamageAmount()
    {
        return DAMAGEAMOUNT;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + enemyTurnAroundAndFlee, transform.eulerAngles.z);
        }
    }
}
