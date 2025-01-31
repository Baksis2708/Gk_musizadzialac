using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
    public Transform targetPlayer;             // Gracz, który jest celem
    public Transform ammoPickupPoint;          // Tylko jeden punkt amunicji w danym czasie
    public float attackRange = 10f;            // Zasiêg ataku
    public float moveSpeed = 3f;               // Prêdkoœæ poruszania siê bota
    public float chaseDistance = 20f;          // Odleg³oœæ, na jak¹ bot bêdzie œciga³ gracza
    public HealthSystem healthSystem;          // Odwo³anie do systemu zdrowia
    public AmmoPickup ammoPickup;             // Odwo³anie do skryptu amunicji
    public SpikesButton spikesButton;          // Odwo³anie do przycisku, który w³¹cza kolce

    private NavMeshAgent navMeshAgent;         // Agenta do poruszania siê po terenie
    private bool hasAmmo = false;              // Zmienna do sprawdzania, czy bot ma amunicjê

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
            AttackPlayer(); // Bot atakuje gracza, jeœli ma amunicjê
        }
    }

    IEnumerator BotBehavior()
    {
        while (true)
        {
            if (!hasAmmo)
            {
                SeekAmmo(); // Jeœli bot nie ma amunicji, szuka jej
            }
            else
            {
                PatrolOrChase(); // Jeœli bot ma amunicjê, mo¿e patrolowaæ lub œcigaæ gracza
            }

            yield return null;
        }
    }

    void SeekAmmo()
    {
        // Bot kieruje siê bezpoœrednio do punktu, w którym pojawi³a siê amunicja
        if (ammoPickupPoint != null)
        {
            navMeshAgent.SetDestination(ammoPickupPoint.position);
        }
    }

    void PatrolOrChase()
    {
        // Jeœli bot znajduje siê w zasiêgu gracza, œciga go
        if (Vector3.Distance(transform.position, targetPlayer.position) <= chaseDistance)
        {
            navMeshAgent.SetDestination(targetPlayer.position);
        }
        else
        {
            // Bot patroluje lub czeka na okazjê
            Patrol();
        }
    }

    void Patrol()
    {
        // Bot mo¿e poruszaæ siê do ró¿nych punktów w pobli¿u, aby utrzymaæ siê w ruchu
        // Mo¿na dodaæ losowe punkty patrolowe w przysz³oœci
    }

    void AttackPlayer()
    {
        // Bot atakuje gracza, kiedy jest w odpowiednim zasiêgu i ma amunicjê
        // Mo¿esz tutaj dodaæ kod do wykonywania ataku (np. strza³ w kierunku gracza)
        Debug.Log("Bot atakuje gracza!");
    }

    // Funkcja do zbierania amunicji
    public void CollectAmmo()
    {
        hasAmmo = true;
        Debug.Log("Bot zdoby³ amunicjê!");
    }

    // Funkcja reaguj¹ca na aktywacjê kolców (unikaj przeszkód)
    public void AvoidSpikes()
    {
        // Jeœli bot zbli¿a siê do aktywnych kolców, zatrzymuje siê lub zmienia kierunek
        // Mo¿esz dodaæ logikê unikania w tym miejscu
        Debug.Log("Bot unika kolców!");
    }
}
