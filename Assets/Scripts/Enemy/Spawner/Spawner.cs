using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct WavesSettingsEnemy
{
    
    [SerializeField] public List<GameObject> enemies;
    [SerializeField] public List<int> enemiesProb;
    [SerializeField] public List<int> enemieslife;

    [SerializeField] public int probOutOf;
    [SerializeField] public float spawnrate;
    [SerializeField] public int enemiesnumber; 
   
}
public class Spawner : MonoBehaviour
{
    [Header("EnnemiesAndProb")]
    [SerializeField] WavesSettingsEnemy[] wavesSettings;
    WavesSettingsEnemy settings;

    [Header("SpawnLogic")]
    [SerializeField] GameObject background;
    [SerializeField] Transform enemyParentTransform;

    [SerializeField] int indexWaveSettings;
    List<int> listForProbEnnemies;

    private HUDManager hudManager;
 
    Bounds spawnPositionRange;

    public Bounds SpawnPositionRange { get => spawnPositionRange; } 
    int difficulty;
    float timerate;

    // Start is called before the first frame update
    void Start()
    {
        Init();

        difficulty = GameManager.instance.GetDataGame().difficulty;
        spawnPositionRange = background.GetComponent<Renderer>().bounds;
        hudManager = GameObject.Find("HUDManager").GetComponent<HUDManager>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (hudManager.Timer > 0f && wavesSettings.Length > 0) 
        {
            CallFunctionEveryRate();
        }
        else if (hudManager.Timer <= 0f)
        {
            DestroyAllEnemies();
        }
    }


    // Start is called before the first frame update
    Vector2 GetRandomLocation()
    {
        Vector2 point = new Vector2(
              Random.Range(spawnPositionRange.min.x, spawnPositionRange.max.x),
              Random.Range(spawnPositionRange.min.y, spawnPositionRange.max.y));

        return point;
    }

    void CallFunctionEveryRate()
    {
        if (settings.enemiesnumber > 0 )
        {
            timerate -= Time.deltaTime;
            if (timerate <= 0.0f)
            {
                settings.enemiesnumber--;
                SpawnAtRandomLocation();
                timerate = settings.spawnrate;
            }
        }        
    }
    
    void SpawnAtRandomLocation()
    {
        Vector2 randomLocation = GetRandomLocation();
        Quaternion rotation = new Quaternion();

        if (listForProbEnnemies.Count != 0 )
        {
            int idxRandominList = Random.Range(0, listForProbEnnemies.Count);
            int idxEnemy = listForProbEnnemies[idxRandominList];
            GameObject temp = Instantiate(settings.enemies[idxEnemy], randomLocation, rotation);
            temp.transform.parent = enemyParentTransform;
            temp.GetComponent<EnemyStatistic>().Life = settings.enemieslife[idxEnemy];
        }     
    }

    private void DestroyAllEnemies()
    {

        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < enemy.Length; i++)
        {
            Destroy(enemy[i]);
        }
    }

    private void Init()
    {
       int waves = GameManager.instance.GetDataGame().waves;

        if (wavesSettings.Length != 0)
        {
            indexWaveSettings = (waves * wavesSettings.Length / wavesSettings.Length) -1;

            if (indexWaveSettings >= wavesSettings.Length)
            {
                indexWaveSettings = wavesSettings.Length - 1;
            }
            settings = wavesSettings[indexWaveSettings];

            timerate = settings.spawnrate;

            listForProbEnnemies = new List<int>();

            for (int i = 0; i < settings.enemies.Count; i++)
            {
                for (int j = 0; j < (settings.enemiesProb[i] * settings.probOutOf) / 100; j++)
                {
                    listForProbEnnemies.Add(i);
                }
            }
        }     
    }
}
