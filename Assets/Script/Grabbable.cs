using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    private bool m_IsGrabbed = false;
    private Transform m_Grabber = null;
    private Rigidbody m_Rb;
    
    // For calculating throw velocity
    private Vector3 m_PreviousPosition;
    private Vector3 m_Velocity;
    private Quaternion m_PreviousRotation; 
    private Vector3 m_AngularVelocity;

    private float m_LerpSpeed;
    
    private void Awake()
    {
        m_Rb = GetComponent<Rigidbody>();
    }

    public void StartGrab(Transform controller, float lerpSpeedValue)
    {
        m_IsGrabbed = true;
        m_Grabber = controller;
        m_LerpSpeed = lerpSpeedValue;
        
        //Cancel physics for the lerp
        m_Rb.useGravity = false;
        m_Rb.isKinematic = true;
    }

    public void EndGrab(float throwForceMultiplier, float angularVelocityMultiplier)
    {
        m_IsGrabbed = false;
        m_Grabber = null;
        
        //Reactivate physic
        m_Rb.useGravity = true;
        m_Rb.isKinematic = false;
        
        // Apply the throw force immediately before physics takes over
        m_Rb.AddForce(m_Velocity * throwForceMultiplier, ForceMode.Impulse);
        // Add angular velocity
        m_Rb.angularVelocity = m_AngularVelocity * angularVelocityMultiplier;
    }

    void Update()
    {
        if (m_IsGrabbed && m_Grabber != null)
        {
            //TODO: Figure out a way to prevent ball from being thrown while lerping toward the hand.
            transform.position = Vector3.Lerp(transform.position, m_Grabber.position, Time.deltaTime * m_LerpSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, m_Grabber.rotation, Time.deltaTime * m_LerpSpeed); 
            
            // Calculate the velocity
            m_Velocity = (transform.position - m_PreviousPosition) / Time.deltaTime; // divide the delta time, since it is the velocity we get not the distance
            
            //TODO: Add a texture to the balloon so it show the rotation when you throw it
            //--- Calculate angular velocity to rotate the object when it is thrown ---//
            
            // the difference between the current rotation and the previous rotation
            Quaternion deltaRotation = transform.rotation * Quaternion.Inverse(m_PreviousRotation);

            // angle = how many degrees the object has rotated, axis = the axis rotated around
            deltaRotation.ToAngleAxis(out float angle, out Vector3 axis); 
            
            // Convert to radians, since unity uses radians
            angle *= Mathf.Deg2Rad; 
            
            // Get the angular velocity
            m_AngularVelocity = (angle * axis) / Time.deltaTime;
            
            //---[END] Calculate angular velocity to rotate the object when it is thrown ---//
            
            // record previous position and rotation
            m_PreviousPosition = transform.position;
            m_PreviousRotation = transform.rotation;
        }
    }
}
