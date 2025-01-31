using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
    public Transform targetPlayer;             // Gracz, kt�ry jest celem
    public Transform ammoPickupPoint;          // Tylko jeden punkt amunicji w danym czasie
    public float attackRange = 10f;            // Zasi�g ataku
    public float moveSpeed = 3f;               // Pr�dko�� poruszania si� bota
    public float chaseDistance = 20f;          // Odleg�o��, na jak� bot b�dzie �ciga� gracza
    public HealthSystem healthSystem;          // Odwo�anie do systemu zdrowia
    public AmmoPickup ammoPickup;             // Odwo�anie do skryptu amunicji
    public SpikesButton spikesButton;          // Odwo�anie do przycisku, kt�ry w��cza kolce

    private NavMeshAgent navMeshAgent;         // Agenta do poruszania si� po terenie
    private bool hasAmmo = false;              // Zmienna do sprawdzania, czy bot ma amunicj�

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
        ammoPickupPoint = ammoPickup.transform; // Przypisujemy punkt amunicji
        StartCoroutine(BotBehavior());
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, targetPlayer.position) <= attackRange && hasAmmo)
        {
            AttackPlayer(); // Bot atakuje gracza, je�li ma amunicj�
        }
    }

    IEnumerator BotBehavior()
    {
        while (true)
        {
            if (!hasAmmo)
            {
                SeekAmmo(); // Je�li bot nie ma amunicji, szuka jej
            }
            else
            {
                PatrolOrChase(); // Je�li bot ma amunicj�, mo�e patrolowa� lub �ciga� gracza
            }

            yield return null;
        }
    }

    void SeekAmmo()
    {
        // Bot kieruje si� bezpo�rednio do punktu, w kt�rym pojawi�a si� amunicja
        if (ammoPickupPoint != null)
        {
            navMeshAgent.SetDestination(ammoPickupPoint.position);
        }
    }

    void PatrolOrChase()
    {
        // Je�li bot znajduje si� w zasi�gu gracza, �ciga go
        if (Vector3.Distance(transform.position, targetPlayer.position) <= chaseDistance)
        {
            navMeshAgent.SetDestination(targetPlayer.position);
        }
        else
        {
            // Bot patroluje lub czeka na okazj�
            Patrol();
        }
    }

    void Patrol()
    {
        // Bot mo�e porusza� si� do r�nych punkt�w w pobli�u, aby utrzyma� si� w ruchu
        // Mo�na doda� losowe punkty patrolowe w przysz�o�ci
    }

    void AttackPlayer()
    {
        // Bot atakuje gracza, kiedy jest w odpowiednim zasi�gu i ma amunicj�
        // Mo�esz tutaj doda� kod do wykonywania ataku (np. strza� w kierunku gracza)
        Debug.Log("Bot atakuje gracza!");
    }

    // Funkcja do zbierania amunicji
    public void CollectAmmo()
    {
        hasAmmo = true;
        Debug.Log("Bot zdoby� amunicj�!");
    }

    // Funkcja reaguj�ca na aktywacj� kolc�w (unikaj przeszk�d)
    public void AvoidSpikes()
    {
        // Je�li bot zbli�a si� do aktywnych kolc�w, zatrzymuje si� lub zmienia kierunek
        // Mo�esz doda� logik� unikania w tym miejscu
        Debug.Log("Bot unika kolc�w!");
    }
}
