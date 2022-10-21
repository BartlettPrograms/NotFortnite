using UnityEngine;
using UnityEditor;

namespace EnemyAI
{
	// This script is an editor extension to vizualize the NPC sense areas (view ,perception and near) on the scene viewer.
	[CustomEditor(typeof(StateController))]
	public class FieldOfViewEditor : Editor
	{
		void OnSceneGUI()
		{
			StateController fov = (StateController)target;
			if (fov)
			{
				Handles.color = Color.white;
				// Draw perception area (circle)
				Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.perceptionRadius);
				// Draw near perception area (half of perception radius)
				Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.nearRadius);
				// Define FOV arc boundaries
				Vector3 viewAngleA = DirFromAngle(fov.transform, -fov.viewAngle / 2, false);
				Vector3 viewAngleB = DirFromAngle(fov.transform, fov.viewAngle / 2, false);
				// Draw FOV area (arc)
				Handles.DrawWireArc(fov.transform.position, Vector3.up, viewAngleA, fov.viewAngle, fov.viewRadius);
				Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius);
				Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius);
				// Draw line from NPC to target, if target in FOV
				Handles.color = Color.yellow;
				if (fov.targetInSight && fov.personalTarget != Vector3.zero)
					Handles.DrawLine(fov.enemyAnimation.gunMuzzle.position, fov.personalTarget);
			}
		}
		// Get rotated direction vector, relative to global or NPC forward direction.
		Vector3 DirFromAngle(Transform transform, float angleInDegrees, bool angleIsGlobal)
		{
			if (!angleIsGlobal)
			{
				angleInDegrees += transform.eulerAngles.y;
			}
			return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
		}

	}
}
