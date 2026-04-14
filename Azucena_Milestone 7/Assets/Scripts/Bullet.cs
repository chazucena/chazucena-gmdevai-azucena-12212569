using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public GameObject explosion;
    public float damage = 20f;

    void OnCollisionEnter(Collision col)
    {
        // Try to damage TankAI
        TankAI tank = col.gameObject.GetComponent<TankAI>();
        if (tank != null)
        {
            tank.TakeDamage(damage);
        }

        // Try to damage Player
        Drive player = col.gameObject.GetComponent<Drive>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }

        // Explosion effect
        GameObject e = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(e, 1.5f);

        // Destroy bullet
        Destroy(gameObject);
    }

    void Start() { }

    void Update() { }
}
