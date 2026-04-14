using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour
{

    public float speed = 10.0F;
    public float rotationSpeed = 100.0F;

    public GameObject bullet;
    public Transform PlayerTurret;
    public float fireForce = 500f;

    //  HP system
    public float maxHP = 100f;
    public float currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    void Update()
    {
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
    }

    void Fire()
    {
        GameObject b = Instantiate(bullet, PlayerTurret.position, PlayerTurret.rotation);
        b.GetComponent<Rigidbody>().AddForce(PlayerTurret.forward * fireForce);
    }

    // Player takes damage (for later use)
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
        Debug.Log("Player Died");
        // You can add respawn or game over here
    }
}
