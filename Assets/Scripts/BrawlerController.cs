using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrawlerController : MonoBehaviour
{
    /// <BrawlerStat>
    /// HealthPoint:    100
    /// MoveSpeed:      2
    /// AttackSpeed:    1
    /// UltimateCharge: 100
    /// ReloadTime:     0.5
    /// Magazine:       3
    /// </BrawlerStat>

    public float MoveSpeed = 2.0f;
    public float ReloadTime = 0.5f;
    public uint Magazine = 3;
    private uint RestMagazine = 3;

    public Text TextMagazine;
    public Transform MuzzlePosition;


    private float LastShootingTime = 0.0f;
    public float ShootingDelay = 0.25f;
    public GameObject Bullet;


    void Start()
    {
        LastShootingTime = Time.realtimeSinceStartup;
    }

    void Update()
    {
        Movement();

        Shooting();

        // Reloading
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine("Reloading");
        }
    }

    void Movement()
    {
        // Movement
        float xAxis = Input.GetAxisRaw("Horizontal");
        float zAxis = Input.GetAxisRaw("Vertical");

        gameObject.transform.Translate(Vector3.forward * MoveSpeed * zAxis * Time.deltaTime);
        gameObject.transform.Translate(Vector3.right * MoveSpeed * xAxis * Time.deltaTime);


    }

    void Shooting()
    {
        if (Input.GetMouseButton(1))
        {
            if (RestMagazine > 0)
            {
                if ((LastShootingTime + ShootingDelay) <= Time.realtimeSinceStartup)
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit))
                    {
                        //Debug.Log("hit.point: " + hit.point);

                        RestMagazine -= 1;
                        UpdateState();
                        LastShootingTime = Time.realtimeSinceStartup;

                        GameObject bullet = Instantiate(Bullet, MuzzlePosition);
                        bullet.GetComponent<Bullet>().SetStatOnSpawn(this, MuzzlePosition.position, transform.forward * 1.5f);
                        bullet.transform.SetParent(GameObject.Find("Spawned Object").transform);
                        NetworkManager.instance.SendData(gameObject.name + " make " + bullet.name);
                    }
                }
                else
                {
                    //Debug.Log("Reloading..");
                }
            }
            else
            {
                //Debug.Log("There is no rest magazine.");
            }
        }
    }

    IEnumerator Reloading()
    {
        // Just don't need to do this ..
        //if (RestMagazine == Magazine)
        //{
        //    StopCoroutine("Reloading");
        //    yield return null;
        //}

        if (RestMagazine < Magazine)
        {
            RestMagazine += 1;
            UpdateState();
            yield return new WaitForSeconds(0.5f);
            StartCoroutine("Reloading");
        }
    }

    void UpdateState()
    {
        TextMagazine.text = Magazine + " / " + RestMagazine;
    }
}
