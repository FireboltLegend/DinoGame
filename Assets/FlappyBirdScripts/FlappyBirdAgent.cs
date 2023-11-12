using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class FlappyBirdAgent : Agent
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int spriteIndex;
    private Vector3 direction;

    public float gravity = -9.8f;

    public float strength = 5f;

    private bool isJumpInputDown;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    public override void OnEpisodeBegin()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;

        /* FOR TRAINING */
        FlappyBirdPipes[] pipes = FindObjectsOfType<FlappyBirdPipes>();

        for (int i = 0; i < pipes.Length; i++) {
            Destroy(pipes[i].gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            isJumpInputDown = true;
        }
        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;
    }

    public override void CollectObservations(VectorSensor sensor) 
    {
        sensor.AddObservation(transform.position.y);

        FlappyBirdPipes[] pipes = FindObjectsOfType<FlappyBirdPipes>();
        for (int i = 0; i < pipes.Length; i++) {
            if (pipes[i].transform.position.x > 0) {
                sensor.AddObservation(pipes[i].transform.position.x);
            }
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int move = actions.DiscreteActions[0];
        if (move == 1) {
            direction = Vector3.up * strength;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = isJumpInputDown ? 1 : 0;

        isJumpInputDown = false;
    }

    private void AnimateSprite()
    {
        spriteIndex++;
        if (spriteIndex >= sprites.Length) {
            spriteIndex = 0;
        }

        spriteRenderer.sprite = sprites[spriteIndex];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Obstacle") {
            //FindObjectOfType<GameManager>().GameOver(); //comment this out to train
            /* FOR TRAINING */
            AddReward(-5f);
            EndEpisode();
        }
        else if (other.gameObject.tag == "Scoring") {
            //FindObjectOfType<GameManager>().IncreaseScore(); //comment this out to train
            /* FOR TRAINING */
            AddReward(10f);
        }
    }
}
