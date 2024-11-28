using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] float speed = 15.0f;
    float time = 10;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Boundaries")
        {          
            Destroy(gameObject);
        }

        PlayerDataManager playerData = collision.GetComponent<PlayerDataManager>();
        if (playerData)
        {
            if (playerData && !playerData.DodgeAttack())
            {
                playerData.PlayAnimDamage();
                collision.gameObject.GetComponent<PlayerDataManager>().CurrentLife--;
                Destroy(gameObject);
            }
        }
    }

    public float GetSpeed()
    {
        return speed;
    }
}
