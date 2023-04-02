using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //Only spawning a few enemies before boss comes in.
    private int enemiesBeforeBoss;
    private float spawnTimer;
    [SerializeField]private Transform[] spawnPoints;
    [SerializeField]private BossScript bossControl;

    private void Awake()
    {
        enemiesBeforeBoss = 20;
        spawnTimer = 2.5f;
    }

    private void Update()
    {
        if (enemiesBeforeBoss > 0)
        {
            StartCoroutine(SpawnEnemies());
        }
        if (enemiesBeforeBoss <= 0)
        {
            bossControl.MoveToArena();
            this.gameObject.SetActive(false);
        }
    }

    //The enemy spawner, works off coroutine so that it's not doing it constantly and also will eventually (with enemy script, stop spawning once enough enemies have been destroyed.)
    IEnumerator SpawnEnemies()
    {
        GameObject enemy = EnemyPool.instance.GetPooledObject();

        if (enemy != null)
        {
            enemy.transform.position = spawnPoints[Random.Range(0,3)].position;
            enemy.SetActive(true);
            enemiesBeforeBoss--;
        }
        yield return new WaitForSeconds(spawnTimer);
    }
}
