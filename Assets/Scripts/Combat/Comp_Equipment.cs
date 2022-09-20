using MoreMountains.InventoryEngine;
using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.InventoryEngine
{
    public class Comp_Equipment : MonoBehaviour, MMEventListener<MMInventoryEvent>
    {
        // Right hand holder for weapons
        [SerializeField] private GameObject WeaponHolder;
        
        public Inventory ArmorInventory;
        public Inventory WeaponInventory;
        
        public string PlayerID = "Player1";

        // Equipped Weapon Variables
        private string weaponID;
        //private int damage;
        private GameObject weaponPrefab;
        private InventoryItem item;
        
        
        
        public void SetWeapon(GameObject newWeapon, InventoryItem metaItem)
        {
            Instantiate(newWeapon, Vector3.zero, Quaternion.identity, WeaponHolder.transform); // instantiate newWeapon;
        }

        
        
        
        /// listening to damn MMEvents. I am new to events so this was a little frustrating to import.
        
        /// <summary>
        /// Catches MMInventoryEvents and if it's an "inventory loaded" one, equips the first armor and weapon stored in the corresponding inventories
        /// </summary>
        /// <param name="inventoryEvent">Inventory event.</param>
        public virtual void OnMMEvent(MMInventoryEvent inventoryEvent)
        {
            Debug.Log(inventoryEvent.TargetInventoryName + " - " + inventoryEvent.InventoryEventType + " - " + inventoryEvent.EventItem);
            /*
            if (inventoryEvent.TargetInventoryName == "MainPlayerInventory")
                if (inventoryEvent.EventItem.ItemName != null)
                    if (weaponID != inventoryEvent.EventItem.ItemID)
                    {
                        item = inventoryEvent.EventItem;
                        weaponID = item.ItemName;
                        //weaponPrefab = item.Prefab;
                        //SetWeapon(weaponPrefab, item);
                        Debug.Log("Succsess!!!");
                    }
            */
            if (inventoryEvent.InventoryEventType == MMInventoryEventType.InventoryLoaded)
            {
                if (inventoryEvent.TargetInventoryName == "PlayerArmorInventory")
                {
                    if (ArmorInventory != null)
                    {
                        if (!InventoryItem.IsNull(ArmorInventory.Content [0]))
                        {
                            ArmorInventory.Content [0].Equip (PlayerID);	
                        }
                    }
                }
                if (inventoryEvent.TargetInventoryName == "PlayerWeaponInventory")
                {
                    Debug.Log("bingo!");
                    if (WeaponInventory != null)
                    {
                        if (!InventoryItem.IsNull (WeaponInventory.Content [0]))
                        {
                            Debug.Log("Sweet sweet relief");
                            WeaponInventory.Content [0].Equip (PlayerID);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// On Enable, we start listening to MMInventoryEvents
        /// </summary>
        protected virtual void OnEnable()
        {
            this.MMEventStartListening<MMInventoryEvent>();
        }


        /// <summary>
        /// On Disable, we stop listening to MMInventoryEvents
        /// </summary>
        protected virtual void OnDisable()
        {
            this.MMEventStopListening<MMInventoryEvent>();
        }
    }
}