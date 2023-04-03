using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossScript : MonoBehaviour
{
    [Header("Boss Values")]
    public float Speed;
    public float DownSpeed, Health, MoveRange;
    public GameObject Lazer;
    public Text HealthText, VictoryText;
    [HideInInspector] public bool Dead;

    [SerializeField] Transform StartPos;
    private Vector2 CurrentPos, DefaultPos;
    private Rigidbody2D Rb;
    private Transform PlayerPos;

    enum State { Idle, Set, Battle}
    private State CurrentState;
    
    void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        PlayerPos = FindObjectOfType<PlayerMovement>().transform;
    }

    void Start()
    {
        HealthText.text = $": {Health}";
        DefaultPos = transform.position;
        Direction = Vector2.left;
    }

    void Update()
    {
        CurrentPos = transform.position;
        switch (CurrentState)
        {
            case State.Idle:
            default:
            break;

            case State.Set:
            Setting();
            break;

            case State.Battle:
            Battling();
            break;

        }
    }

    void Setting()
    {
        transform.position = Vector2.MoveTowards(CurrentPos, StartPos.position, Speed * Time.deltaTime); //move transform
        var dist = Vector2.Distance(CurrentPos, StartPos.position); //check distance to destination

        if (Mathf.Approximately(dist, 0)) //if distance is approximately zero, we've made it to the starting pos
        {
            CurrentState = State.Battle;
            HealthText.gameObject.SetActive(true);
            Speed += 1.5f; //happens only once since this state is being left
            StartCoroutine("FireRoutine"); //also happens only once
        }
    }

    private Vector2 Direction;
    void Battling()
    {
        //cool math that definitly didn't take long to solve
        var destinationX = new Vector2(DefaultPos.x + (Direction.x * MoveRange), CurrentPos.y); //Where we want to be on x axis
        var destinationY = new Vector2(CurrentPos.x, PlayerPos.position.y); //Where we want to be on y axis
        var distX = Vector2.Distance(CurrentPos, destinationX); //Our distance form x destination
        var tempLerp = Vector2.MoveTowards(CurrentPos, destinationY, DownSpeed * Time.deltaTime); //Meta lerp for y axis
        var tempPos = new Vector2(CurrentPos.x, tempLerp.y); //Meta position for shaving off the lerp x axis
        transform.position = Vector2.MoveTowards(tempLerp, destinationX, Speed * Time.deltaTime);

        //for turning left and right
        if (Mathf.Approximately(distX, 0))
        {
            Direction = Direction == Vector2.left ? Vector2.right : Vector2.left;
        }

    }

    public int FireClockMin, FireClockMax;
    private int FireClock;
    IEnumerator FireRoutine()
    {
        FireClock = FireClockMin;

        while (!Dead)
        {
            yield return new WaitForSeconds(FireClock);
            FireClock = Random.Range(FireClockMin,FireClockMax);
            FireLaser();
        }
    }

    void FireLaser()
    {
        //Instantiate(Lazer, CurrentPos + Vector2.down * 2, Quaternion.identity);

        GameObject bullet = EnemyFirePool.instance.GetPooledObject();

        if (bullet != null)
        {
            bullet.transform.position = CurrentPos + (Vector2.down * 2);
            bullet.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(CurrentState);
        if (CurrentState != State.Battle) return;

        switch (other.tag)
        {
            case "PlayerFire":
            Health--; HealthText.text = $": {Health}"; 
            Speed++; Speed = Mathf.Clamp(Speed, 0, 10);
            break;
            
            default:
            break;
        }

        if (Health <= 0)
        {
            HealthText.gameObject.SetActive(false);
            VictoryText.gameObject.SetActive(true);
            VictoryText.text = "You Won! Press R to Restart or Esc to Quit.";
            Dead = true;
            StopCoroutine("FireRoutine");
            gameObject.SetActive(false);
        }
    }

    public void MoveToArena()
    {
        CurrentState = State.Set;
    }
}
