using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSys : MonoBehaviour {

    public float maxHealth;

    public GameObject medKit;
    public GameObject ammoPack;
    public GameObject cory;

    private float currentHealth;

	void Start () {
        currentHealth = maxHealth;
	}

	public void DealDamage(float damageTaken)
    {
        currentHealth -= damageTaken;

        if(currentHealth <= 0)
        {
            currentHealth = 0;

            if(this.gameObject.name == "Survivor(Clone)" || this.gameObject.name ==  "Boss(Clone)")
            {
                DropItem();
            }

            GameObject.Find("Player").GetComponent<Player>().enemiesKilled += 1;

            Destroy(gameObject);
        }
    }

    void DropItem()
    {
        int itemChoose = Random.Range(0, 3);

        switch (itemChoose)
        {
            case 0:
                SoundManager.instance.PlayOneShot(SoundManager.instance.itemDrop);
                Instantiate(medKit, transform.position, Quaternion.identity);
                break;

            case 1:
                SoundManager.instance.PlayOneShot(SoundManager.instance.itemDrop);
                Instantiate(ammoPack, transform.position, Quaternion.identity);
                break;

            case 2:
                SoundManager.instance.PlayOneShot(SoundManager.instance.itemDrop);

                if (GameObject.Find("Cory(Clone)") == null && GameObject.Find("CoryHelp(Clone)") == null)
                {
                    Instantiate(cory, transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(ammoPack, transform.position, Quaternion.identity);
                }
                break;

            default:
                break;
        }
    }
}
