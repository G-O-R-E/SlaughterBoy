using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    int damage = 0;
    [SerializeField] GameObject paintHit;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Boundaries")
        {
            PlayerDataManager playerData = transform.parent.parent.GetComponent<PlayerDataManager>();
            collision.gameObject.TryGetComponent<EnemyStatistic>(out EnemyStatistic component);
            if (component != null)
            {
                component.GetDamage(playerData.CritAttack() ? damage * 2 : damage);
            }

            TryGetComponent<ParticleSystem>(out ParticleSystem ps);
            if (ps != null)
            {
                paintHit.GetComponent<ParticleSystem>().Play();
            }

            Destroy(gameObject);
        }
    }

    public void SetBulletDamage(int _damage)
    {
        damage = _damage;
    }
}
