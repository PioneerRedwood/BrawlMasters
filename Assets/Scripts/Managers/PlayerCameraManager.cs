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
            // ������ ���� �������� ī�޶� ������ �� �ְ� �ؾ��� 
            // ��������, +X-X-Z+Z
            // X: -11 ~ 60, Z: -30 ~ 50
            // ���� �÷��̾ �ϼ��� �Ѱ輱�� ������ ���̻� ī�޶�� +Z, -X �������δ� �� ������
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
