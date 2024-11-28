using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponCollision : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerDataManager playerData = transform.parent.parent.parent.GetComponent<PlayerDataManager>();
        MeleeWeapon meleeWeapon = transform.parent.GetComponent<MeleeWeapon>();
        if (meleeWeapon.isAttacking && meleeWeapon.isMovingForward && collision.tag == "Enemy")
        {
            int weaponDamage = transform.parent.GetComponent<MeleeWeapon>().GetDamage();
            collision.GetComponent<EnemyStatistic>().GetDamage(playerData.CritAttack() ? weaponDamage * 2 : weaponDamage);
        }
    }
}
