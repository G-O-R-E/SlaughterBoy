using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossLogic : MonoBehaviour
{
    [SerializeField] float spawnrange = 3;
    [SerializeField] int monsterbyspawn = 5;
    [SerializeField] float distanceRay = 100;
    [SerializeField] float spawnrate = 1f;
    [SerializeField] public List<int> enemiesProb;
    [SerializeField] public List<GameObject> enemies;


    List<int> indexProb;
    Bounds bounds;
    float timerate;   


    // Start is called before the first frame update
    void Start()
    {
        indexProb = new List<int>();
        bounds = FindAnyObjectByType<Spawner>().SpawnPositionRange;
        timerate = spawnrate/2f;
        Init();
    }

    // Update is called once per frame
    void Update()
    {

        timerate -= Time.deltaTime;
        if (timerate <= 0.0f)
        {
            for (int i = 0; i < monsterbyspawn; i++)
            {
                SpawnMonster();             
            }
           
            timerate = spawnrate;
        }

        
    }

    void SpawnMonster()
    {
        Vector3 spawnPos = new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle * spawnrange;
        int randomIdx = indexProb[Random.Range(0, indexProb.Count)];

        if (bounds.Contains(spawnPos))
        {
            Instantiate(enemies[randomIdx], spawnPos, transform.rotation, transform.parent);
        }
    }

    private void Init()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            for (int j = 0; j < (enemiesProb[i] * 100) / 100; j++)
            {
                indexProb.Add(i);
            }
        }
    }


  
}
