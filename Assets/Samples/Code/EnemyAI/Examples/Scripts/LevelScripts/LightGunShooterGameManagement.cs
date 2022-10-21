using UnityEngine;
using EnemyAI;
using UnityEngine.AI;

// This class is created for the example scene. There is no support for this script.
public class LightGunShooterGameManagement : MonoBehaviour
{
	public Texture2D cursorTexture;
	public int currentLevel = 1;

	private SimplePlayerHealth player;
	private Transform waypoints;
	private NavMeshAgent nav;
	private bool engaging;
	private float fullHealth;
	private int index;

	void Awake()
	{
		Cursor.SetCursor(cursorTexture, Vector2.one * (cursorTexture.width * 0.5f), CursorMode.Auto);
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<SimplePlayerHealth>();
		fullHealth = player.health;
		nav = GameObject.FindGameObjectWithTag("Player").GetComponent<NavMeshAgent>();
	}

	void Start()
	{
		waypoints = transform.Find("waypoints");
		nav.SetDestination(waypoints.GetChild(index).position);
		Cursor.visible = false;
	}

	void Update()
	{
		if(nav.remainingDistance <= nav.stoppingDistance &&
			index < waypoints.childCount &&
			!waypoints.GetChild(index).GetComponent<Collider>())
		{
			if(++index < waypoints.childCount)
				nav.SetDestination(waypoints.GetChild(index).position);
		}
	}

	public void ToggleLevel()
	{
		Cursor.SetCursor(cursorTexture, Vector2.one * (cursorTexture.width * 0.5f), CursorMode.Auto);
		Cursor.visible = true;
		EnableLevelEnemies();
		engaging = true;
	}

	void EnableLevelEnemies()
	{
		foreach(StateController enemy in GameObject.FindObjectsOfType<StateController>())
		{
			if (enemy.name.Contains("enemy (" + currentLevel))
				enemy.enabled = true;
		}
	}

	public void CheckLevel()
	{
		if (engaging)
		{
			foreach (StateController enemy in GameObject.FindObjectsOfType<StateController>())
			{
				if (enemy.enabled)
				{
					return;
				}
			}
			Destroy(waypoints.GetChild(index).GetComponent<Collider>());
			Cursor.visible = false;
			engaging = false;
			currentLevel++;
			player.health = fullHealth;

			if (currentLevel > 3)
				player.health = 0f;
		}
	}
}

