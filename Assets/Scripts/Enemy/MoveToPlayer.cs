using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class MoveToPlayer : MonoBehaviour
{
    [SerializeField] float maxSpeed;
    [SerializeField] float minSpeed;

    float moveSpeed;
    Rigidbody2D rb;
    GameObject player;
    SpriteRenderer spriteRenderer;
    [SerializeField] float overlapStop = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveSpeed = (float)Random.Range(minSpeed, maxSpeed) *1000 / 1000;        
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();       
        
        spriteRenderer.flipX = player.transform.position.x >= transform.position.x;
    }

    void Move()
    {
        rb.position = Vector3.MoveTowards(rb.position, player.transform.position, moveSpeed * Time.deltaTime);
    }
}

