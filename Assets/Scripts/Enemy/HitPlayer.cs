using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlayer : MonoBehaviour
{
    [SerializeField] float timebeforehit = 1f;
    float timerhit;
    // Start is called before the first frame update
    void Start()
    {
        timerhit = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerhit > 0f)
        {
            timerhit -= Time.deltaTime;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (enabled && timerhit <= 0f)
        {
            PlayerDataManager playerData = collision.gameObject.GetComponent<PlayerDataManager>();
            if (playerData && !playerData.DodgeAttack())
            {
                playerData.PlayAnimDamage();
                playerData.CurrentLife--;
                timerhit = timebeforehit;
            }
        }
       
    }

}