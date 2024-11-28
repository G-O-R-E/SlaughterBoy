using UnityEngine;

public class PlayAnimOnStart : MonoBehaviour
{
    Color color;

    bool isSpawnAnimFinished = false;

    float animTime = 3.0f;
    [SerializeField] AnimationClip animSpaw;

    Animator animator;

    // Start is called before the first frame update
    void Awake()
    {       // GetClip("Enemy_Spawn").length;
        animTime = animSpaw.length;
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        PlaySpawnAnimation();
        if (isSpawnAnimFinished == true)
        {
            animator.SetBool("HasSpawn", true);
            GetComponent<EnemyStatistic>().hasSpawned = true;

            if (GetComponent<MoveToPlayer>())
            {
                GetComponent<MoveToPlayer>().enabled = true;
            }
            if (GetComponent<HitPlayer>())
            {
                GetComponent<HitPlayer>().enabled = true;
            }
            if (GetComponent<ShootToPlayer>())
            {
                GetComponent<ShootToPlayer>().enabled = true;
            }
            if (GetComponent<KamikazeEnemy>())
            {
                GetComponent<KamikazeEnemy>().enabled = true;
            }
            if (GetComponent<BossLogic>())
            {
                GetComponent<BossLogic>().enabled = true;
            }
            if (GetComponent<BoxCollider2D>())
            {
                GetComponent<BoxCollider2D>().enabled = true;
            }
            enabled = false;
        }
    }

    void PlaySpawnAnimation()
    {
        animTime -= Time.deltaTime;
        if (animTime <= 0.0f) 
        {
            isSpawnAnimFinished = true;
        }
    }
}
