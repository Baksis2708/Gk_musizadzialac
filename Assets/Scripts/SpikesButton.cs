using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesButton : MonoBehaviour
{
    [Header("Button Actions")]
    public Transform spikes;   // Kolce, które maj¹ siê wysun¹æ
    public float spikeHeight = 2f;  // Wysokoœæ, na jak¹ maj¹ siê wysun¹æ kolce
    public float spikeDuration = 2f;  // Czas, przez jaki kolce bêd¹ wysuniête
    public float spikeCooldown = 3f; // Czas odnowienia wysuniêcia kolców

    private Vector3 originalPosition; // Pocz¹tkowa pozycja kolców
    private bool isPlayerOnButton = false; // Czy gracz jest na przycisku
    private bool isSpikeActive = false; // Sprawdzenie, czy kolce s¹ aktywne

    private void Start()
    {
        if (spikes != null)
        {
            originalPosition = spikes.position; // Zapamiêtujemy pocz¹tkow¹ pozycjê kolców
        }
    }

    private void Update()
    {
        // Jeœli gracz stoi na przycisku i kolce nie s¹ jeszcze aktywne, aktywuj je
        if (isPlayerOnButton && !isSpikeActive)
        {
            StartCoroutine(ActivateSpikes());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Sprawdzamy, czy gracz wszed³ na przycisk
        if (other.CompareTag("Player"))
        {
            isPlayerOnButton = true;  // Gracz wchodzi na przycisk
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Sprawdzamy, czy gracz opuœci³ przycisk
        if (other.CompareTag("Player"))
        {
            isPlayerOnButton = false;  // Gracz opuszcza przycisk
        }
    }

    // Aktywacja kolców (wysuwanie ich)
    private IEnumerator ActivateSpikes()
    {
        isSpikeActive = true;

        // Natychmiastowe wysuniêcie kolców na okreœlon¹ wysokoœæ
        spikes.position = originalPosition + Vector3.up * spikeHeight;

        // Czekanie przez okreœlony czas, aby kolce by³y wysuniête
        yield return new WaitForSeconds(spikeDuration);

        // Cofanie kolców na pocz¹tkow¹ pozycjê
        spikes.position = originalPosition;

        // Kolce s¹ gotowe do ponownego wysuniêcia po up³ywie cooldownu
        yield return new WaitForSeconds(spikeCooldown);

        isSpikeActive = false; // Kolce znowu gotowe do aktywacji
    }
}
