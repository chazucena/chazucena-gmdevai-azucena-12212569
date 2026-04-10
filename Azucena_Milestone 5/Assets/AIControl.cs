using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{
    NavMeshAgent agent;

    public GameObject target;
    public WASDMovement playerMovement;

    // Detection ranges
    public float detectionRange = 15f;
    public float loseRange = 18f;

    bool isChasing = false;

    Vector3 wanderTarget;

    // Behavior selector
    public enum AIBehavior
    {
        Pursue, Hide, Evade
    }

    public AIBehavior behaviorType;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        playerMovement = target.GetComponent<WASDMovement>();

        wanderTarget = transform.position;
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeDirection = location - this.transform.position;
        agent.SetDestination(this.transform.position - fleeDirection);
    }

    void Pursue()
    {
        Vector3 targetDirection = target.transform.position - this.transform.position;
        float lookAhead = targetDirection.magnitude / (agent.speed + playerMovement.currentSpeed);

        Seek(target.transform.position + target.transform.forward * lookAhead);
    }

    void Evade()
    {
        Vector3 targetDirection = target.transform.position - this.transform.position;
        float lookAhead = targetDirection.magnitude / (agent.speed + playerMovement.currentSpeed);

        Flee(target.transform.position + target.transform.forward * lookAhead);
    }

    void Wander()
    {
        float wanderRadius = 20f;
        float wanderDistance = 20f;
        float wanderJitter = 1f;

        wanderTarget += new Vector3(
            Random.Range(-1.0f, 1.0f) * wanderJitter,
            0,
            Random.Range(-1.0f, 1.0f) * wanderJitter
        );

        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = transform.TransformPoint(targetLocal);

        Seek(targetWorld);
    }

    void Hide()
    {
        float distance = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        GameObject[] hidingSpots = World.Instance.GetHidingSpots();

        for (int i = 0; i < hidingSpots.Length; i++)
        {
            Vector3 hideDirection = hidingSpots[i].transform.position - target.transform.position;
            Vector3 hidePosition = hidingSpots[i].transform.position + hideDirection.normalized * 5;

            float spotDistance = Vector3.Distance(this.transform.position, hidePosition);

            if (spotDistance < distance)
            {
                chosenSpot = hidePosition;
                distance = spotDistance;
            }
        }

        Seek(chosenSpot);
    }

    void ChaseConditions()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);

        // State switching
        if (isChasing)
        {
            if (distance > loseRange)
                isChasing = false;
        }
        else
        {
            if (distance <= detectionRange)
                isChasing = true;
        }

        if (isChasing)
        {
            switch (behaviorType)
            {
                case AIBehavior.Pursue:
                    Pursue();
                    break;

                case AIBehavior.Hide:
                    Hide();
                    break;

                case AIBehavior.Evade:
                    Evade();
                    break;
            }
        }
        else
        {
            Wander();
        }
    }

    void Update()
    {
        ChaseConditions();
    }
}
