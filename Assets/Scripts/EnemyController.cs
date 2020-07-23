using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    public bool alive = true;
    float health;
    public float maxHealth = 25f;
    public float damage = 0;
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    float attackTimer = 0;
    public Transform groundDetector;
    public LayerMask groundMask;

    public Transform target;
    public LayerMask targetList;
    float distance = -1;
    public float speed = 50;
    public float prefDistance = 0.5f;
    public float jumpStr = 10;
    public float nextWaypointDistance = 0.5f;
    public float agroRange = 5f;

    bool grounded = true;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    private Seeker seeker;
    private Animator anim;
    private Rigidbody2D physObj;

    void Start()
    {
        health = maxHealth;
        anim = GetComponentInChildren<Animator>();
        physObj = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void FixedUpdate()
    {
        float movingThreshold = 0.1f;
        if (physObj.velocity.x > movingThreshold || physObj.velocity.x < -movingThreshold)
        {
            anim.SetBool("Moving", true);
        }
        else
        {
            anim.SetBool("Moving", false);
        }

        if (alive)
        {
            grounded = Physics2D.OverlapCircle(groundDetector.position, 0.05f, groundMask);
            anim.SetBool("Grounded", grounded);
            if (target)
            {
                distance = Vector2.Distance(physObj.position, target.position);
                
                //Lose Target Agro if too far away (for performance reasons)
                if (distance > 10)
                    target = null;

                bool moving = Move();
                //Attacks if not moving.
                if (!moving && distance <= attackRange)
                {
                    attackTimer += Time.deltaTime;
                    Vector2 direction = ((Vector2)target.position - physObj.position).normalized;
                    FaceDirection(direction);
                    //TODO Add Logic for what attack to do eg. ranged attack or melee.
                    if (attackTimer >= attackCooldown)
                    {
                        Attack();
                        attackTimer = 0;
                    }
                }
            }
            else
            {
                Collider2D foundTarget = Physics2D.OverlapCircle(transform.position, agroRange, targetList);
                if (foundTarget)
                {
                    target = foundTarget.transform;
                }
            }
        }
    }

    //Movement Script
    //uses A* Seeker Path to find out how to get to player.
    //jumps if path points up and stops automatically when in preferred distance.
    private bool Move()
    {
        if (path == null)
            return false;

        UpdateEndOfPath();

        if (!reachedEndOfPath)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - physObj.position).normalized;

            FaceDirection(direction);

            if (distance > prefDistance)
            {

                Vector2 force = direction * speed * 10 * Time.fixedDeltaTime;
                if (direction.y > 0.9 && grounded)
                {
                    //Jump up
                    physObj.AddForce(new Vector2(0, jumpStr));
                }
                physObj.AddForce(force);

                float waypointDist = Vector2.Distance(physObj.position, path.vectorPath[currentWaypoint]);
                if (waypointDist < nextWaypointDistance)
                {
                    currentWaypoint++;
                }
            }
            else
            {
                Vector3 v = physObj.velocity;
                v.x *= 0.5f;
                physObj.velocity = v;
                return false;
            }
            return true;
        }
        else return false;

    }

    //TODO Make Enemy Attack actually do something.
    private bool Attack()
    {
        //Debug.Log($"{gameObject.name} is Attacking");
        anim.SetTrigger("Attack1");
        return false;
    }

    public void FaceDirection(Vector3 direction)
    {
        float scalingThreshold = 0.1f;
        Vector3 scale = transform.localScale;
        if (direction.x > scalingThreshold)
        {
            scale.x = 1;
        }
        else if (direction.x < -scalingThreshold)
        {
            scale.x = -1;
        }
        transform.localScale = scale;
    }

    private void UpdateEndOfPath()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            //Debug.Log(gameObject.name + " reached end of path");
        }
        else
        {
            reachedEndOfPath = false;
        }

    }

    void UpdatePath()
    {
        if (alive && seeker.IsDone() && target != null)
        {
            seeker.StartPath(physObj.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public float Damage(Player source, float hit)
    {
        if (hit > 0)
        {
            anim.SetTrigger("Hit");
            health -= hit;
            if (health <= 0)
            {
                alive = false;
            }
            anim.SetBool("Alive", alive);
        }
        return health;
    }

    public void ApplyVelocity(Vector3 v)
    {
        physObj.velocity = v;
    }
}
