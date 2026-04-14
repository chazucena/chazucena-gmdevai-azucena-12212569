using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankAI : MonoBehaviour
{
    Animator anim;
    NavMeshAgent agent;

    public GameObject player;
    public GameObject bullet;
    public GameObject turret;

    // HP system
    public float maxHP = 100f;
    public float currentHP;

    // Flee settings
    public float fleeThreshold = 0.2f; // 20%
    private bool isFleeing = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        currentHP = maxHP;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        anim.SetFloat("distance", distance);

        // Check HP threshold
        if (!isFleeing && currentHP <= maxHP * fleeThreshold)
        {
            StartFleeing();
        }

        // If fleeing continue run
        if (isFleeing)
        {
            Flee(player.transform.position);
        }
    }

    // Shooting
    void Fire()
    {
        GameObject b = Instantiate(bullet, turret.transform.position, turret.transform.rotation);
        b.GetComponent<Rigidbody>().AddForce(turret.transform.forward * 500);
    }

    public void StartFiring()
    {
        if (!isFleeing)
            InvokeRepeating("Fire", 0.5f, 0.5f);
    }

    public void StopFiring()
    {
        CancelInvoke("Fire");
    }

    // Damage function
    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        StopFiring();
        Destroy(gameObject);
    }

    // Start fleeing
    void StartFleeing()
    {
        isFleeing = true;
        StopFiring(); // stop attacking
        anim.SetBool("isFleeing", true); // optional Animator param
    }

    // Flee behavior
    void Flee(Vector3 targetPosition)
    {
        Vector3 fleeDirection = transform.position - targetPosition;
        Vector3 newPosition = transform.position + fleeDirection;

        agent.SetDestination(newPosition);
    }

    public GameObject GetPlayer()
    {
        return player;
    }
}
