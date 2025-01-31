using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float moveDistance = 5f;
    public bool startMovingRight = true;

    private float startX;
    private int direction;
    private Vector3 lastPosition;
    private CharacterController playerOnPlatform;

    void Start()
    {
        startX = transform.position.x;
        direction = startMovingRight ? 1 : -1;
        lastPosition = transform.position;
    }

    void Update()
    {
        float newX = transform.position.x + direction * moveSpeed * Time.deltaTime;

        if (newX > startX + moveDistance)
        {
            newX = startX + moveDistance;
            direction = -1;
        }
        else if (newX < startX - moveDistance)
        {
            newX = startX - moveDistance;
            direction = 1;
        }

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    void FixedUpdate()
    {
        Vector3 movement = transform.position - lastPosition;
        lastPosition = transform.position;

        // Przesuñ gracza, jeœli stoi na platformie
        if (playerOnPlatform != null)
        {
            playerOnPlatform.Move(movement);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnPlatform = other.GetComponent<CharacterController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnPlatform = null;
        }
    }
}