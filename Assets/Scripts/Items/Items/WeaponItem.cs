using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System;

namespace MoreMountains.InventoryEngine
{	
	[CreateAssetMenu(fileName = "WeaponItem", menuName = "MoreMountains/InventoryEngine/WeaponItem", order = 2)]
	[Serializable]
	/// <summary>
	/// Demo class for a weapon item
	/// </summary>
	public class WeaponItem : InventoryItem
	{
		[Header("Weapon")]
		/// Change this sprite to a prefab // Trying to put this in inventory item. Fk knows if a good idea
		/// the sprite to use to show the weapon when equipped
		//public Sprite WeaponSprite;
		public GameObject WeaponPrefab;


		/// <summary>
		/// What happens when the object is used 
		/// </summary>
		public override bool Equip(string playerID)
		{
			base.Equip(playerID);
			//Debug.Log(TargetInventory(playerID) + "Weapon Equiping"); //.GetComponent<Comp_Equipment>());
			//					(replace IDC script with Combat Manager)										(Replace the sprite with a weapon prefab)
			//Below line accesses IDC script. It runs the function SetWeapon. It passes the inventory item and the sprite to display.
			//										Weapon prefab is in place. Everything is good to go, I just cant resolve my Equipment script from here
			//Below is demo code to set weapon into game onEquip
			
			//TargetInventory(playerID).TargetTransform.GetComponent<Comp_Equipment>().SetWeapon(WeaponPrefab,this);
			return true;
		}

		/// <summary>
		/// What happens when the object is used 
		/// </summary>
		public override bool UnEquip(string playerID)
		{
			base.UnEquip(playerID);
			//TargetInventory(playerID).TargetTransform.GetComponent<InventoryDemoCharacter>().SetWeapon(null,this);
			return true;
		}
		
	}
}