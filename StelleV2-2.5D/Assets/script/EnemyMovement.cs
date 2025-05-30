using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed;
    public float chaseRadius;
    public float attackRadius;
    public int attackDamage;
    public Transform target;

    public int maxHealth = 30;
    private int currentHealth;
    private bool isDead = false;
    private bool isAttacking = false;

    private int currentWaypointIndex;
    private Animator animator;
    private EnemyHealth enemyHealth;

    private float lastDirectionX;
    private float idleTimer;
    private bool isIdle;
    private bool isChasing;
    public float minIdleTime = 1f;
    public float maxIdleTime = 3f;
    private float idleTime;

    public float attackCooldown = 1f;
    private float attackTimer = 0;

    void Start()
    {
        currentHealth = maxHealth;
        currentWaypointIndex = Random.Range(0, waypoints.Length);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        enemyHealth = GetComponent<EnemyHealth>(); // Initialize enemyHealth
    }

    void Update()
    {

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        if (enemyHealth.IsDead())
            return;

   
        float targetDistance = Vector2.Distance(target.position, transform.position);

        if (targetDistance <= chaseRadius && targetDistance > attackRadius)
        {
            isChasing = true;
            ChaseAndAttack(target.position);
        }
        else if (targetDistance > chaseRadius)
        {
            isChasing = false;
            if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position) > 0.5f)
            {
                MoveToWaypoint(currentWaypointIndex);
            }
            else
            {
                Patrol();
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dead") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            DestroyEnemy();
        }
    }

    void Patrol()
    {
        if (!isChasing)
        {
            if (isIdle)
            {
                idleTimer += Time.deltaTime;
                if (idleTimer >= idleTime)
                {
                    isIdle = false;
                    idleTimer = 0;
                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                }

                animator.SetFloat("MoveX", lastDirectionX);
                animator.SetBool("isIdle", true);
            }
            else
            {
                Vector2 direction = (waypoints[currentWaypointIndex].position - transform.position).normalized;
                float distance = Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position);
                if (distance < 0.5f)
                {
                    isIdle = true;
                }

                float speedModifier = Mathf.Clamp(distance, 0, 1);
                transform.Translate(direction * speed * speedModifier * Time.deltaTime, Space.World);

                if (distance > 0.5f)
                {
                    lastDirectionX = Mathf.Sign(direction.x);
                }

                animator.SetFloat("MoveX", lastDirectionX);
                animator.SetBool("isIdle", false);
            }
        }
    }

    void ChaseAndAttack(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        if (Mathf.Abs(Mathf.Sign(direction.x) - lastDirectionX) > 0.5f)
        {
            lastDirectionX = Mathf.Sign(direction.x);
        }

        animator.SetFloat("MoveX", lastDirectionX);
        animator.SetBool("isIdle", false);

        if (Vector2.Distance(target.position, transform.position) <= attackRadius)
        {

            // Check the attack timer
            if (attackTimer <= 0)
            {
                animator.SetTrigger("Attack");
                isAttacking = true;

                PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                }

                // Reset the attack timer
                attackTimer = attackCooldown;
            }

        }
    }

    void MoveToWaypoint(int waypointIndex)
    {
        Vector2 direction = (waypoints[waypointIndex].position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        if (Mathf.Abs(Mathf.Sign(direction.x) - lastDirectionX) > 0.5f)
        {
            lastDirectionX = Mathf.Sign(direction.x);
        }

        animator.SetFloat("MoveX", lastDirectionX);
        animator.SetBool("isIdle", false);
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                isDead = true;
                animator.SetTrigger("isDead");
            }
            else
            {
                animator.SetTrigger("isHit");
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    animator.ResetTrigger("isHit");
                }
            }
        }
    }

    private void DestroyEnemy()
    {
        StartCoroutine(DestroyEnemyCoroutine());
    }

    private IEnumerator DestroyEnemyCoroutine()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    public void AttackFinished()
    {
        isAttacking = false;
    }
}


