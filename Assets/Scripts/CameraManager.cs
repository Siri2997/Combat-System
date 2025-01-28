using UnityEngine;
//using Cinemachine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private CinemachineCamera virtualCamera;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (virtualCamera == null)
        {
            virtualCamera = FindAnyObjectByType<CinemachineCamera>();
        }
    }

    private void Start()
    {
        // Set the initial target for the camera
        //FocusOnTarget(GameManager.Instance.GetRandomTarget());
    }

    // Focus on the attacking player
    public void FocusOnPlayer(Transform playerTransform)
    {
        if (virtualCamera != null)
        {
            virtualCamera.Follow = playerTransform;
            virtualCamera.LookAt = playerTransform;
        }
    }

    // Focus on the target being attacked
    public void FocusOnTarget(Transform targetTransform)
    {
        if (virtualCamera != null)
        {
            virtualCamera.Follow = targetTransform;
            virtualCamera.LookAt = targetTransform;
        }
    }
}
