using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{

    GameObject[] goalLocations;
    NavMeshAgent agent;
    Animator anim;
    float speedMultiplier;
    float detectionRadius = 20; //Pull Radius
    float attractRadius = 10f;

    void ResetAgent()
    {
        speedMultiplier = Random.Range(0.1f, 1.5f);
        agent.speed = 2 * speedMultiplier;
        agent.angularSpeed = 120;
        anim.SetFloat("speedMultiplier", speedMultiplier);
        anim.SetTrigger("isWalking");
        agent.ResetPath();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        goalLocations = GameObject.FindGameObjectsWithTag("goal");
        agent = this.GetComponent<NavMeshAgent>();
        
        agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        anim = this.GetComponent<Animator>();
        anim.SetTrigger("isWalking");
        anim.SetFloat("wOffset", Random.Range(0.1f, 1f));

        ResetAgent();
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.remainingDistance < 1)
        {
            ResetAgent();
            agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);

        }
    }

    public void DetectNewObstacle(Vector3 location)
    {
        if (Vector3. Distance(location, this.transform.position) < detectionRadius)
        {
            Vector3 attractDirection = (location - transform.position).normalized;
            Vector3 newGoal = transform.position + attractDirection * attractRadius; // Attraction Script

            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(newGoal, path);

            if (path.status != NavMeshPathStatus.PathInvalid)
            {
                agent.SetDestination(path.corners[path.corners.Length - 1]);
                anim.SetTrigger("isRunning");
                agent.speed = 10;
                agent.angularSpeed = 500;
            }
        }
    }
}
