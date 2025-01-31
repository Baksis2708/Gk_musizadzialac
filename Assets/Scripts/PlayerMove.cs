using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController control;
    private Vector3 pVelo;
    private float speed = 5f; // Zwiêkszona prêdkoœæ
    private int jumpLeft = 2;

    // Zmienne do kamery
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 200f;
    private float xRotation = 0f;

    void Start()
    {
        control = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleJump();

        // Grawitacja
        pVelo.y -= 9.81f * Time.deltaTime;
        control.Move(pVelo * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void HandleMovement()
    {
        // NOWY SYSTEM RUCHU - uwzglêdnia rotacjê gracza
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D
        float moveZ = Input.GetAxisRaw("Vertical");   // W/S

        // Kierunek ruchu wzglêdem rotacji gracza
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        move = move.normalized * speed;

        pVelo.x = move.x;
        pVelo.z = move.z;

        // Sprint
        if (Input.GetKey(KeyCode.LeftShift))
            pVelo.x *= 2f;
        pVelo.z *= 2f;
    }

    void HandleJump()
    {
        if (control.isGrounded)
        {
            jumpLeft = 2;
            pVelo.y = -0.1f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpLeft > 0)
        {
            pVelo.y = 7f;
            jumpLeft--;
        }
    }
}