using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesButton : MonoBehaviour
{
    [Header("Button Actions")]
    public Transform spikes;   // Kolce, kt�re maj� si� wysun��
    public float spikeHeight = 2f;  // Wysoko��, na jak� maj� si� wysun�� kolce
    public float spikeDuration = 2f;  // Czas, przez jaki kolce b�d� wysuni�te
    public float spikeCooldown = 3f; // Czas odnowienia wysuni�cia kolc�w

    private Vector3 originalPosition; // Pocz�tkowa pozycja kolc�w
    private bool isPlayerOnButton = false; // Czy gracz jest na przycisku
    private bool isSpikeActive = false; // Sprawdzenie, czy kolce s� aktywne

    private void Start()
    {
        if (spikes != null)
        {
            originalPosition = spikes.position; // Zapami�tujemy pocz�tkow� pozycj� kolc�w
        }
    }

    private void Update()
    {
        // Je�li gracz stoi na przycisku i kolce nie s� jeszcze aktywne, aktywuj je
        if (isPlayerOnButton && !isSpikeActive)
        {
            StartCoroutine(ActivateSpikes());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Sprawdzamy, czy gracz wszed� na przycisk
        if (other.CompareTag("Player"))
        {
            isPlayerOnButton = true;  // Gracz wchodzi na przycisk
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Sprawdzamy, czy gracz opu�ci� przycisk
        if (other.CompareTag("Player"))
        {
            isPlayerOnButton = false;  // Gracz opuszcza przycisk
        }
    }

    // Aktywacja kolc�w (wysuwanie ich)
    private IEnumerator ActivateSpikes()
    {
        isSpikeActive = true;

        // Natychmiastowe wysuni�cie kolc�w na okre�lon� wysoko��
        spikes.position = originalPosition + Vector3.up * spikeHeight;

        // Czekanie przez okre�lony czas, aby kolce by�y wysuni�te
        yield return new WaitForSeconds(spikeDuration);

        // Cofanie kolc�w na pocz�tkow� pozycj�
        spikes.position = originalPosition;

        // Kolce s� gotowe do ponownego wysuni�cia po up�ywie cooldownu
        yield return new WaitForSeconds(spikeCooldown);

        isSpikeActive = false; // Kolce znowu gotowe do aktywacji
    }
}
