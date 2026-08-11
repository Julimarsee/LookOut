using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float sensitivityX = 2f;
    public float sensitivityY = 2f;

    public float minPitchY = -30f;
    public float maxPitchY = 40f;

    public float minPitchX = 0f;
    public float maxPitchX = 200f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 angels = transform.eulerAngles;  
        rotationX = angels.x; 
        rotationY = angels.y;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

        rotationX -= mouseY;
        rotationY += mouseX;

        rotationX = Mathf.Clamp(rotationX, minPitchY, maxPitchY);
        rotationY = Mathf.Clamp(rotationY, minPitchX, maxPitchX);

        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }
}
