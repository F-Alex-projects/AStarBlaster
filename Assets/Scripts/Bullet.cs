using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 24f;
    [SerializeField]private Rigidbody2D blltBody;

    private void FixedUpdate()
    {
        blltBody.velocity = Vector2.up * speed;
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
