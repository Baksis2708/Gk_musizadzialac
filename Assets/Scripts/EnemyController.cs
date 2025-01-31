using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // Ruch
    [Header("Movement")]
    public float moveSpeed = 3.5f;

    // Atak
    [Header("Attack")]
    public float viewDistance = 30f;
    public float viewAngle = 60f;
    public float fireRate = 1f;
    public int damage = 1;
    public float aimingTime = 0.5f;

    // Amunicja
    [Header("Ammunition")]
    public int currentAmmo = 0;
    public int maxAmmo = 10;

    // Animator
    [Header("Animations")]
    public Animator enemyAnimator;

    // Layer Mask dla przeszkód
    [Header("Layer Masks")]
    public LayerMask obstacleMask;

    // Prywatne zmienne
    private NavMeshAgent agent;
    private Transform player;
    private float fireTimer;
    private bool isAiming = false;
    private bool isMoving = false;
    private bool isDead = false;
    private float aimTimer = 0f;

    // Amunicja do której d¹¿y przeciwnik
    private AmmoPickup currentAmmoPickup;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyAnimator = GetComponent<Animator>();

        // Ustawienia agenta NavMesh
        agent.speed = moveSpeed;

        // Subskrypcja zdarzeñ amunicji
        AmmoPickup.OnAmmoSpawned += OnAmmoSpawned;
        AmmoPickup.OnAmmoPickedUp += OnAmmoPickedUp;
    }

    void OnDestroy()
    {
        // Odsubskrybowanie zdarzeñ
        AmmoPickup.OnAmmoSpawned -= OnAmmoSpawned;
        AmmoPickup.OnAmmoPickedUp -= OnAmmoPickedUp;
    }

    void Update()
    {
        if (isDead) return;

        DetectPlayer();
        HandleAnimations();

        if (currentAmmo > 0)
        {
            // Przeciwnik ma amunicjê
            if (isAiming)
            {
                // Przeciwnik widzi gracza i atakuje
                agent.isStopped = true;
                AimAndShoot();
            }
            else
            {
                // Przeciwnik nie widzi gracza - szuka go
                agent.isStopped = false;
                PatrolOrSearchForPlayer();
            }
        }
        else
        {
            // Przeciwnik nie ma amunicji
            if (currentAmmoPickup != null && currentAmmoPickup.isActive)
            {
                // Jest dostêpna amunicja - idzie po ni¹
                agent.isStopped = false;
                agent.SetDestination(currentAmmoPickup.transform.position);
            }
            else
            {
                // Brak amunicji na mapie - czeka w miejscu
                agent.isStopped = true;
                PatrolOrWait();
            }
        }
    }

    void DetectPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer < viewDistance)
        {
            float angle = Vector3.Angle(transform.forward, directionToPlayer.normalized);
            if (angle < viewAngle / 2f)
            {
                Vector3 rayStart = transform.position + Vector3.up * 1.5f;
                Vector3 rayEnd = player.position + Vector3.up * 1.5f;

                Debug.DrawLine(rayStart, rayEnd, Color.red, 0.1f);

                RaycastHit hitInfo;
                if (Physics.Linecast(rayStart, rayEnd, out hitInfo, obstacleMask))
                {
                    Debug.Log("Przeciwnik nie widzi gracza - przeszkoda: " + hitInfo.collider.name);
                    isAiming = false;
                    agent.isStopped = false;
                }
                else
                {
                    isAiming = true;
                    agent.isStopped = true;
                    Debug.Log("Przeciwnik widzi gracza i atakuje.");
                    return;
                }
            }
            else
            {
                isAiming = false;
                agent.isStopped = false;
            }
        }
        else
        {
            isAiming = false;
            agent.isStopped = false;
        }
    }

    void AimAndShoot()
    {
        // Upewnij siê, ¿e przeciwnik jest zatrzymany podczas celowania
        agent.isStopped = true;

        // Obracaj przeciwnika w kierunku gracza
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        aimTimer += Time.deltaTime;
        Debug.Log($"aimTimer: {aimTimer}, aimingTime: {aimingTime}");

        if (aimTimer >= aimingTime)
        {
            fireTimer += Time.deltaTime;
            Debug.Log($"fireTimer: {fireTimer}, fireRate: {fireRate}");
            if (fireTimer >= fireRate)
            {
                fireTimer = 0f;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        if (currentAmmo <= 0)
        {
            Debug.Log("Przeciwnik nie ma amunicji!");
            return;
        }

        // Wykonaj raycast w kierunku gracza
        Vector3 shootOrigin = transform.position + Vector3.up * 1.5f; // Dopasuj wysokoœæ
        Vector3 shootDirection = (player.position - shootOrigin).normalized;

        Debug.DrawRay(shootOrigin, shootDirection * viewDistance, Color.blue, 1f); // Wizualizacja

        if (Physics.Raycast(shootOrigin, shootDirection, out RaycastHit hit, viewDistance, ~obstacleMask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                HealthSystem playerHealth = hit.collider.GetComponent<HealthSystem>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                    Debug.Log("Przeciwnik trafi³ gracza!");
                }
            }
        }

        currentAmmo--;
        enemyAnimator.SetTrigger("Shoot");
        Debug.Log($"Przeciwnik pozosta³a amunicja: {currentAmmo}");

        // Jeœli przeciwnik nie ma ju¿ amunicji, przestaje celowaæ
        if (currentAmmo <= 0)
        {
            isAiming = false;
            agent.ResetPath();
        }
    }

    void HandleAnimations()
    {
        isMoving = agent.velocity.magnitude > 0.1f;
        bool isSprinting = isMoving; // Mo¿esz dostosowaæ do swoich potrzeb
        bool isWalking = isMoving && !isSprinting;

        enemyAnimator.SetBool("isMoving", isMoving);
        enemyAnimator.SetBool("isRunning", isSprinting);
        enemyAnimator.SetBool("isWalking", isWalking);

        enemyAnimator.SetBool("isAiming", isAiming);
        enemyAnimator.SetBool("isGrounded", agent.isOnNavMesh);
        enemyAnimator.SetBool("isJumping", false);
    }

    void PatrolOrSearchForPlayer()
    {
        if (agent.remainingDistance < 0.5f)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 10f;
            randomDirection += transform.position;
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(randomDirection, out navHit, 10f, NavMesh.AllAreas))
            {
                agent.SetDestination(navHit.position);
                Debug.Log("Przeciwnik patroluje / szuka gracza");
            }
        }
    }

    void PatrolOrWait()
    {
        // Przeciwnik czeka w miejscu
        Debug.Log("Przeciwnik czeka na amunicjê.");
    }

    public void Die()
    {
        isDead = true;
        enemyAnimator.SetTrigger("isDead");
        agent.isStopped = true;
        GetComponent<Collider>().enabled = false;
        this.enabled = false;

        AmmoPickup.OnAmmoSpawned -= OnAmmoSpawned;
        AmmoPickup.OnAmmoPickedUp -= OnAmmoPickedUp;
     //   GameManager.instance.PlayerScored(); // Dodaj punkty graczowi
     //   GameManager.instance.RestartScene();
    }

    void OnAmmoSpawned(AmmoPickup ammoPickup)
    {
        if (currentAmmo <= 0)
        {
            currentAmmoPickup = ammoPickup;
            agent.SetDestination(currentAmmoPickup.transform.position);
        }
    }

    void OnAmmoPickedUp(AmmoPickup ammoPickup)
    {
        if (currentAmmoPickup == ammoPickup)
        {
            currentAmmoPickup = null;
            agent.ResetPath();
        }
    }

    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, maxAmmo);
        Debug.Log($"Przeciwnik zdoby³ amunicjê: {currentAmmo}");
        currentAmmoPickup = null;
    }
}