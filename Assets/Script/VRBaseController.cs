using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class VRBaseController : MonoBehaviour
{
    [SerializeField] private bool debugMode;
    
    [Header("General Reference")]
    [SerializeField] protected Transform XRigParents;
    
    [Header("Input Actions References")]
    [SerializeField] private InputActionReference positionAction;
    [SerializeField] private InputActionReference rotationAction;
    [SerializeField] private InputActionReference grabAction;
    [SerializeField] protected InputActionReference thumbstickAction;
    
    [Header("Grab and throw Settings")]
    [SerializeField] private Color unselectColor = Color.white;
    [SerializeField] private Color hoverColor = Color.green;
    [SerializeField] private float grabRange = 100.0f;
    [SerializeField] private float throwForceMultiplier = 5.0f;
    [SerializeField] private float angularVelocityMultiplier = 1.0f;
    [SerializeField] private float lerpSpeed = 10f;
    
    private LineRenderer m_LineRenderer;
    private GameObject m_CurrentHoveredObject = null;
    private Grabbable m_CurrentGrabbedObject = null;
     
    private void Awake()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
        m_LineRenderer.positionCount = 2;
    }
    
    private void OnEnable()
    {
        positionAction.action.Enable();
        rotationAction.action.Enable();
        grabAction.action.Enable();
        thumbstickAction.action.Enable();
    }

    private void OnDisable()
    {
        positionAction.action.Disable();
        rotationAction.action.Disable();
        grabAction.action.Disable();
        thumbstickAction.action.Disable();
    }

    protected void Update()
    {
        // Update position
        Vector3 position = positionAction.action.ReadValue<Vector3>();
        transform.localPosition = position;

        // Update rotation with correction
        Quaternion rotation = rotationAction.action.ReadValue<Quaternion>();
        
        // Apply correction for different coordinate systems
        transform.localRotation = rotation * Quaternion.Euler(65.0f, 0f, 0f);

        if (debugMode)
        {
            // Update LineRenderer
            m_LineRenderer.SetPosition(0, transform.position);
            m_LineRenderer.SetPosition(1, transform.position + transform.forward * grabRange);
        }
        
        // raycast in front of the controller
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, grabRange))
        {
            Grabbable grabbable = hit.collider.GetComponent<Grabbable>();
            if (grabbable != null)
            {
                grabbable.GetComponent<Renderer>().material.color = hoverColor;
                m_CurrentHoveredObject = grabbable.gameObject;
                
                if (grabAction.action.ReadValue<float>() > 0.5f && m_CurrentGrabbedObject == null)
                {
                    m_CurrentGrabbedObject = grabbable;
                    grabbable.StartGrab(transform,lerpSpeed );
                }
            }
            else
            {
                UnhoverObject();
            }
            
            BallSpawnerButton ballSpawnerButton = hit.collider.GetComponent<BallSpawnerButton>();
            if (ballSpawnerButton != null)
            {
                ballSpawnerButton.GetComponent<Renderer>().material.color = hoverColor;
                m_CurrentHoveredObject = ballSpawnerButton.gameObject;
                
                if (grabAction.action.ReadValue<float>() > 0.5f)
                {
                    ballSpawnerButton.RespawnBalloon();
                }
            }
        }
        else
        {
            UnhoverObject();
        }

        // Release object if grab button is released
        if (grabAction.action.ReadValue<float>() <= 0.5f && m_CurrentGrabbedObject != null)
        {
            m_CurrentGrabbedObject.EndGrab(throwForceMultiplier, angularVelocityMultiplier);
            m_CurrentGrabbedObject = null;
        }
    }

    private void UnhoverObject()
    {
        if (m_CurrentHoveredObject != null && m_CurrentGrabbedObject == null)
        {
            m_CurrentHoveredObject.GetComponent<Renderer>().material.color = unselectColor;
            m_CurrentHoveredObject = null;
        }
    }
}
