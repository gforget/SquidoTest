using UnityEngine;
public class VRLeftController : VRBaseController
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5.0f;

    private void Update()
    {
        Vector2 TranslationValue = thumbstickAction.action.ReadValue<Vector2>();
        
        // Get the camera's forward and right vectors
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        
        // Remove the y component to ignore pitch
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();
        
        // Calculate movement direction based on camera orientation
        Vector3 movement = (cameraRight * TranslationValue.x + cameraForward * TranslationValue.y) * speed * Time.deltaTime;
        
        // Check for collisions before moving
        if (!Physics.Raycast(XRigParents.position, movement.normalized, movement.magnitude))
        {
            XRigParents.position += movement;
        }
        
        base.Update();
    }
}