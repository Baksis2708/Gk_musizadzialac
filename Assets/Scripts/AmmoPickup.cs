using UnityEngine;
using System.Collections;
using System;

public class AmmoPickup : MonoBehaviour
{
    // Zdarzenia
    public static event Action<AmmoPickup> OnAmmoSpawned;
    public static event Action<AmmoPickup> OnAmmoPickedUp;

    [Header("Ustawienia")]
    public float floatSpeed = 1f;
    public float floatHeight = 0.5f;
    public int ammoAmount = 1;
    public float respawnDelay = 5f;
    public Transform[] spawnPoints;
    public Transform hiddenPosition;

    private Vector3 startPosition;
    [HideInInspector]
    public bool isActive = true;
    private int lastSpawnIndex = -1;

    void Start()
    {
        StartCoroutine(InitialSpawnDelay(UnityEngine.Random.Range(0f, 2f)));
    }

    IEnumerator InitialSpawnDelay(float delay)
    {
        // Schowaj amunicjê na start
        HideAmmo();

        yield return new WaitForSeconds(delay);
        RespawnAmmo();
        StartCoroutine(FloatAnimation());
    }

    IEnumerator FloatAnimation()
    {
        while (true)
        {
            if (isActive)
            {
                float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }
            yield return null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isActive && other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().AddAmmo(ammoAmount);
            StartCoroutine(RespawnAfterDelay());

            // Wywo³aj zdarzenie zebrania amunicji
            OnAmmoPickedUp?.Invoke(this);
        }
        else if (isActive && other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().AddAmmo(ammoAmount);
            StartCoroutine(RespawnAfterDelay());

            // Wywo³aj zdarzenie zebrania amunicji
            OnAmmoPickedUp?.Invoke(this);
        }
    }

    IEnumerator RespawnAfterDelay()
    {
        isActive = false;
        HideAmmo();

        yield return new WaitForSeconds(respawnDelay);
        RespawnAmmo();
    }

    void RespawnAmmo()
    {
        int newIndex;
        do
        {
            newIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
        } while (newIndex == lastSpawnIndex); // Zapobiega respawnowi w tym samym miejscu

        transform.position = spawnPoints[newIndex].position;
        startPosition = transform.position;
        lastSpawnIndex = newIndex;

        isActive = true;
        ShowAmmo();

        // Wywo³aj zdarzenie pojawienia siê amunicji
        OnAmmoSpawned?.Invoke(this);
    }

    void HideAmmo()
    {
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        transform.position = hiddenPosition.position;
    }

    void ShowAmmo()
    {
        GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
}
