using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoryAI : MonoBehaviour {

    public float speed;
    public float fireRate;

    public Vector2 bulletDirection;

    public GameObject bullet;

    GameObject playerGO;

    private float lastShot;

    private Vector2 targetLocation;

    void Awake()
    {
        playerGO = GameObject.Find("Player");
    }

    void FixedUpdate()
    {
        if (AllowFire())
        {
            targetLocation = FindTarget();
            
            if(targetLocation != Vector2.zero)
            {
                FireWeapon();
            }
            else
            {
                MoveCory();
            }
        }
        else
        {
            MoveCory();
        }
    }

    void FireWeapon()
    {
        bulletDirection = new Vector2(targetLocation.x - transform.position.x, targetLocation.y - transform.position.y);

        transform.up = bulletDirection;

        if (Time.time >= lastShot + fireRate && CloseDistant())
        {
            lastShot = Time.time;

            SoundManager.instance.PlayOneShot(SoundManager.instance.bulletFire);

            Instantiate(bullet, transform.position, Quaternion.identity);
        }
    }

    void MoveCory()
    {
        Vector2 dir = playerGO.transform.position - transform.position;

        transform.up = dir;

        transform.position = Vector2.MoveTowards(transform.position, playerGO.transform.position, Time.deltaTime * speed);
    }

    Vector2 FindTarget()
    {
        float closetDistance = Mathf.Infinity;

        GameObject closetTarget = null;
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("Mob");

        if(allTargets.Length >= 1)
        {
            foreach (GameObject currentTarget in allTargets)
            {
                float distanceToCory = (currentTarget.transform.position - transform.position).sqrMagnitude;

                if (distanceToCory < closetDistance)
                {
                    closetDistance = distanceToCory;
                    closetTarget = currentTarget;
                }
            }

            Vector2 targetLocation = closetTarget.transform.position;

            return targetLocation;
        }

        return Vector2.zero;
    }

    bool AllowFire()
    {
        if(Vector2.Distance(playerGO.transform.position, transform.position) < 6)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CloseDistant()
    {
        if(Vector2.Distance(targetLocation, transform.position) <= 7)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.name == "Zombie(Clone)")
        {
            GetComponent<EnemyHealthSys>().DealDamage(25);
            SoundManager.instance.PlayOneShot(SoundManager.instance.coryHurt);
        }
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "EnemBullet")
        {
            GetComponent<EnemyHealthSys>().DealDamage(15);
            Destroy(col.gameObject);

            SoundManager.instance.PlayOneShot(SoundManager.instance.coryHurt);
        }
    }
}
