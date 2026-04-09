using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelToGoal : MonoBehaviour
{

    public Transform goal;
    float speed = 5;
    
    void Start()
    {
        
    }

    void LateUpdate()
    {
        Vector3 direction = goal.position - this.transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }
}
