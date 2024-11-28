using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class KamikazeEnemy : MonoBehaviour
{
    [SerializeField] GameObject explosionFx;
    [SerializeField] int damage = 3;
    Transform fxtransform;
    
    // Start is called before the first frame update
    void Start()
    {
        fxtransform = GameObject.Find("Fxs").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (enabled)
        {
            if (collider2D.GetComponent<PlayerDataManager>() != null)
            {
                PlayerDataManager playerData = collider2D.gameObject.GetComponent<PlayerDataManager>();
                if (playerData && !playerData.DodgeAttack())
                {
                    playerData.PlayAnimDamage();
                    playerData.CurrentLife-= damage;
                    Explode();
                }
              
            }
        }
      
    }

    private void OnTriggerStay2D(Collider2D collider2D)
    {
        OnTriggerEnter2D(collider2D);
    }

    void Explode()
    {
        Instantiate(explosionFx, transform.position, transform.rotation, fxtransform);
        EnemyStatistic stats= GetComponent<EnemyStatistic>();
        if (stats)
        {
            stats.Life -= 100000; ;
            stats.hasgetDamage = false;
        }
       
       
    }
}
