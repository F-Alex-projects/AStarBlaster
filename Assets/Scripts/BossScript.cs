using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{
    private State state;
    private bool inArena, toP1, toP2;
    public bool isDead;
    [SerializeField]GameObject startPosition;
    [SerializeField]GameObject pointA, pointB;
    private Transform currentPoint;
    private Rigidbody2D rb;
    private float speed;
    private float bossHealth;
    private float distance;
    [SerializeField]private float distanceBetween;
    [SerializeField]private Text bossHealthText, victoryText;
    [SerializeField]private GameObject FinalText;
    

    private enum State
    {
        idle, starting, bossTime
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = 3;
        state = State.idle;
        inArena = false;
        currentPoint = pointB.transform;
        distanceBetween = .05f;
        bossHealth = 5;
        bossHealthText.text = $": {bossHealth}";
        isDead = false;
    }

    void Update()
    {
        switch(state)
        {
            default:
            case State.idle:
                break;
            case State.starting:
                Starting();
                break;
            case State.bossTime:
                BossTime();
                break;
                
        }
    }

    private void Starting()
    {
        transform.position = Vector2.MoveTowards(this.transform.position, startPosition.transform.position, speed * Time.deltaTime);
        if (inArena)
        {
            bossHealthText.gameObject.SetActive(true);
            toP2 = true;
            state = State.bossTime;
        }
    }

    private void BossTime()
    {
        distance = Vector2.Distance(transform.position, currentPoint.transform.position);
        Vector2 direction = currentPoint.position - transform.position;
        direction.Normalize();
        speed = 4.5f;
        if (distance <= distanceBetween && toP1)
        {
            currentPoint = pointA.transform;
            transform.position = Vector2.MoveTowards(this.transform.position, currentPoint.transform.position, speed * Time.deltaTime);
            toP1 = false;
            toP2 = true;
        }
        else if (distance <= distanceBetween && toP2)
        {
            currentPoint = pointB.transform;
            transform.position = Vector2.MoveTowards(this.transform.position, currentPoint.transform.position, speed * Time.deltaTime);
            toP1 = true;
            toP2 = false;
        }
        else
        {
            transform.position = Vector2.MoveTowards(this.transform.position, currentPoint.transform.position, speed * Time.deltaTime);
        }
            
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8 && !inArena)
        {
            inArena = true;
        }

        if (other.gameObject.GetComponent<Bullet>() == true && inArena)
        {
            bossHealth--;
            bossHealthText.text = $": {bossHealth}";
            if (bossHealth <= 0)
            {
                bossHealthText.gameObject.SetActive(false);
                FinalText.gameObject.SetActive(true);
                victoryText.text = "You won! Press R to restart or Esc to quit.";
                isDead = true;
                gameObject.SetActive(false);
            }
            else
            {
                return;
            }
        }
    }

    public void MoveToArena()
    {
        state = State.starting;
    }
}
