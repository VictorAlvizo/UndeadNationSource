using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    public float speed;
    public float fireRate = 0.2f;

    public int enemiesKilled = 0;

    public Image healthBar;
    public GameObject bulletPrefab;
    public GameObject coryPrefab;
    public Transform shootingPlace;

    public Vector2 bulletDirection;

    GameObject itemGO;

    Animator animator;

    private int currentAmmo;
    private int maxAmmo;

    private float maxHealth = 100;
    private float currentHealth = 100;
    private float lastFired;

    private bool moveAllow = true;
    private bool onItem = false;
    private bool finishPickUp = true;
    private bool finishReload = true;
    private bool isWeapon = false;

    private string weaponName = "None";
    private string holderName;

    private Rigidbody2D rb;
    private Vector3 lastCursorPos;

	void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        GameObject.Find("noAmmoText").GetComponent<Text>().enabled = false;
        GameObject.Find("GameOverText").GetComponent<Text>().enabled = false;
        GameObject.Find("ScoreText").GetComponent<Text>().enabled = false;
        GameObject.Find("PickUpTitle").GetComponent<Text>().enabled = false;
        GameObject.Find("PickUpImage").GetComponent<Image>().enabled = false;
    }

    void Start()
    {
        healthBar.fillAmount = 1;
    }
	
	void Update () {
        if (moveAllow)
        {
            FaceMouse();

            if (Input.GetMouseButton(0) && finishReload && weaponName != "None" && Time.time >= lastFired + fireRate)
            {
                lastFired = Time.time;

                FireWeapon();
            }
        }
	}

    void FaceMouse()
    {
        if (CheckCursor())
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 dir = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);

            transform.up = dir;

            lastCursorPos = Input.mousePosition;
        }
    }

    bool CheckCursor()
    {
        Vector3 currentCursorPos = Input.mousePosition - lastCursorPos;

        if(currentCursorPos.x == 0 || currentCursorPos.y == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void FireWeapon()
    {
        if(currentAmmo != 0 || maxAmmo != 0)
        {
            SoundManager.instance.PlayOneShot(SoundManager.instance.bulletFire);

            Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            bulletDirection = new Vector2(target.x - transform.position.x, target.y - transform.position.y);

            Instantiate(bulletPrefab, shootingPlace.transform.position, Quaternion.identity);

            currentAmmo--;

            if (currentAmmo == 0 && maxAmmo > 0)
            {
                SoundManager.instance.PlayOneShot(SoundManager.instance.reload);

                finishReload = false;
                Invoke("Reload", 1.5f);

            }
            else if (currentAmmo == 0 && maxAmmo == 0)
            {
                GameObject.Find("noAmmoText").GetComponent<Text>().enabled = true;
            }

            WeaponText();
        }
        else
        {
            SoundManager.instance.PlayOneShot(SoundManager.instance.emptyGun);
        }
    }

    void Reload()
    {
        int valueToFull = 30 - currentAmmo;

        if (currentAmmo == 0 && maxAmmo >= 30)
        {
            maxAmmo = maxAmmo - 30;
            currentAmmo = 30;

        }else if(currentAmmo == 0 && maxAmmo < 30)
        {
            currentAmmo = maxAmmo;
            maxAmmo = 0;

        }else if(maxAmmo >= valueToFull)
        {
            maxAmmo -= valueToFull;
            currentAmmo = 30;
        }
        else
        {
            currentAmmo += maxAmmo;
            maxAmmo = 0;
        }

        finishReload = true;
        WeaponText();
    }

    void FixedUpdate()
    {
        if (moveAllow)
        {
            float horzMove = Input.GetAxisRaw("Horizontal");
            float vertMove = Input.GetAxisRaw("Vertical");

            rb.velocity = new Vector2(horzMove * speed, vertMove * speed);

            if (Input.GetKeyDown(KeyCode.F) && onItem)
            {
                finishPickUp = false;

                if (isWeapon)
                {
                    PickUpWeapon();
                }
                else
                {
                    PickUpItem();
                }
            }

            if (Input.GetKeyDown(KeyCode.R) && weaponName != "None")
            {
                if (maxAmmo > 0)
                {
                    SoundManager.instance.PlayOneShot(SoundManager.instance.reload);

                    finishReload = false;
                    Invoke("Reload", 1.5f);
                }
            }
        }
    }

    void PickUpWeapon()
    {
        SoundManager.instance.PlayOneShot(SoundManager.instance.pickUp);

        weaponName = holderName;

        finishPickUp = true;
        Destroy(itemGO);

        switch (weaponName)
        {
            case "AR-15":
                currentAmmo = 30;
                maxAmmo = 90;
                animator.SetInteger("WeaponID", 1);
                break;

            default:
                currentAmmo = 0;
                maxAmmo = 0;
                animator.SetInteger("WeaponID", 0);
                break;
        }

        WeaponText();
    }

    void PickUpItem()
    {
        SoundManager.instance.PlayOneShot(SoundManager.instance.pickUp);

        switch (holderName)
        {
            case "ammoPack(Clone)":
                if(weaponName != "None")
                {
                    maxAmmo += 30;
                    GameObject.Find("noAmmoText").GetComponent<Text>().enabled = false;
                    WeaponText();
                }
                break;

            case "medKit(Clone)":
                AddHealth(25);
                break;

            case "CoryHelp(Clone)":
                Instantiate(coryPrefab, transform.position, Quaternion.identity);
                break;

            default:
                break;
        }

        finishPickUp = true;
        Destroy(itemGO);
    }

    void DealDamage(float damageTaken)
    {
        if (moveAllow)
        {
            currentHealth -= damageTaken;
            healthBar.fillAmount = currentHealth / maxHealth;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                ChangeScore();

                moveAllow = false;

                SoundManager.instance.PlayOneShot(SoundManager.instance.gameOver);

                GameObject.Find("ScoreText").GetComponent<Text>().text = FinalScoreText();

                GameObject.Find("GameOverText").GetComponent<Text>().enabled = true;
                GameObject.Find("ScoreText").GetComponent<Text>().enabled = true;

                Invoke("Death", 4);
            }

            ChangeScore();
        }
    }

    void AddHealth(float healthIntake)
    {
        SoundManager.instance.PlayOneShot(SoundManager.instance.moreHealth);

        if(currentHealth + healthIntake > 100)
        {
            currentHealth = 100;
        }
        else
        {
            currentHealth += healthIntake;
        }

        healthBar.fillAmount = currentHealth / maxHealth;

        ChangeScore();
    }

    void Death()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    void ChangeScore()
    {
        var healthText = GameObject.Find("HealthPercent").GetComponent<Text>();

        string formatNewText = string.Format("{0}%", currentHealth);

        healthText.text = formatNewText;
    }

    void WeaponText()
    {
        var weaponText = GameObject.Find("WeaponText").GetComponent<Text>();


        string textShow = string.Format("{0}:{1}/{2}", weaponName, currentAmmo, maxAmmo);

        weaponText.text = textShow;
    }

    string FinalScoreText()
    {
        string finalMessage = string.Empty;

        if(enemiesKilled == 0)
        {
            finalMessage = "Put up a fight!";

        }else if(enemiesKilled >= 20)
        {
            finalMessage = "Are you god himself?!";

        }else if(enemiesKilled >= 15)
        {
            finalMessage = "So good yet so bad...";

        }else if(enemiesKilled >= 10)
        {
            finalMessage = "Could have been worse :/";

        }
        else
        {
            finalMessage = "Better luck next time!";
        }

        return string.Format("Enemies Killed: {0} | {1}", enemiesKilled, finalMessage);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.name == "Zombie(Clone)")
        {
            DealDamage(10);
            SoundManager.instance.PlayOneShot(SoundManager.instance.damageTaken);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Weapon")
        {
            itemGO = col.gameObject;
            holderName = col.gameObject.name;
            onItem = true;
            isWeapon = true;

            GameObject.Find("PickUpTitle").GetComponent<Text>().text = holderName;
            GameObject.Find("PickUpTitle").GetComponent<Text>().enabled = true;
            GameObject.Find("PickUpImage").GetComponent<Image>().enabled = true;
        }

        if(col.gameObject.tag == "medKit")
        {
            itemGO = col.gameObject;
            holderName = col.gameObject.name;
            onItem = true;
            isWeapon = false;

            GameObject.Find("PickUpTitle").GetComponent<Text>().text = "MedKit";
            GameObject.Find("PickUpTitle").GetComponent<Text>().enabled = true;
            GameObject.Find("PickUpImage").GetComponent<Image>().enabled = true;
        }

        if(col.gameObject.tag == "ammoPack")
        {
            itemGO = col.gameObject;
            holderName = col.gameObject.name;
            onItem = true;
            isWeapon = false;

            GameObject.Find("PickUpTitle").GetComponent<Text>().text = "Ammo";
            GameObject.Find("PickUpTitle").GetComponent<Text>().enabled = true;
            GameObject.Find("PickUpImage").GetComponent<Image>().enabled = true;
        }

        if(col.gameObject.tag == "CoryHelper")
        {
            itemGO = col.gameObject;
            holderName = col.gameObject.name;
            onItem = true;
            isWeapon = false;

            GameObject.Find("PickUpTitle").GetComponent<Text>().text = "Cory";
            GameObject.Find("PickUpTitle").GetComponent<Text>().enabled = true;
            GameObject.Find("PickUpImage").GetComponent<Image>().enabled = true;
        }

        if (col.tag == "EnemBullet")
        {
            DealDamage(15);
            Destroy(col.gameObject);
            SoundManager.instance.PlayOneShot(SoundManager.instance.damageTaken);
        }
    }

    void OnTriggerExit2D()
    {
        if (finishPickUp)
        {
            itemGO = null;
            holderName = string.Empty;
            onItem = false;
            isWeapon = false;

            GameObject.Find("PickUpTitle").GetComponent<Text>().enabled = false;
            GameObject.Find("PickUpImage").GetComponent<Image>().enabled = false;
        }
    }
}
