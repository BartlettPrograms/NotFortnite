using UnityEngine;
using UnityEngine.UI;

namespace EnemyAI
{
	// HealthBillboardManager aligns the health HUD above the object to always face the main camera.
	public class HealthBillboardManager : MonoBehaviour
	{
		[Tooltip("How many time health HUD is displayed after the object is shot?")]
		public float decayDuration = 2f;

		private Camera m_Camera;                                    // Main camera reference.
		private Image hud, bar;                                     // Health HUD and bar references.
		private float decayTimer;                                   // Current decay timer.
		private Color originalColor, noAlphaColor;                  // Health HUD color references.

		private void Start()
		{
			// Set up the references.
			hud = transform.Find("HUD").GetComponent<Image>();
			bar = transform.Find("Bar").GetComponent<Image>();
			m_Camera = Camera.main;
			originalColor = noAlphaColor = hud.color;
			noAlphaColor.a = 0f;

			// Hide health HUD on start.
			gameObject.SetActive(false);
		}

		//Orient bilboard after all camera movement is completed in this frame to avoid jittering
		void LateUpdate()
		{
			if (gameObject.activeSelf)
			{
				// Orientate HUD.
				transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
					m_Camera.transform.rotation * Vector3.up);
				// Update decay timer while HUD is visible.
				decayTimer += Time.deltaTime;
				if (decayTimer >= 0.5f * decayDuration)
				{
					float from = decayTimer - (0.5f * decayDuration);
					float to = 0.5f * decayDuration;
					// Lerp HUD colors to transparent.
					hud.color = Color.Lerp(originalColor, noAlphaColor, from / to);
					bar.color = Color.Lerp(originalColor, noAlphaColor, from / to);
				}
				// Disable HUD visibility.
				if (decayTimer >= decayDuration)
				{
					gameObject.SetActive(false);
				}
			}
		}

		// Set health HUD as visible (called externally).
		public void SetVisible()
		{
			gameObject.SetActive(true);
			decayTimer = 0;
			hud.color = bar.color = originalColor;
		}
	}
}