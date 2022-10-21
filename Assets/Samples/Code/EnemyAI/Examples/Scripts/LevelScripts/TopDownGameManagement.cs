using UnityEngine;

// This class is created for the example scene. There is no support for this script.
public class TopDownGameManagement : MonoBehaviour
{
	public Texture2D cursorTexture;

	void Awake()
    {
		Cursor.SetCursor(cursorTexture, Vector2.one * (cursorTexture.width * 0.5f), CursorMode.Auto);
	}
}
