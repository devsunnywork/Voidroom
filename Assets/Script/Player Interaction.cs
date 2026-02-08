using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionDistance = 3f;
    public LayerMask interactableLayer;

    [Header("References")]
    public Transform playerCamera;
    public Image crosshair;
    public Transform holdPoint;
    public PlayerMovement playerMovement; // PlayerMovement script ka reference

    [Header("Inspection Settings")]
    public float rotationSpeed = 100f;
    public KeyCode inspectKey = KeyCode.R;

    private GameObject heldObject;
    private bool isInspecting = false;

    private void Update()
    {
        // 1. Agar pehle se kuch pakda hai
        if (heldObject != null)
        {
            // Drop karne ka check
            if (Input.GetKeyDown(KeyCode.E) && !isInspecting)
            {
                DropObject();
            }

            // Inspect toggle (R press karne par)
            if (Input.GetKeyDown(inspectKey))
            {
                ToggleInspection();
            }

            // Agar inspect kar rahe hain, toh rotation handle karo
            if (isInspecting)
            {
                HandleInspectionRotation();
            }
            
            return;
        }

        CheckInteraction();
    }

    private void CheckInteraction()
    {
        if (playerCamera == null || crosshair == null || holdPoint == null) return;

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            crosshair.color = Color.red;

            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUpObject(hit.collider.gameObject);
            }
        }
        else
        {
            crosshair.color = Color.white;
        }
    }

    void PickUpObject(GameObject obj)
    {
        heldObject = obj;
        heldObject.transform.SetParent(holdPoint);
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
    }

    void DropObject()
    {
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        heldObject.transform.SetParent(null);
        heldObject = null;
    }

    void ToggleInspection()
    {
        isInspecting = !isInspecting;

        if (isInspecting)
        {
            // Player movement aur look freeze kar do
            playerMovement.canMove = false;
            crosshair.enabled = false; // Inspect karte waqt dot ki zaroorat nahi
        }
        else
        {
            // Player movement wapas chalu
            playerMovement.canMove = true;
            crosshair.enabled = true;
            
            // Object ki rotation wapas seedhi kar do (Optional)
            heldObject.transform.localRotation = Quaternion.identity;
        }
    }

    void HandleInspectionRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        // Object ko rotate karo
        heldObject.transform.Rotate(playerCamera.up, -mouseX, Space.World);
        heldObject.transform.Rotate(playerCamera.right, mouseY, Space.World);
    }
}
