using UnityEngine;
using UnityEngine.InputSystem;

public class VRBaseController : MonoBehaviour
{
    [SerializeField] private InputActionReference positionAction;
    [SerializeField] private InputActionReference rotationAction;

    private LineRenderer lineRenderer;
    
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }
    
    private void OnEnable()
    {
        positionAction.action.Enable();
        rotationAction.action.Enable();
    }

    private void OnDisable()
    {
        positionAction.action.Disable();
        rotationAction.action.Disable();
    }

    private void Update()
    {
        // Update position
        Vector3 position = positionAction.action.ReadValue<Vector3>();
        transform.localPosition = position;

        // Update rotation with correction
        Quaternion rotation = rotationAction.action.ReadValue<Quaternion>();
        
        // Apply correction for different coordinate systems
        transform.localRotation = rotation * Quaternion.Euler(65.0f, 0f, 0f);
        
        // Update LineRenderer
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + transform.forward * 100);
    }
}
