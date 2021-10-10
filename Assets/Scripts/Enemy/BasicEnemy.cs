using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    GameObject target;

    void Start()
    {
        target = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        //gameObject.transform.position = 
        //    Vector3.Lerp(gameObject.transform.position, target.transform.position, Time.time);

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 0.03f);
    }
}
