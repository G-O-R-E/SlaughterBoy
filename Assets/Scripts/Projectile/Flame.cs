using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    float damage = 0;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            PlayerDataManager playerData = FindAnyObjectByType<PlayerDataManager>();
            collision.TryGetComponent<EnemyStatistic>(out EnemyStatistic component);
            if (component != null) 
            {
                component.timer += Time.deltaTime;
                if(component.timer > 0.25f) 
                {
                    component.timer = 0;
                    component.GetDamage(playerData.CritAttack() ? (damage/5) * 2 : (damage/5));
                }
            }
        }
    }

    public void SetDamage(float _damage)
    {
        damage = _damage;
    }

    
}
