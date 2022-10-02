using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 1f;
    
    void FixedUpdate()
    {
        transform.Rotate(0, rotateSpeed, 0 * Time.deltaTime);
    }
}
