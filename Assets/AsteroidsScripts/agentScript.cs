using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class agentScript : Agent
{

    [SerializeField] private Transform target;
    [SerializeField] private Transform bulletSpawnPoint;
    public float boundaryX = 100f;
    public float boundaryY = 0.01f;
    public float obstacleDetectionDistance = 2f;

    public GameObject bulletPrefab;
    private float bulletSpeed = 10f; // Adjust the bullet speed as needed

    private float survivalTime; // Variable to track the agent's survival time

    public override void OnEpisodeBegin()
    {
        // Set the player's position to a specific location
        transform.localPosition = new Vector3(0f, 0f, 0f);
        survivalTime = 0f; // Reset survival time at the beginning of each episode
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation((Vector2)transform.localPosition);
        sensor.AddObservation((Vector2)target.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveForward = actions.ContinuousActions[0];
        float rotate = actions.ContinuousActions[1];
        float shootingIntensity = actions.ContinuousActions[2];

        float movementSpeed = 2.5f;
        float rotationSpeed = 250f;

        // Move the agent forward
        transform.Translate(Vector3.up * Mathf.Abs(moveForward) * Time.deltaTime * movementSpeed);

        // Rotate the agent
        transform.Rotate(Vector3.forward, -rotate * Time.deltaTime * rotationSpeed);

        // Keep the agent within bounds
        ClampToBounds();

        DetectObstacles();

        if (shootingIntensity > 0.5f)  // Adjust the threshold as needed
        {
            Shoot();
        }
        // Update the rewards based on the agent's survival time
        AddReward(0.01f); // Small reward for each frame survived
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;

        // Move forward when the "Vertical" axis is pressed
        continuousActions[0] = Input.GetAxis("Vertical");

        // Rotate right when the 'D' key is pressed and left when the 'A' key is pressed
        continuousActions[1] = 0f;  // Reset rotation
        if (Input.GetKey(KeyCode.D))
        {
            continuousActions[1] = 1f;  // Rotate right
        }
        else if (Input.GetKey(KeyCode.A))
        {
            continuousActions[1] = -1f;  // Rotate left
        }

        continuousActions[2] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Target target))
        {
            AddReward(10f);
            EndEpisode();
        }
        else if (collision.TryGetComponent(out Wall wall))
        {
            AddReward(-2f);
            EndEpisode();
        }
        else if (collision.CompareTag("Obstacle")) // Check if the collider has the "Obstacle" tag
        {
            AddReward(-5f); // Penalty for colliding with an asteroid
            EndEpisode();
        }
    }

    private void Update()
    {
        survivalTime += Time.deltaTime; // Increment survival time in each frame
    }

    private void DetectObstacles()
    {
        // Detect obstacles on both sides within the specified detection distance
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, -transform.right, obstacleDetectionDistance, LayerMask.GetMask("Obstacle"));
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, transform.right, obstacleDetectionDistance, LayerMask.GetMask("Obstacle"));

        // Apply rewards or penalties based on obstacle detection
        if (leftHit.collider != null && leftHit.collider.CompareTag("Obstacle"))
        {
            AddReward(-1f); // Apply a penalty for being close to an obstacle on the left
        }

        if (rightHit.collider != null && rightHit.collider.CompareTag("Obstacle"))
        {
            AddReward(-1f); // Apply a penalty for being close to an obstacle on the right
        }
    }

    private void ClampToBounds()
    {
        // Keep the agent within bounds
        Vector3 clampedPosition = transform.localPosition;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -boundaryX, boundaryX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -boundaryY, boundaryY);
        transform.localPosition = clampedPosition;
    }

    private void Shoot()
    {
        // Create a bullet and set its velocity
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        // Set initial velocity based on the rotation of the bulletSpawnPoint
        bulletRb.velocity = bulletSpawnPoint.up * bulletSpeed; // Adjust bulletSpeed as needed

        // Destroy the bullet after a certain time (adjust as needed)
        Destroy(bullet, 2f);

        // Apply a small penalty for shooting (encourages more strategic shooting)
        AddReward(-0.1f);
    }
}
