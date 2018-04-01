using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorAI : MonoBehaviour {

    public float speed;
    public float fireRate;
    public float fireRange;

    public GameObject bulletPrefab;

    private float lastFired;

    private Vector2 targetLocation;

    private Rigidbody2D rb;
	
	void FixedUpdate()
    {
        targetLocation = FindTarget();

        if (FireDistance())
        {
            FireWeapon();
        }
        else
        {
            MoveSurvivor();
        }
    }

    void FireWeapon()
    {
        Vector2 bulletDir = new Vector2(targetLocation.x - transform.position.x, targetLocation.y - transform.position.y);

        transform.up = bulletDir;

        if (Time.time >= lastFired + fireRate && FireDistance())
        {
            lastFired = Time.time;

            SoundManager.instance.PlayOneShot(SoundManager.instance.bulletFire);

            GameObject bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;

            bulletGO.transform.up = bulletDir;

            Rigidbody2D tempBulletControl = bulletGO.GetComponent<Rigidbody2D>();

            tempBulletControl.velocity = bulletDir * 8f;
        }
    }

    void MoveSurvivor()
    {
        Vector2 dir = new Vector2(targetLocation.x - transform.position.x, targetLocation.y - transform.position.y);

        transform.up = Vector2.Lerp(transform.up, dir, 0.3f);

        transform.position = Vector2.MoveTowards(transform.position, targetLocation, Time.deltaTime * speed);
    }

    Vector2 FindTarget()
    {
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("Friend");

        if (allTargets.Length == 1)
        {
            return GameObject.Find("Player").transform.position;
        }

        float closetDistance = Mathf.Infinity;
        GameObject closetTarget = null;

        foreach (GameObject currentTarget in allTargets)
        {
            float distance = (currentTarget.transform.position - transform.position).sqrMagnitude;

            if (distance < closetDistance)
            {
                closetDistance = distance;
                closetTarget = currentTarget;
            }
        }

        Vector2 finalTarget = closetTarget.transform.position;

        return finalTarget;
    }

    bool FireDistance()
    {
        if(Vector2.Distance(transform.position, targetLocation) <= fireRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Bullet")
        {
            GetComponent<EnemyHealthSys>().DealDamage(15);
            Destroy(col.gameObject);
        }
    }
}
