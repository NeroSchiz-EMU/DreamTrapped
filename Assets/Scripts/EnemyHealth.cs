using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    private int currentHealth;
    void Start()
    {
        currentHealth = health;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    public void damageEnemy(int damage)
    {
        currentHealth -= damage;
    }

    public void TakeDamage(int swordDamage)
    {
        damageEnemy(swordDamage);
    }
}
