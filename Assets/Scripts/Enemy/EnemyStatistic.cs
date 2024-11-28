using UnityEngine;
using UnityEngine.Rendering;

public class EnemyStatistic : MonoBehaviour
{
    static int instancenmbr = 0;
    [Header("Statistique")]
    [SerializeField] bool isAlive = true;
     public bool hasSpawned = false;
    [SerializeField] float life = 1;
    [SerializeField] int xp = 5;
    [SerializeField] float animDamageTime = 0.5f;

    public float Life { get => life;  set => life = value; }

    [SerializeField] GameObject money;
    [SerializeField] GameObject blood;
    Transform moneyParent;
    Transform fxParent;

    GameObject player;
    Color color;

    public bool IsAlive { get { return isAlive; } }
    public bool hasgetDamage = false;

    public float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {        
        GetComponent<SortingGroup>().sortingOrder = instancenmbr;
        instancenmbr++;
        player = GameObject.Find("Player");
        moneyParent = GameObject.Find("Coins").transform;
        fxParent = GameObject.Find("Fxs").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0)
        {
            if (GetComponent<BossLogic>())
            {
                FindAnyObjectByType<HUDManager>().Timer = 1;
            }
            isAlive = false;
            if (hasgetDamage)
            {
                DestroyEnemy(true);
            }
            else
            {
                DestroyEnemy(false);
            }
           
        }
    }

    public void GetDamage(float _damage)
    {
        if (hasSpawned) 
        {
            hasgetDamage = true;
            life -= _damage;
            PlayAnimDamage();        
        }
     
    } 
    
 
    void PlayAnimDamage()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("StopAnimDamage", animDamageTime);        
    }
    void StopAnimDamage()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    public void DestroyEnemy(bool hasblood)
    {
        if (!isAlive)
        {
            instancenmbr--;
            player.GetComponent<PlayerDataManager>().CurrentXp += xp;
            AchievementManager.instance.AddEnnemyKill();

            if (hasblood)
            {
                Instantiate(money, transform.position, transform.rotation, moneyParent);
                Instantiate(blood, transform.position, blood.transform.rotation, fxParent);
            }

            Destroy(gameObject);
        }
    }

}
