using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MarioAgent : Agent
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] runSprites;
    public Sprite climbSprite;
    private int spriteIndex;

    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    private Collider2D[] overlaps = new Collider2D[4];
    private Vector2 direction;

    private bool grounded;
    private bool climbing;
    private bool climbed = false;

    private bool ladderTop = false;
    private bool ladderTop2 = false;

    public float moveSpeed = 3f;
    public float jumpStrength = 4f;

    private bool first = false; 

    private bool second = false; 

    private bool third = false; 

    private bool fourth = false; 

    private bool fifth = false; 

    private bool sixth = false; 

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    /*private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 1f/12f, 1f/12f);
    }*/

    /*private void OnDisable()
    {
        CancelInvoke();
    }*/

    public override void OnEpisodeBegin() {
        transform.localPosition = new Vector3(-5.25f, -5f, 0f);
        first = false;
        second = false;
        third = false;
        fourth = false;
        fifth = false;
        sixth = false;

        //Purely for training, comment during actual gameplay
        Barrel[] barrels = FindObjectsOfType<Barrel>();
        for(int i = 0; i < barrels.Length; i++) {
            Destroy(barrels[i].gameObject);
        }
    }

    private void Update()
    {
        CheckCollision();
        AnimateSprite();
        
    }

    public override void CollectObservations(VectorSensor sensor) {
        //sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(transform.localPosition.y);
    }

   public override void OnActionReceived(ActionBuffers actions) {
        int moveX = actions.DiscreteActions[0];
        int moveY = actions.DiscreteActions[1];
        int jump = actions.DiscreteActions[2];
        
        if(moveY == 1 && climbing) {
            direction.y = moveSpeed;
        } else if(moveY == 2 && climbing) {
            direction.y = -1 * moveSpeed;
        } else if(jump == 1 && grounded) {
            direction = Vector2.up * jumpStrength;
        } else {
            direction+= Physics2D.gravity * Time.deltaTime;
        }

        if(moveX == 0) {
            direction.x = moveSpeed;
        } 
        if(moveX == 1) {
            direction.x = -1 * moveSpeed;
        }

        if (grounded) {
            direction.y = Mathf.Max(direction.y, -1f);
        }

        if (direction.x > 0f) {
            transform.eulerAngles = Vector3.zero;
        } else if (direction.x < 0f) {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    } 

    /*public override void OnActionReceived(ActionBuffers actions) {
    int moveX = actions.DiscreteActions[0];
    int moveY = actions.DiscreteActions[1];
    int jump = actions.DiscreteActions[2];

    if(moveY == 1 && climbing) {
        direction.y = moveSpeed;
    } else if(moveY == 2 && climbing) {
        direction.y = -1 * moveSpeed;
    } else if(jump == 1 && grounded) {
        jump = 1;
    } else {
        direction+= Physics2D.gravity * Time.deltaTime;
    }

    if(moveX == 1) {
        direction.x = moveSpeed;
    } else if(moveX == 2) {
        direction.x = -1 * moveSpeed;
    }

    if (grounded && jump != 1) {
        direction.y = Mathf.Max(direction.y, -1f);
    }

    if (direction.x > 0f) {
        transform.eulerAngles = Vector3.zero;
    } else if (direction.x < 0f) {
        transform.eulerAngles = new Vector3(0f, 180f, 0f);
    }
}*/

    private void CheckCollision()
    {
        grounded = false;
        climbing = false;

        // the amount that two colliders can overlap
        // increase this value for steeper platforms
        float skinWidth = 0.1f;

        Vector2 size = collider.bounds.size;
        size.y += skinWidth;
        size.x /= 2f;

        int amount = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, overlaps);

        for (int i = 0; i < amount; i++)
        {
            GameObject hit = overlaps[i].gameObject;

            if (hit.layer == LayerMask.NameToLayer("Ground"))
            {
                if(first || second || third || fourth || fifth || sixth) {
                    Debug.Log("Moved Down to ground");
                    AddReward(-10f);
                    EndEpisode();
                }
                
                if(overlaps[i] == null) continue;
                // Only set as grounded if the platform is below the player
                grounded = hit.transform.position.y < (transform.position.y - 0.5f + skinWidth);

                // Turn off collision on platforms the player is not grounded to
                Physics2D.IgnoreCollision(overlaps[i], collider, climbing);
                //Physics2D.IgnoreCollision(overlaps[i], collider, !grounded);
                if(direction.x > 0f && ladderTop == false) {
                    AddReward(2f);
                } else if(direction.x < 0f && ladderTop == false) {
                    AddReward(-1f);
                }
            } else if(hit.layer == LayerMask.NameToLayer("Ground1")) {
                if(second || third || fourth || fifth || sixth) {
                    Debug.Log("Moved Down to first");
                    AddReward(-10f);
                    EndEpisode();
                }

                if(ladderTop == true && first == false) {
                    first = true;
                    Debug.Log("Hit ladder top at first, first is now true");
                    AddReward(5f);
                    climbed = false;
                }

                //Give the agent a reward for moving left on the first platform
                if(direction.x < 0f && first && ladderTop) {
                    AddReward(3f);
                } else if(first && ladderTop && direction.x > 0f) {
                    AddReward(-5f);
                }

                grounded = hit.transform.position.y < (transform.position.y - 0.5f + skinWidth);
                Physics2D.IgnoreCollision(overlaps[i], collider, climbing);
            } else if(hit.layer == LayerMask.NameToLayer("Ground2") && first && climbed) {
                if(third || fourth || fifth || sixth) {
                    Debug.Log("Moved Down to second");
                    AddReward(-10f);
                    EndEpisode();
                }

                if(ladderTop2 == true && second == false) {
                    second = true;
                    Debug.Log("Hit ladder top at second, second is now true");
                    AddReward(7f);
                    climbed = false;
                }

                if(direction.x > 0f && second == true ||  direction.x > 0f && ladderTop2 == true) {
                    AddReward(2f);
                } else if(second == true && ladderTop2 == true) {
                    AddReward(-1f);
                }

                grounded = hit.transform.position.y < (transform.position.y - 0.5f + skinWidth);
                Physics2D.IgnoreCollision(overlaps[i], collider, climbing);
                AddReward(12f);
            } else if(hit.layer == LayerMask.NameToLayer("Ground3") && third == false) {
                third = true;
                grounded = hit.transform.position.y < (transform.position.y - 0.5f + skinWidth);
                Physics2D.IgnoreCollision(overlaps[i], collider, climbing);
                AddReward(14f);
            } else if(hit.layer == LayerMask.NameToLayer("Ground4") && fourth == false) {
                fourth = true;
                grounded = hit.transform.position.y < (transform.position.y - 0.5f + skinWidth);
                Physics2D.IgnoreCollision(overlaps[i], collider, climbing);
                AddReward(16f);
            } else if(hit.layer == LayerMask.NameToLayer("Ground5") && fifth == false) {
                fifth = true;
                grounded = hit.transform.position.y < (transform.position.y - 0.5f + skinWidth);
                Physics2D.IgnoreCollision(overlaps[i], collider, climbing);
                AddReward(18f);
            } else if(hit.layer == LayerMask.NameToLayer("Ground6") && sixth == false) {
                sixth = true;
                grounded = hit.transform.position.y < (transform.position.y - 0.5f + skinWidth);
                Physics2D.IgnoreCollision(overlaps[i], collider, climbing);
                AddReward(20f);
            }
            else if (hit.layer == LayerMask.NameToLayer("Ladder"))
            {
                climbing = true;
                climbed = true;

            } 
            if (hit.layer == LayerMask.NameToLayer("LadderTop") && ladderTop == false)
            {
                Debug.Log("Hit ladder top");
                AddReward(5f);
                ladderTop = true;
            } 
            if (hit.layer == LayerMask.NameToLayer("LadderTop2") && ladderTop2 == false)
            {
                Debug.Log("Hit ladder top 2");
                AddReward(5f);
                ladderTop2 = true;
            }
            //If laddertop is true nad the agents X value is less than 0 and the y is less than -4.6 end episode
            if(ladderTop == true && transform.localPosition.x < 0 && transform.localPosition.y < -4.6f) {
                Debug.Log("Moved Down to ground");
                AddReward(-10f);
                EndEpisode();
            }
        }
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + direction * Time.fixedDeltaTime);
        //rigidbody.velocity = direction;
    }

    private void AnimateSprite()
    {
        if (climbing)
        {
            spriteRenderer.sprite = climbSprite;
        }
        else if (direction.x != 0f)
        {
            spriteIndex++;

            if (spriteIndex >= runSprites.Length) {
                spriteIndex = 0;
            }

            if (spriteIndex > 0 && spriteIndex <= runSprites.Length) {
                spriteRenderer.sprite = runSprites[spriteIndex];
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Objective"))
        {
          /*  
          Commented out for Training purposes
          enabled = false;
            FindObjectOfType<DKGameManager>().LevelComplete();*/
            AddReward(100f);
            EndEpisode();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            /* 
            Commented out for Training purposes
            enabled = false;
            FindObjectOfType<DKGameManager>().LevelFailed();*/
            AddReward(-5f);
            EndEpisode();
        }
    }

}
