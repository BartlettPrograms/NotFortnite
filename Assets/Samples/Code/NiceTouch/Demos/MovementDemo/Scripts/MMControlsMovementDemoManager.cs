using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	[Serializable]
	public class MMControlsMovementDemoManagerItem
	{
		public string Title;
		public string Description;
		public GameObject Target;
	}
	
	public class MMControlsMovementDemoManager : MonoBehaviour
	{
		public List<MMControlsMovementDemoManagerItem> Items;

		[Header("Bindings")] 
		public Text Title;
		public Text Description;

		[MMReadOnly]
		public int _currentIndex = 0;
		
		protected virtual void Awake()
		{
			_currentIndex = 0;
			HandleIndexChange();
		}

		public virtual void Next()
		{
			_currentIndex++;
			HandleIndexChange();
		}

		public virtual void Previous()
		{
			_currentIndex--;
			HandleIndexChange();
		}

		protected virtual void HandleIndexChange()
		{
			if (_currentIndex >= Items.Count)
			{
				_currentIndex = 0;
			}
			if (_currentIndex < 0)
			{
				_currentIndex = Items.Count - 1;
			}

			foreach (MMControlsMovementDemoManagerItem item in Items)
			{
				item.Target.SetActive(false);
			}
			Items[_currentIndex].Target.SetActive(true);
			Title.text = Items[_currentIndex].Title.ToUpper();
			Description.text = Items[_currentIndex].Description;
		}
	}	
}

