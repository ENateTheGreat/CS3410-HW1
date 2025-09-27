/*
 * Author: E. Nathan Lee
 * Date: 9/27/2025
 * Description: Controls hazard movement. Original rotator script + movement and stick detection logic. A lot of the variables are to try and fine tune the movement.
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    // initialization
    public Rigidbody2D rb2d;
    private Vector2 moveDirection;
    private Vector2 lastPosition;
    private float lastX;
    private float lastY;

    // constants
    public float speed = 10f;
    public float stuckTime = 0.1f;
    public float positionTolerance = 0.05f; // General movement
    public float postTurnCooldown = 0.075f;
    public float watchdog = 0.75f;
    public float positionToleranceXY = 0.045f; // Specific axis movement

    // timers
    private float stuckTimer = 0f;
    private float elapsed = 0f;
    private float cooldown = 0f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        moveDirection = GetRandomDirection(); // Initial direction setting

        rb2d.velocity = moveDirection * speed;
        lastPosition = rb2d.position;

        // Tracking of specific axis movement to prevent sticking to walls
        lastX = rb2d.position.x;
        lastY = rb2d.position.y;
    }

    // Random direction generator
    Vector2 GetRandomDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        float radians = randomAngle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)); // I tried with the basic -1 to 1 range with it was not properly calculating
    }

    void Update()
    {
        transform.Rotate(0, 0, 45 * Time.deltaTime); // OG roation logic

        // cooldown time tracking 
        elapsed += Time.deltaTime;
        if (cooldown > 0f) cooldown -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        rb2d.velocity = moveDirection * speed; // Movement

        // Tracking to begin after cooldown || initial watchdog period to prevent freezing
        if (elapsed < watchdog || cooldown > 0f)
        {
            lastPosition = rb2d.position;
            stuckTimer = 0f;
            lastX = rb2d.position.x;
            lastY = rb2d.position.y;
            return;
        }

        float distanceMoved = Vector2.Distance(rb2d.position, lastPosition); // General movement tracking as a whole

        // Combining the general and axis-specific movement to prevent sticking against walls and in corners
        if (distanceMoved < positionTolerance || Mathf.Abs(lastX - rb2d.position.x) < positionToleranceXY || Mathf.Abs(lastY - rb2d.position.y) < positionToleranceXY)
        {
            stuckTimer += Time.fixedDeltaTime; // setting the timer so it can detect if it is really stuck or changing direction
            if (stuckTimer >= stuckTime)
            {
                moveDirection = GetRandomDirection(); // if stuck, change direction
                rb2d.velocity = moveDirection * speed;
                stuckTimer = 0f; // timer reset
            }
        } 
        else
        {
            stuckTimer = 0f; // not stuck, reset timer
        }
        // Update tracking positions
        lastPosition = rb2d.position;
        lastX = rb2d.position.x;
        lastY = rb2d.position.y;
    }
}