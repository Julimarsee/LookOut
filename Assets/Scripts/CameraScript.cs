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

    [Header("Zoom Settings")]
    public bool disableRotationWhenZoomed = true;

    private float rotationX = 0f;
    private float rotationY = 0f;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private Vector3 previousPosition;
    private Quaternion previousRotation;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private bool isMoving = false;
    private bool isAtTarget = false;
    private bool isRotationLocked = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 angels = transform.eulerAngles;
        rotationX = angels.x;
        rotationY = angels.y;

        startPosition = transform.position;
        startRotation = transform.rotation;

        previousPosition = startPosition;
        previousRotation = startRotation;

        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isAtTarget)
        {
            ReturnToPreviousPosition();
        }

        bool canRotate = !isMoving || !lockControlsWhileMoving;

        if (disableRotationWhenZoomed && isAtTarget && !isMoving)
        {
            canRotate = false;
        }

        if (canRotate)
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

                if (disableRotationWhenZoomed && isAtTarget)
                {
                    isRotationLocked = true;
                }
            }
        }
    }

    public void MoveToObject(Transform targetObject)
    {
        if (targetObject == null)
            return;

        previousPosition = transform.position;
        previousRotation = transform.rotation;

        Vector3 currentAngles = transform.eulerAngles;
        previousRotation = Quaternion.Euler(currentAngles.x, currentAngles.y, 0f);

        MoveToPosition(targetObject.position, targetObject.rotation);
    }

    public void MoveToPosition(Vector3 position, Quaternion rotation)
    {
        targetPosition = position;
        targetRotation = rotation;
        isMoving = true;
        isAtTarget = true;
        isRotationLocked = false;
    }

    public void ReturnToPreviousPosition()
    {
        targetPosition = previousPosition;
        targetRotation = previousRotation;
        isMoving = true;
        isAtTarget = false;
        isRotationLocked = false;

        Vector3 angles = previousRotation.eulerAngles;
        rotationX = angles.x;
        rotationY = angles.y;
    }

    public void ZoomToObject(Transform targetObject)
    {
        if (IsAtTarget())
            ReturnToPreviousPosition();
        else
            MoveToObject(targetObject);
    }

    public bool IsAtTarget()
    {
        return isAtTarget;
    }
}