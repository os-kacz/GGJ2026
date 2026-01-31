using UnityEngine;

public class FollowCamera : MonoBehaviour
{
  [SerializeField] Transform playerTransform;
  [SerializeField] Vector3 offset;
  [SerializeField] float smoothSpeed;
  public Camera m_camera;
  public float cameraSize;

    void Start()
    {
        m_camera.orthographicSize = cameraSize;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = playerTransform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
        
    }
}
