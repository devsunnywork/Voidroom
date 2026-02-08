using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [Header("Settings")]
    public KeyCode toggleKey = KeyCode.F;
    public bool isOn = false;

    [Header("Smart Light Settings")]
    public float maxDistance = 1f;       // Kitni paas aane par light piche jayegi
    public float moveBackAmount = 0.5f;  // Kitna piche jayegi
    public float smoothSpeed = 10f;      // Kitni tezi se move karegi

    [Header("References")]
    public GameObject lightSource;
    public Transform playerCamera;       // Camera ka reference zaroori hai

    private Vector3 originalPosition;    // Light ki asli jagah
    private float targetZ;               // Hum kahan jana chahte hain

    private void Start()
    {
        if (lightSource != null)
        {
            lightSource.SetActive(isOn);
            originalPosition = lightSource.transform.localPosition; // Shuruwat ki jagah yaad kar lo
        }

        if (playerCamera == null && Camera.main != null)
        {
            playerCamera = Camera.main.transform;
        }
    }

    private void Update()
    {
        // Toggle Input
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleFlashlight();
        }

        // Smart Position Logic
        HandleLightPosition();
    }

    void HandleLightPosition()
    {
        if (lightSource == null || playerCamera == null) return;

        // 1. Check karo samne deewar kitni door hai
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Default: Light apni asli jagah (0) par honi chahiye
        targetZ = originalPosition.z;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            // 2. Agar deewar paas hai (< 1m), toh light ko piche khiskao
            // Formula: Jitna paas, utna piche
            float distanceRatio = hit.distance / maxDistance; // 0 (chipak gaye) se 1 (door)
            targetZ = originalPosition.z - (moveBackAmount * (1 - distanceRatio));
        }

        // 3. Smoothly light ko nayi jagah par le jao
        Vector3 newPos = lightSource.transform.localPosition;
        newPos.z = Mathf.Lerp(newPos.z, targetZ, Time.deltaTime * smoothSpeed);
        lightSource.transform.localPosition = newPos;
    }

    void ToggleFlashlight()
    {
        isOn = !isOn;
        if (lightSource != null)
        {
            lightSource.SetActive(isOn);
        }
    }
}
