using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    /// <BulletStat>
    /// Speed:  10.0f
    /// Damage: 10.0f
    /// Owner:  Who has this
    /// Type:   Basic Bullet; Now, not specified how to use it.
    /// 
    /// </BulletStat>

    public float Speed = 10.0f;
    public float Damage = 10.0f;
    private BrawlerController Owner = null;
    public string Type = "Basic";

    private Vector3 SpawnedPosition;
    private Vector3 Destination;
    public float Distance = 50.0f;

    public void SetStatOnSpawn(BrawlerController owner, Vector3 spawnedPosition, Vector3 destination)
    {
        Owner = owner;
        SpawnedPosition = spawnedPosition;
        Destination = destination;

        transform.SetPositionAndRotation(spawnedPosition, Quaternion.identity);
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (Owner != null)
        {
            // Do your work
            transform.Translate(Destination * Speed * Time.deltaTime);
        }

        if(Vector3.Distance(transform.position, SpawnedPosition) >= Distance)
        {
            Destroy(gameObject);
        }
    }

    void MoveToDest()
    {
        Vector3.MoveTowards(SpawnedPosition, Destination, 100.0f);
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(Owner.gameObject.name + " " + other.gameObject.name);
        Destroy(gameObject);
    }
}
