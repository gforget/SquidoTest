using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BallSpawnerButton : MonoBehaviour
{
    [SerializeField] private GameObject balloonReference;
    [SerializeField] private Transform spawnerTransform;
    
    public void RespawnBalloon()
    {
        balloonReference.transform.position = spawnerTransform.position;
        Rigidbody rb = balloonReference.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
