using UnityEngine;
using System.Collections;

// This class is created for the example scene. There is no support for this script.
public class CameraFollow : MonoBehaviour
{
	public Transform target;            // The position that that camera will be following.
	public float smoothing = 5f;        // The speed with which the camera will be following.

	public bool zMove;

	Vector3 offset;                     // The initial offset from the target.

	void Start()
	{
		zMove = true;
		// Calculate the initial offset.
		offset = transform.position - target.position;
	}

	void FixedUpdate()
	{
		// Create a postion the camera is aiming for based on the offset from the target.
		Vector3 targetCamPos = target.position + offset;

		if(!zMove)
		{
			targetCamPos.z = Mathf.Max(targetCamPos.z, transform.position.z);
		}

		// Smoothly interpolate between the camera's current position and it's target position.
		transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);

		transform.LookAt(target.position);
	}

	public void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Finish"))
		{
			zMove = false;
		}
	}

	public void OnTriggerExit(Collider other)
	{
		zMove = true;
	}
}
