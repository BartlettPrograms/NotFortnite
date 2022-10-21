using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class is created for the example scene. There is no support for this script.
public class HurtHUD : MonoBehaviour
{
	private Transform canvas;
	private GameObject hurtPrefab;
	private float decayFactor = 0.8f;

	private Dictionary<int, HurtData> hurtUIdata;

	private Transform player, cam;

	struct HurtData
	{
		public Transform shotOrigin;
		public Image hurtImg;
	}


	public void Setup(Transform canvas, GameObject hurtPrefab, float decayFactor, Transform player)
	{
		hurtUIdata = new Dictionary<int, HurtData>();
		this.canvas = canvas;
		this.hurtPrefab = hurtPrefab;
		this.decayFactor = decayFactor;
		this.player = player;
		cam = Camera.main.transform;
	}

	void Update()
    {
		List<int> toRemoveKeys = new List<int>();
		foreach (int key in hurtUIdata.Keys)
		{
			SetRotation(hurtUIdata[key].hurtImg, cam.forward, hurtUIdata[key].shotOrigin.position - player.position);
			hurtUIdata[key].hurtImg.color = GetUpdatedAlpha(hurtUIdata[key].hurtImg.color);
			if(hurtUIdata[key].hurtImg.color.a <= 0)
			{
				toRemoveKeys.Add(key);
			}
		}
		for (int i = 0; i < toRemoveKeys.Count; i++)
		{
			DestroyHurtUI(toRemoveKeys[i]);
		}
    }

	public void DrawHurtUI(Transform shotOrigin, int hashId)
	{
		if (hurtUIdata.ContainsKey(hashId))
		{
			hurtUIdata[hashId].hurtImg.color = GetUpdatedAlpha(hurtUIdata[hashId].hurtImg.color, true);
		}
		else
		{
			GameObject hurtUI = Object.Instantiate<GameObject>(hurtPrefab, canvas);
			SetRotation(hurtUI.GetComponent<Image>(), cam.forward, shotOrigin.position - player.position);
			HurtData data;
			data.shotOrigin = shotOrigin;
			data.hurtImg = hurtUI.GetComponent<Image>();
			hurtUIdata.Add(hashId, data);
		}
	}

	private Color GetUpdatedAlpha(Color currentColor, bool reset = false)
	{
		if(reset)
		{
			currentColor.a = 1f;
		}
		else
		{
			currentColor.a -= decayFactor * Time.deltaTime;
		}

		return currentColor;
	}

	private void DestroyHurtUI(int key)
	{
		Destroy(hurtUIdata[key].hurtImg.transform.gameObject);
		hurtUIdata.Remove(key);
	}

	private void SetRotation(Image hurtUI, Vector3 orientation, Vector3 shotDirection)
	{
		orientation.y = 0;
		shotDirection.y = 0;
		float rotation = Vector3.SignedAngle(shotDirection, orientation, Vector3.up);

		Vector3 newRotation = hurtUI.rectTransform.rotation.eulerAngles;
		newRotation.z = rotation;
		Image hurtImg = hurtUI.GetComponent<Image>();
		hurtImg.rectTransform.rotation = Quaternion.Euler(newRotation);
	}
}
