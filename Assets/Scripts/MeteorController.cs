using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour
{
    [SerializeField] private float meteorHealth;
    [SerializeField] private GameObject player;
    private Vector2 ahead;
    private float speed;
    private float distance;
    [SerializeField] private float distanceBetween;
    private bool lockedOn;
    
    void Awake()
    {
        speed = 2.75f;
        lockedOn = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnDisable()
    {
        lockedOn = false;
    }
    
    void Update()
    {
        ahead = new Vector2(this.transform.position.x, this.transform.position.y -10);
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if(distance <= distanceBetween || lockedOn)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
            lockedOn = true;
        }
        else if (distance > distanceBetween)
        {
           transform.position = Vector2.MoveTowards(this.transform.position, ahead, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Bullet>() == true)
        {
            meteorHealth--;
            if (meteorHealth <= 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                return;
            }
        }

        if (other.gameObject.layer == 6)
        {
            gameObject.SetActive(false);
        }

        if (other.gameObject.GetComponent<PlayerMovement>() == true)
        {
            gameObject.SetActive(false);
        }
    }
}
