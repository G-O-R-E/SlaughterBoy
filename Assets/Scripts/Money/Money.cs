using UnityEngine;
using UnityEngine.Audio;

public class Money : MonoBehaviour
{
    private Transform targetIcon;
    private float speed = 20.0f;
    private bool moveToPlayer = false;
    private HUDManager hudManager;
    private PlayerDataManager playerDataManager;
    private Stats playerStats;

    [Header("Sound Parameter")]
    [SerializeField] AudioMixerGroup mixerGroup;
    [SerializeField] AudioClip clip;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        targetIcon = GameObject.Find("Player").transform;
        hudManager = GameObject.Find("HUDManager").GetComponent<HUDManager>();
        playerDataManager = GameObject.Find("Player").GetComponent<PlayerDataManager>();
        playerStats = GameObject.Find("Player").GetComponentInChildren<Stats>();
        GetComponent<CircleCollider2D>().radius = 1f + (0.1f * playerStats.GetHarvest());
    }

    // Update is called once per frame
    void Update()
    {
        if (moveToPlayer && hudManager.Timer > 0f)
        {
            MoveToTarget();
        }
        else if (hudManager.Timer <= 0f)
        {
            if (targetIcon != GameObject.Find("MoneyHUD").transform)
            {
                targetIcon = GameObject.Find("MoneyHUD").transform;
            }
            MoveToUITarget();
        }
    }
    private void MoveToUITarget()
    {
        if (targetIcon != null)
        {
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(targetIcon.position);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                playerDataManager.Money++;
                Destroy(gameObject);
            }
        }
    }
    private void MoveToTarget()
    {
        if (targetIcon != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetIcon.position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetIcon.position) < 0.1f)
            {
                audioManager.PlaySound(clip, mixerGroup);
                playerDataManager.Money++;
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!moveToPlayer)
        {
            if (collision.tag == "Player")
            {
                moveToPlayer = true;
            }
        }
    }
}