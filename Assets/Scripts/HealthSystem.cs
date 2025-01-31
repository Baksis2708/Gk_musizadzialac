using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maxHP = 3;
    public int currentHP;
    public AudioSource hurtSound;
    public ParticleSystem hitEffect;

    void Start()
    {
        currentHP = maxHP;
        Debug.Log($"Pocz¹tkowe HP: {currentHP}");
    } 
        

    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log($"{gameObject.name} HP: {currentHP}");

        if (hitEffect != null) hitEffect.Play();
        if (hurtSound != null) hurtSound.Play();

        // Sprawdzamy, który obiekt otrzymuje obra¿enia, aby przyznaæ punkt drugiemu
        if (currentHP > 0)
        {
            // Jeœli gracz zosta³ trafiony, dodaj punkt przeciwnikowi
            if (gameObject.CompareTag("Player"))
            {
                GameManager.instance.AddPointToEnemy();
            }
            // Jeœli bot zosta³ trafiony, dodaj punkt graczowi
            else if (gameObject.CompareTag("Enemy"))
            {
                GameManager.instance.AddPointToPlayer();
            }
        }
        else
        {
            Die();
        }
    }


    void Die()
    {
        Debug.Log($"{gameObject.name} umar³!");

        // Dodaj punkt dla drugiego gracza
        if (gameObject.CompareTag("Player")) // Jeœli to gracz, dodaj punkt dla bota
        {
            GameManager.instance.AddPointToEnemy();
        }
        else if (gameObject.CompareTag("Enemy")) // Jeœli to bot, dodaj punkt dla gracza
        {
            GameManager.instance.AddPointToPlayer();
        }
    }
}
