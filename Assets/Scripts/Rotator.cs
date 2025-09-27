using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public float speed = 2f;
    private Vector2 moveDirection;
    private float lastDirectionChange = 0f;
    private float directionChangeCooldown = 0.5f;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        moveDirection = GetRandomDirection();
        
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        rb2d.gravityScale = 0f;
        rb2d.drag = 0f;
        rb2d.angularDrag = 0f;
        rb2d.constraints = RigidbodyConstraints2D.None;
        rb2d.sleepMode = RigidbodySleepMode2D.NeverSleep;
        
        rb2d.velocity = moveDirection * speed;
    }

    Vector2 GetRandomDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        float radians = randomAngle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    }

    void Update()
    {
        rb2d.velocity = moveDirection * speed;
        transform.Rotate(0, 0, 45 * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("WallTrigger") && Time.time > lastDirectionChange + directionChangeCooldown)
        {
            moveDirection = GetRandomDirection();
            rb2d.velocity = moveDirection * speed;
            
            lastDirectionChange = Time.time;
            
            Debug.Log("Hit wall trigger, new direction: " + moveDirection);
        }
    }
}