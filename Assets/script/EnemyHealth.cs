using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 30; // 敌人的最大生命值
    public int currentHealth; // 敌人的当前生命值
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

        // 播放受伤动画
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
        // 敌人死亡逻辑，敌人消失或者掉落物品

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

