using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    ParticleSystem particlesystem;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        particlesystem = GetComponent<ParticleSystem>();
        timer = particlesystem.main.duration;       
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (particlesystem.particleCount <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
