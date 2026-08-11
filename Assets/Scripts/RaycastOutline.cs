using UnityEngine;

public class RaycastOutline : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float outlineWidth = 10f;
    [SerializeField] private float maxRayDistance = 3f;

    private Outline lastOutlinedObject;

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, maxRayDistance))
        {
            if (hit.transform.gameObject.CompareTag("Item"))
            {
                Outline currentOutline = hit.transform.gameObject.GetComponent<Outline>();

                if (currentOutline != lastOutlinedObject)
                {
                    if (lastOutlinedObject != null)
                    {
                        lastOutlinedObject.enabled = false;
                    }

                    currentOutline.OutlineMode = Outline.Mode.OutlineVisible;
                    currentOutline.OutlineWidth = outlineWidth;

                    currentOutline.enabled = true;

                    lastOutlinedObject = currentOutline;
                }
            }
        }
        else if (lastOutlinedObject != null)
        {
            lastOutlinedObject.enabled = false;
            lastOutlinedObject = null;
        }
    }
}