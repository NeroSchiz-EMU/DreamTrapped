using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBlockHealth : MonoBehaviour
{
    public int health;
    private int currentHealth;

    [SerializeField] ParticleSystem particles;

    void Start()
    {
        currentHealth = health;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            particles.Play();
            particles.transform.parent = null; //Remove parent, so that particles can persist after this object is destroyed
            Destroy(gameObject);
        }
    }
    
    public void damageGunBlock(int damage)
    {
        currentHealth -= damage;
    }
}
