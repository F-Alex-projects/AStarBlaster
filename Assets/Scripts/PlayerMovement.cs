using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private float strafingPower, upDownMovement;
    private float playerSpeed = 15f;
    private float playerHealth;
    public bool isDead;
    [SerializeField]private Rigidbody2D playerBody;
    [SerializeField]private Transform bulletPosition;
    [SerializeField]private Text healthText, gmOverText;
    [SerializeField]private GameObject finalText;
    
    void Awake()
    {
        playerBody = this.GetComponent<Rigidbody2D>();
        playerSpeed = 9f;
        playerHealth = 5;
        healthText.text = $": {playerHealth}";
        isDead = false;
        Time.timeScale = 1;
    }

    void Update()
    {
        strafingPower = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }
    
    
    void FixedUpdate()
    {
        playerBody.velocity = new Vector2(strafingPower * playerSpeed, 0);
    }

    private void Shoot()
    {
        GameObject bullet = ObjectPool.instance.GetPooledObject();

        if (bullet != null)
        {
            bullet.transform.position = bulletPosition.position;
            bullet.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //new if EnemyFire condition added
        if (other.gameObject.GetComponent<MeteorController>() == true || other.tag.Equals("EnemyFire"))
        {
            playerHealth--;
            healthText.text = $": {playerHealth}";
            if (playerHealth <= 0)
            {
                finalText.gameObject.SetActive(true);
                gmOverText.text = "Game Over. Press R to restart or Esc to quit.";
                isDead = true;
                gameObject.SetActive(false);
            }
            else
            {
                return;
            }
        }
    }

    private void Quit()
    {
        Application.Quit();
    }
}
