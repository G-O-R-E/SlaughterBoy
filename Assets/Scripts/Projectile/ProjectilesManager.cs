using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesManager : MonoBehaviour
{
    HUDManager hudManager;
    // Start is called before the first frame update
    void Start()
    {
        hudManager = GameObject.Find("HUDManager").GetComponent<HUDManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hudManager.Timer <= 0)
        {
            DestroyAllEnemies();
        }
    }

    private void DestroyAllEnemies()
    {

        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");

        for (int i = 0; i < projectiles.Length; i++)
        {
            Destroy(projectiles[i]);
        }
    }
}
