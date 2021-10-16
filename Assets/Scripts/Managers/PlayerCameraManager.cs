using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    private Transform playerPos;
    private Rigidbody cameraBody;

    private float[] axisLimits;

    void Start()
    {
        playerPos = GameObject.FindWithTag("Player").GetComponent<Transform>();
        cameraBody = Camera.main.gameObject.GetComponent<Rigidbody>();

        axisLimits = new float[]{ -11, 60, -30, 53 };
        //InvokeRepeating("CameraMove", 0.0f, 0.01f);
    }

    void Update()
    {
        Vector2 playerPosition = new Vector2(playerPos.position.x, playerPos.position.z);
        Vector2 mainCamPos = new Vector2(cameraBody.transform.position.x, cameraBody.transform.position.z);

        if (Vector2.Distance(mainCamPos, playerPosition) > 0.01f)
		{
            // 정해진 범위 내에서만 카메라가 움직일 수 있게 해야함 
            // 동서남북, +X-X-Z+Z
            // X: -11 ~ 60, Z: -30 ~ 50
            // 만약 플레이어가 북서쪽 한계선에 있으면 더이상 카메라는 +Z, -X 방향으로는 못 가도록
            float moveX = playerPosition.x, moveZ = playerPosition.y;
            if((playerPosition.x <= axisLimits[0]) || (playerPosition.x >= axisLimits[1]))
			{
                moveX = mainCamPos.x;
			}
            if((playerPosition.y <= axisLimits[2]) || (playerPosition.y >= axisLimits[3]))
			{
                moveZ = mainCamPos.y;
			}

            cameraBody.MovePosition(new Vector3(moveX, cameraBody.position.y, moveZ));
        }

    }
}
