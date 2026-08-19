using UnityEngine;
using UnityEngine.UI;

public class RaycastOutline : MonoBehaviour
{
    public Image Scope;
    public Sprite InteractScope;
    public Sprite SimpleScope;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private float outlineWidth = 10f;
    private float maxRayDistance = 4f;
    
    public Outline lastOutlinedObject;

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, maxRayDistance))
        {
            if (hit.transform.gameObject.CompareTag("Item"))
            {
                if (lastOutlinedObject != null)
                {
                    lastOutlinedObject.OutlineWidth = 0;
                    Scope.sprite = SimpleScope;
                }

                lastOutlinedObject = hit.transform.gameObject.GetComponent<Outline>();
                lastOutlinedObject.OutlineWidth = outlineWidth;
                Scope.sprite = InteractScope;

                if (Input.GetMouseButtonDown(0))
                {
                    IInteractable[] interactables = lastOutlinedObject.GetComponents<IInteractable>();
                    
                    foreach (var interactable in interactables)
                    {
                        interactable.Interact();
                    }
                }
            }
            else if (lastOutlinedObject != null)
            {
                Scope.sprite = SimpleScope;
                lastOutlinedObject.OutlineWidth = 0;
                lastOutlinedObject = null;
            }
        }
    }
}

public interface IInteractable
{
    void Interact();
}