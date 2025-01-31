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
        Debug.Log($"Pocz�tkowe HP: {currentHP}");
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

        // Sprawdzamy, kt�ry obiekt otrzymuje obra�enia, aby przyzna� punkt drugiemu
        if (currentHP > 0)
        {
            // Je�li gracz zosta� trafiony, dodaj punkt przeciwnikowi
            if (gameObject.CompareTag("Player"))
            {
                GameManager.instance.AddPointToEnemy();
            }
            // Je�li bot zosta� trafiony, dodaj punkt graczowi
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
        Debug.Log($"{gameObject.name} umar�!");

        // Dodaj punkt dla drugiego gracza
        if (gameObject.CompareTag("Player")) // Je�li to gracz, dodaj punkt dla bota
        {
            GameManager.instance.AddPointToEnemy();
        }
        else if (gameObject.CompareTag("Enemy")) // Je�li to bot, dodaj punkt dla gracza
        {
            GameManager.instance.AddPointToPlayer();
        }
    }
}
