using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingObject : MonoBehaviour
{
    [Header("Komponenty")]
    public Renderer objectRenderer;
    public Collider objectCollider;

    void Start()
    {
        if (objectRenderer == null) objectRenderer = GetComponent<Renderer>();
        if (objectCollider == null) objectCollider = GetComponent<Collider>();
    }

    public void DisappearForSeconds(float seconds)
    {
        StartCoroutine(DisappearanceRoutine(seconds));
    }

    IEnumerator DisappearanceRoutine(float seconds)
    {
        // Wy³¹cz obiekt
        SetComponentsState(false);

        // Czekaj okreœlony czas
        yield return new WaitForSeconds(seconds);

        // W³¹cz ponownie
        SetComponentsState(true);
    }

    void SetComponentsState(bool state)
    {
        if (objectRenderer != null) objectRenderer.enabled = state;
        if (objectCollider != null) objectCollider.enabled = state;
    }
}
