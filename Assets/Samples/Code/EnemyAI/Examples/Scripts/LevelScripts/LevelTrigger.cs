using UnityEngine;

// This class is created for the example scene. There is no support for this script.
public class LevelTrigger : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			gameObject.SendMessageUpwards("ToggleLevel");
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			gameObject.SendMessageUpwards("CheckLevel");
		}
	}
}
