using UnityEngine;

// This class handles event propagation (ex.: shot noise), alerting nearby objects.
public class AlertManagement : MonoBehaviour
{
	[Tooltip("Radius of alert propagation.")]
	[Range(0, 50)] public float alertRadius;
	[Tooltip("Time to live. How many extra tiers the alert is forwarded.")]
	public int extraWaves;
	[Tooltip("The layer mask of objects to alert.")]
	public LayerMask alertMask = 1 << 12;

	private Vector3 current;       // The current alert position.
	private bool alert;            // Is there a new alert to propagate?

	void Start()
	{
		// Ping the current alert (if any) periodically. Default period: 1 second.
		InvokeRepeating("PingAlert", 1, 1);
	}

	// Alert nearby objects of an event.
	private void AlertNearby(Vector3 origin, Vector3 target, int wave = 0)
	{
		// Will this object keep or forward the alert? 
		// If TTL is higher than defined, do not forward alert.
		if (wave > this.extraWaves)
			return;

		// Grab nearby objects to trigger alert.
		Collider[] targetsInViewRadius = Physics.OverlapSphere(origin, alertRadius, alertMask);

		foreach (Collider obj in targetsInViewRadius)
		{
			// Call the object callback to receive the alert.
			obj.SendMessageUpwards("AlertCallback", target, SendMessageOptions.DontRequireReceiver);

			// Forward alert to nearby objects
			AlertNearby(obj.transform.position, target, wave + 1);
		}
	}

	// Root alert callback, set current alert origin (called externally).
	public void RootAlertNearby(Vector3 origin)
	{
		current = origin;
		alert = true;
	}

	// Ping the alert, if any, periodically.
	void PingAlert()
	{
		if (alert)
		{
			alert = false;
			AlertNearby(current, current);
		}
	}
}
