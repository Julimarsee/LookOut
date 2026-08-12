using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float sensitivityX = 2f;
    public float sensitivityY = 2f;
    public float minPitchY = -30f;
    public float maxPitchY = 40f;
    public float minPitchX = 0f;
    public float maxPitchX = 200f;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public bool lockControlsWhileMoving = true; 
    private float rotationX = 0f;
    private float rotationY = 0f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Quaternion startRotation;
    private Quaternion targetRotation;
    private bool isMoving = false;
    private bool isAtTarget = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 angels = transform.eulerAngles;
        rotationX = angels.x;
        rotationY = angels.y;

        startPosition = transform.position;
        startRotation = transform.rotation;
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    void Update()
    {
        if (!isMoving || !lockControlsWhileMoving)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivityX;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

            rotationX -= mouseY;
            rotationY += mouseX;

            rotationX = Mathf.Clamp(rotationX, minPitchY, maxPitchY);
            rotationY = Mathf.Clamp(rotationY, minPitchX, maxPitchX);

            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
        }

        if (isMoving)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                Time.deltaTime * moveSpeed
            );

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * moveSpeed
            );

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                transform.rotation = targetRotation;
                isMoving = false;
            }
        }
    }

    public void MoveToObject(Transform targetObject)
    {
        if (targetObject == null)
            return;

        MoveToPosition(targetObject.position, targetObject.rotation);
    }

    public void MoveToPosition(Vector3 position, Quaternion rotation)
    {
        targetPosition = position;
        targetRotation = rotation;
        isMoving = true;
        isAtTarget = true;
    }

    public void ReturnToStart()
    {
        targetPosition = startPosition;
        targetRotation = startRotation;
        isMoving = true;
        isAtTarget = false;
    }

    public bool IsAtTarget()
    {
        return isAtTarget;
    }
}