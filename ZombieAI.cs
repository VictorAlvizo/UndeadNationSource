using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ZombieAI : MonoBehaviour {

    public float speed;

    void FixedUpdate()
    {
        Vector2 targetLocation = FindTarget();

        Vector2 dir = new Vector2(targetLocation.x - transform.position.x, targetLocation.y - transform.position.y);

        transform.up = Vector2.Lerp(transform.up, dir, 0.5f);

        transform.position = Vector2.MoveTowards(transform.position, targetLocation, Time.deltaTime * speed);
    }

    Vector2 FindTarget()
    {
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("Friend");

        if(allTargets.Length == 1)
        {
            return GameObject.Find("Player").transform.position;
        }

        float closetDistance = Mathf.Infinity;
        GameObject closetTarget = null;

        foreach(GameObject currentTarget in allTargets)
        {
            float distance = (currentTarget.transform.position - transform.position).sqrMagnitude;

            if(distance < closetDistance)
            {
                closetDistance = distance;
                closetTarget = currentTarget;
            }
        }

        Vector2 finalTarget = closetTarget.transform.position;

        return finalTarget;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Bullet")
        {
            GetComponent<EnemyHealthSys>().DealDamage(25);

            Destroy(col.gameObject);
        }
    }
}
