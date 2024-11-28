using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootToPlayer : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] float shootrateSpeed = 1f;

    GameObject player;
    GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        parent = GameObject.Find("Projectiles");

        InvokeRepeating("Shoot", 1f, shootrateSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector2 GetPlayerDirection()
    {       
        return (player.transform.position - transform.position).normalized;
    }


    void Shoot()
    {
        if (projectile != null)
        {
            GameObject temp = Instantiate(projectile, transform.position, transform.rotation);
            float speed = temp.GetComponent<ProjectileController>().GetSpeed();
            temp.GetComponentInChildren<Rigidbody2D>().velocity = GetPlayerDirection() * speed;

            if (parent != null)
            {
                temp.transform.parent = parent.transform;
            }
            
        }
    }
}
