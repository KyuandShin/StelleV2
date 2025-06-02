using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 30; 
    public int currentHealth;
    public float EnemydieTime;
    private Animator animator;

   void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Take damage!");
        currentHealth -= damage;


        if (currentHealth > 0)
        {
            animator.SetTrigger("isHit");
        }
        else
        {
            Die();
        }
    }

   

    private void Die()
    {
        EnemydieTime = 10;
        GetComponent<Animator>().SetTrigger("isDead");
        Invoke("KillEnemy", EnemydieTime);


    }

    void KillEnemy()
    {
        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Dead");
    }

}

