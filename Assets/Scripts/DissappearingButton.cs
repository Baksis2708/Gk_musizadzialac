using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissappearingButton : MonoBehaviour
{
    [Header("Ustawienia")]
    public float cooldownTime = 7f;       // Czas po którym przycisk siê aktywuje
    public DisappearingObject[] targets;  // Przypisz obiekty do znikania w inspektorze

    private bool isReady = true;
    private Material buttonMaterial;

    void Start()
    {
        buttonMaterial = GetComponent<Renderer>().material;
        UpdateButtonColor();
    }

    void OnTriggerEnter(Collider other)
    {
        if (isReady && other.CompareTag("Player"))
        {
            StartCoroutine(ButtonCooldown());
            TriggerObjectsDisappearance();
        }
    }

    IEnumerator ButtonCooldown()
    {
        isReady = false;
        UpdateButtonColor();

        yield return new WaitForSeconds(cooldownTime);

        isReady = true;
        UpdateButtonColor();
    }

    void TriggerObjectsDisappearance()
    {
        foreach (DisappearingObject obj in targets)
        {
            if (obj != null) obj.DisappearForSeconds(7f);
        }
    }

    void UpdateButtonColor()
    {
        buttonMaterial.color = isReady ? Color.green : Color.red;
    }

}
