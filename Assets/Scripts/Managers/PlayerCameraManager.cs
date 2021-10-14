using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    private Transform playerPos;
    private Rigidbody cameraBody;

    void Start()
    {
        playerPos = GameObject.FindWithTag("Player").GetComponent<Transform>();
        cameraBody = Camera.main.gameObject.GetComponent<Rigidbody>();
		InvokeRepeating("CameraMove", 0.0f, 0.01f);
	}

	void CameraMove()

	//void FixedUpdate()
    {
        Vector2 playerPosition = new Vector2(playerPos.position.x, playerPos.position.z);
        Vector2 mainCamPos = new Vector2(cameraBody.transform.position.x, cameraBody.transform.position.z);

        if (Vector2.Distance(mainCamPos, playerPosition) > 0.01f)
		{
            cameraBody.MovePosition(new Vector3(playerPosition.x, cameraBody.position.y, playerPosition.y));
        }

    }
}
