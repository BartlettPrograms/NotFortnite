using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 1f;
    [SerializeField] private GameObject rotateAround;
    
    void FixedUpdate()
    {
        float edit = transform.rotation.y;
        //transform.rotation = transform.rotation.y + rotateSpeed * Time.deltaTime;
    }
}
