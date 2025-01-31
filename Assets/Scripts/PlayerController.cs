using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Ruch
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 5f;
    public float gravity = 30f;
    public int maxJumps = 2;
    public float sprintMultiplier = 2f;
    public float crouchMultiplier = 0.3f;

    // Kamera
    [Header("Camera")]
    public Transform playerCamera;
    public float mouseSensitivity = 100f;
    [Range(0f, 120f)] public float verticalClamp = 80f;

    // Dash
    [Header("Dash")]
    public float dashForce = 40f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1.5f;

    private CharacterController controller;
    private Vector3 velocity;
    private float verticalVelocity;
    private int jumpsRemaining;
    private float xRotation;
    private bool canDash = true;

    [Header("Amunicja")]
    public int currentAmmo = 0;
    public int maxAmmo = 10;

    [Header("Shoot")]
    public float aimingFOV = 40f;
    public float normalFOV = 60f;
    public float aimSmoothSpeed = 10f;
    public float fireRate = 0.5f; // Czas miêdzy strza³ami
    public Transform firePoint;
    

    private bool isAiming = false;
    private bool canShoot = true;
    private Camera mainCamera;
    

    [Header("Celownik")]
    public Image crosshair; // Przypisz w inspektorze
    public Color normalCrosshairColor = Color.white;
    public Color aimingCrosshairColor = Color.red;
    public float crosshairSize = 20f;

    [Header("Kamera FP")]
    public Vector3 normalCameraPosition; // Ustaw w inspektorze
    public Vector3 aimingCameraPosition; 
    public float cameraTransitionSpeed = 5f;

    [Header("Animcje")]
    public Animator playerAnimator;
    private bool isDead = false;
    private bool isJumping = false;

    private MovingPlatform currentPlatform;
    bool isMoving = false;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        jumpsRemaining = maxJumps;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mainCamera = playerCamera.GetComponent<Camera>();
        normalFOV = mainCamera.fieldOfView;
        currentAmmo = 0; // Start z 0 amunicji (do zmiany przy spawnie ammo)
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

        HandleAnimations();
            
        
        HandleAiming();
        HandleShooting();
        HandleMouseLook();
        HandleMovement();
        HandleJump();
        HandleDash();
        HandleCrosshair();    
        HandleCameraPosition();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotacja gracza (lewo/prawo)
        transform.Rotate(Vector3.up * mouseX);

        // Rotacja kamery (góra/dó³)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = (transform.right * horizontal + transform.forward * vertical).normalized;
        float speedMultiplier = 1f;

        if (Input.GetKey(KeyCode.LeftShift)) speedMultiplier = sprintMultiplier;
        if (Input.GetKey(KeyCode.LeftControl)) speedMultiplier = crouchMultiplier;

        Vector3 movement = move * moveSpeed * speedMultiplier * Time.deltaTime;

        // Dodanie ruchu platformy, jeœli gracz na niej stoi
        //if (currentPlatform != null)
        //{
                     
        //    movement += currentPlatform.GetPlatformMovement();
        //}

        controller.Move(movement);
    }


    void HandleJump()
    {
        if (controller.isGrounded)
        {
            jumpsRemaining = maxJumps;
            verticalVelocity = -2f;
            isJumping = false; // L¹dowanie = koniec animacji skoku
            playerAnimator.SetBool("isJumping", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0)
        {
            verticalVelocity = Mathf.Sqrt(jumpForce * 2f * gravity);
            jumpsRemaining--;
            isJumping = true; // Rozpoczêcie animacji skoku
            playerAnimator.SetBool("isJumping", true);
        }

        verticalVelocity -= gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }


    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.Q) && canDash)
        {
            StartCoroutine(PerformDash());
        }
    }

    IEnumerator PerformDash()
    {
        canDash = false;
        float timer = dashDuration;

        Vector3 dashDirection = GetDashDirection();
        Vector3 dashVelocity = dashDirection * dashForce;

        while (timer > 0)
        {
            controller.Move(dashVelocity * Time.deltaTime);
            timer -= Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    Vector3 GetDashDirection()
    {
        // Dash w kierunku ruchu lub patrzenia
        Vector3 inputDirection = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0f,
            Input.GetAxisRaw("Vertical")
        ).normalized;

        return inputDirection != Vector3.zero ?
            transform.TransformDirection(inputDirection) :
            transform.forward;
    }
    void HandleAiming()
    {
        if (Input.GetMouseButtonDown(1) && !isMoving)
        {
            isAiming = !isAiming;
        }

        float targetFOV = isAiming ? aimingFOV : normalFOV;
        mainCamera.fieldOfView = Mathf.Lerp(
            mainCamera.fieldOfView,
            targetFOV,
            aimSmoothSpeed * Time.deltaTime
        );
    }

    void HandleShooting()
    {
        if (isAiming && Input.GetMouseButtonDown(0) && canShoot && currentAmmo > 0)
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        canShoot = false;

        // Wykonaj raycast w kierunku celowania
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.collider.CompareTag("Enemy")) // Wymaga tagu "Enemy" na przeciwniku
            {
                HealthSystem enemyHealth = hit.collider.GetComponent<HealthSystem>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(1); // Zadaj 1 obra¿enie
                    Debug.Log("Gracz trafi³ przeciwnika!");
                }
            }
        }
   //     Debug.DrawRay(shootOrigin, shootDirection * viewDistance, Color.red, 1f);
        currentAmmo--;
        Debug.Log($"Gracz pozosta³a amunicja: {currentAmmo}");

        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    // FUTURE: Metoda do dodawania amunicji              / done
    // public void AddAmmo(int amount) => currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, maxAmmo);


    void HandleCrosshair()
    {
        if (crosshair != null)
        {
            // Zmiana koloru i rozmiaru
            crosshair.color = isAiming ? aimingCrosshairColor : normalCrosshairColor;
            crosshair.rectTransform.sizeDelta = Vector2.one * crosshairSize;
        }
    }

    void HandleCameraPosition()
    {
        Vector3 targetPosition = isAiming ? aimingCameraPosition : normalCameraPosition;

        playerCamera.localPosition = Vector3.Lerp(
            playerCamera.localPosition,
            targetPosition,
            cameraTransitionSpeed * Time.deltaTime
        );
    }
    void HandleAnimations()
    {
        // Ruch
        float moveThreshold = 0.1f;
        isMoving = Mathf.Abs(Input.GetAxis("Horizontal")) > moveThreshold || Mathf.Abs(Input.GetAxis("Vertical")) > moveThreshold;
        playerAnimator.SetBool("isMoving", isMoving);

        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);
        bool isWalking = isMoving && !isRunning; // Tylko jeœli siê rusza, ale nie biegnie

        playerAnimator.SetBool("isRunning", isRunning);
        playerAnimator.SetBool("isWalking", isWalking);

        // Skok
        playerAnimator.SetBool("isGrounded", controller.isGrounded);
        playerAnimator.SetBool("isJumping", isJumping);

        // Celowanie
        playerAnimator.SetBool("isAiming", isAiming);

        // Strza³
        if (Input.GetMouseButtonDown(0) && isAiming)
        {
            playerAnimator.SetTrigger("Shoot");
        }
    }
    void Die()
    {
      //  GameManager.instance.EnemyScored(); // Dodaj punkty botowi
     //   GameManager.instance.RestartScene();
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // SprawdŸ, czy gracz stoi na platformie
        if (hit.collider.CompareTag("MovingPlatform"))
        {
            currentPlatform = hit.collider.GetComponent<MovingPlatform>();
        }
        else
        {
            currentPlatform = null;
        }
    }
    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, maxAmmo);
        Debug.Log($"Aktualna amunicja: {currentAmmo}");
    }
}