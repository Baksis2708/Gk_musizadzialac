using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    [Header("Ustawienia obra¿eñ")]
    public int damageAmount = 1;
    public float cooldown = 1f;

    private bool canDamage = true;
    public BloodySpikes bloodySpikes; // Odwo³anie do skryptu BloodySpikes, który obs³uguje krew

    private void OnTriggerEnter(Collider other)
    {
        if (canDamage && other.CompareTag("Player"))
        {
            HealthSystem health = other.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
                StartCoroutine(DamageCooldown());

                // Zastosowanie efektu krwi
                if (bloodySpikes != null)
                {
                    bloodySpikes.ApplyBloodEffect(); // Zwiêkszamy intensywnoœæ krwi
                }
            }
        }
    }

    IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(cooldown);
        canDamage = true;
    }
}
