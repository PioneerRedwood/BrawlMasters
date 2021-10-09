using UnityEngine;

public enum SinglePlayerCameraMode
{
    Stationary,
    FollowAndOrbit
}

public class CameraManager : Singleton<CameraManager>
{
    [Header("Component References")]
    public GameObject gameplayCameraObject;
    public GameObject uiOverlayCameraObject;

    [Header("Virtual Cameras")]
    public GameObject VCamStationaryObect;
    public GameObject VCamSinglePlayerOrbitObject;

    public void SetupManager()
    {
        SetCameraObjectNewState(gameplayCameraObject, true);
        SetCameraObjectNewState(uiOverlayCameraObject, false);
    }

    public void SetupSinglePlayerCamera(SinglePlayerCameraMode currentCameraMode)
    {
        switch (currentCameraMode)
        {
            case SinglePlayerCameraMode.Stationary:
                SetCameraObjectNewState(VCamStationaryObect, true);
                SetCameraObjectNewState(VCamSinglePlayerOrbitObject, false);
                break;
            case SinglePlayerCameraMode.FollowAndOrbit:
                SetCameraObjectNewState(VCamStationaryObect, false);
                SetCameraObjectNewState(VCamSinglePlayerOrbitObject, false);
                break;
        }
    }

    void SetCameraObjectNewState(GameObject cameraObject, bool newState)
    {
        cameraObject.SetActive(newState);
    }

    // 
    public Transform GetGameplayCameraTransform()
    {
        return gameplayCameraObject.transform;
    }

    public Camera GetGameplayCamera()
    {
        return gameplayCameraObject.GetComponent<Camera>();
    }
}
