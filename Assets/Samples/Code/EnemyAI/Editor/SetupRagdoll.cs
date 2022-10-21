using System;
using UnityEditor;
using UnityEngine;


public class SetupRagdoll : Editor
{
	[MenuItem("GameObject/Enemy AI/Setup Ragdoll", false, 33)]
	static void Init()
	{
		Animator anim = Selection.activeGameObject.GetComponent<Animator>();
		Type type = Type.GetType("UnityEditor.RagdollBuilder, UnityEditor");
		UnityEngine.Object[] windowsOpened = Resources.FindObjectsOfTypeAll(type);

		if (windowsOpened == null || windowsOpened.Length == 0)
		{
			EditorApplication.ExecuteMenuItem("GameObject/3D Object/Ragdoll...");
			windowsOpened = Resources.FindObjectsOfTypeAll(type);
		}

		ScriptableWizard ragdollWizard = windowsOpened[0] as ScriptableWizard;

		SetBoneField(anim, ragdollWizard, "pelvis", HumanBodyBones.Hips);
		SetBoneField(anim, ragdollWizard, "leftHips", HumanBodyBones.LeftUpperLeg);
		SetBoneField(anim, ragdollWizard, "leftKnee", HumanBodyBones.LeftLowerLeg);
		SetBoneField(anim, ragdollWizard, "leftFoot", HumanBodyBones.LeftFoot);
		SetBoneField(anim, ragdollWizard, "rightHips", HumanBodyBones.RightUpperLeg);
		SetBoneField(anim, ragdollWizard, "rightKnee", HumanBodyBones.RightLowerLeg);
		SetBoneField(anim, ragdollWizard, "rightFoot", HumanBodyBones.RightFoot);
		SetBoneField(anim, ragdollWizard, "leftArm", HumanBodyBones.LeftUpperArm);
		SetBoneField(anim, ragdollWizard, "leftElbow", HumanBodyBones.LeftLowerArm);
		SetBoneField(anim, ragdollWizard, "rightArm", HumanBodyBones.RightUpperArm);
		SetBoneField(anim, ragdollWizard, "rightElbow", HumanBodyBones.RightLowerArm);
		SetBoneField(anim, ragdollWizard, "middleSpine", HumanBodyBones.Spine);
		SetBoneField(anim, ragdollWizard, "head", HumanBodyBones.Head);

		ragdollWizard.GetType().GetField("totalMass").SetValue(ragdollWizard, 80f);

		Debug.Log("Please edit one field of the Ragdoll wizard to refresh the window, and then click on Create");
	}

	private static void SetBoneField(Animator anim, ScriptableWizard window, String fieldName, HumanBodyBones boneType)
	{

		Transform bone = anim.GetBoneTransform(boneType);
		if (bone == null)
		{
			Debug.LogWarning("This character avatar doesn't have the " + boneType + " bone!");
		}
		else
			window.GetType().GetField(fieldName).SetValue(window, bone);
	}
}
