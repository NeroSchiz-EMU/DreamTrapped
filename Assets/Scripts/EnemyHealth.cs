using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    private int currentHealth;

    [SerializeField] ParticleSystem particlesDeath;
    [SerializeField] SpriteRenderer sprite;

    void Start()
    {
        currentHealth = health;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            particlesDeath.Play();
            particlesDeath.transform.parent = null; //Remove parent, so that particles can persist after this object is destroyed
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
        StartCoroutine(DamageFlash());
    }

    IEnumerator DamageFlash()
    {
        sprite.color = new Color(1f, 0.5f, 0.5f, 1f); //Set color to slight red
        yield return new WaitForSeconds(0.2f);
        sprite.color = new Color(1f, 1f, 1f, 1f); //Set color to normal
    }
}
