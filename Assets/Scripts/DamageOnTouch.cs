using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    [Header("Ustawienia obra�e�")]
    public int damageAmount = 1;
    public float cooldown = 1f;

    private bool canDamage = true;
    public BloodySpikes bloodySpikes; // Odwo�anie do skryptu BloodySpikes, kt�ry obs�uguje krew

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
                    bloodySpikes.ApplyBloodEffect(); // Zwi�kszamy intensywno�� krwi
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
