using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool Up = true;
    private float speed = 24f;
    private Vector2 direction;
    [SerializeField]private Rigidbody2D blltBody;

    void Start()
    {
        direction = Up ? Vector2.up : Vector2.down;
    }

    private void FixedUpdate()
    {
        blltBody.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            gameObject.SetActive(false);
        }
        if (other.gameObject.GetComponent<MeteorController>() == true)
        {
            gameObject.SetActive(false);
        }
        if (other.gameObject.GetComponent<BossScript>() == true)
        {
            gameObject.SetActive(false);
        }
    }
}
