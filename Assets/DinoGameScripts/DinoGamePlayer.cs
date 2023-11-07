using UnityEngine;
using UnityEngine.Timeline;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.TextCore.Text;
using static Google.Protobuf.WellKnownTypes.Field;

public class DinoGamePlayer : Agent
{
    private CharacterController character;
    private Vector3 direction;

    public float gravity = 9.81f * 2f;
    public float jumpForce = 8f;

    private bool isJumpInputDown;

    public override void OnEpisodeBegin()
    {
        character = GetComponent<CharacterController>();
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;

        DinoGameObstacle[] cactus = FindObjectsOfType<DinoGameObstacle>();

        for (int i = 0; i < cactus.Length; i++)
        {
            Destroy(cactus[i].gameObject);
        }
    }
    private void Update()
    {
        direction += Vector3.down * gravity * Time.deltaTime;

        if(character.isGrounded)
        {
            direction = Vector3.down;
            
            if(Input.GetButton("Jump"))
            {
                direction = Vector3.up * jumpForce;
            }
        }

        character.Move(direction * Time.deltaTime);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position.y);
        DinoGameObstacle[] cactus = FindObjectsOfType<DinoGameObstacle>();
        for (int i = 0; i < cactus.Length; i++)
        {
            if (cactus[i].transform.position.x > 0)
            {
                sensor.AddObservation(cactus[i].transform.position.x);
            }
        }
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        int move = actions.DiscreteActions[0];
        if (move == 1)
        {
            direction = Vector3.up * jumpForce;
            character.Move(direction * Time.deltaTime);
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = isJumpInputDown ? 1 : 0;

        isJumpInputDown = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Obstacle"))
        {
            AddReward(-2f);
            EndEpisode();
            // DinoGameManager.Instance.GameOver();
        }
        else
        {
            AddReward(1f);
        }
    }
}
