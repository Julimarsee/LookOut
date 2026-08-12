using UnityEngine;

public class ComputerCameras : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform targetObject;
    [SerializeField] private CameraScript cameraScript;

    public void Interact()
    {
        if (cameraScript.IsAtTarget())
            cameraScript.ReturnToStart();
        else
            cameraScript.MoveToObject(targetObject);
    }
}