using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

public class SetupProject : Editor
{
	[MenuItem("Enemy AI/Set Project Settings", false, 1)]
	static void Init()
	{
		Object tagManager = AssetDatabase.LoadAssetAtPath<Object>("ProjectSettings/TagManager.asset");

		Object newTagManager = AssetDatabase.LoadAssetAtPath<Object>("Assets/Samples/Code/EnemyAI/Setup/TagManager.preset");
		if (newTagManager == null)
		{
			newTagManager = AssetDatabase.LoadAssetAtPath<Object>("Assets/TPS Bundle/EnemyAI/Setup/TagManager.preset");
		}

		Preset tagManagerPreset = (Preset) newTagManager;
		tagManagerPreset.ApplyTo(tagManager);


		Debug.Log("Project Tags and Layers successfully applied.");
	}
}

[InitializeOnLoad]
public class Startup
{
	static Startup()
	{
		if (LayerMask.NameToLayer("Enemy") != 12)
			Debug.LogFormat("Select Enemy AI > Set Project Settings to load the custom Tags and Layers.\n");
	}
}
