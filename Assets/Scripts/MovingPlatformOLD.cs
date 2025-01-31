using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform2 : MonoBehaviour
{
    [Header("Platform Settings")]
    public Transform[] points;  // Punkty trasy platformy
    public float speed = 2f;
    public bool loop = true;
    public bool pingPong = false;

    private int currentPointIndex = 0;
    private bool movingForward = true;

    private void Start()
    {
        if (points.Length < 2)
        {
            Debug.LogError("Platforma potrzebuje co najmniej 2 punktów do ruchu!");
            enabled = false;
            return;
        }

        transform.position = points[0].position; // Ustawienie pozycji startowej
    }

    private void Update()
    {
        if (points.Length < 2) return;

        MovePlatform();
    }

    private void MovePlatform()
    {
        Transform targetPoint = points[currentPointIndex];

        // Debugowanie pozycji
        Debug.Log($"Platforma porusza siê do {targetPoint.position}");

        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            if (pingPong)
            {
                HandlePingPongMovement();
            }
            else
            {
                HandleLoopMovement();
            }
        }
    }

    private void HandleLoopMovement()
    {
        currentPointIndex++;

        if (currentPointIndex >= points.Length)
        {
            if (loop)
            {
                currentPointIndex = 0; // Restart
            }
            else
            {
                currentPointIndex = points.Length - 1; // Nie idŸ dalej
            }
        }
    }

    private void HandlePingPongMovement()
    {
        if (movingForward)
        {
            currentPointIndex++;
            if (currentPointIndex >= points.Length)
            {
                currentPointIndex = points.Length - 2;
                movingForward = false;
            }
        }
        else
        {
            currentPointIndex--;
            if (currentPointIndex < 0)
            {
                currentPointIndex = 1;
                movingForward = true;
            }
        }
    }
}
