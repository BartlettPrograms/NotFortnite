using Cinemachine;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.InventoryEngine
{
    public class Comp_Equipment : MonoBehaviour, MMEventListener<MMInventoryEvent>
    {
        // Right hand holder for weapons
        [SerializeField] private GameObject WeaponHolder;

        // Inventories
        public Inventory ArmorInventory;
        public Inventory WeaponInventory; // Listen to what is in here...
        private InventoryItem metaItem;

        public string PlayerID = "Player1";

        // Equipped Weapon Variables
        //private bool weaponEquipped;
        private string equippedWeaponID;
        private string inventoryWeaponID;
        private string weaponType;
        private int weaponTypeInt;

        //private int damage;
        private GameObject weaponPrefab;
        private InventoryItem item;
        
        // Gameobjects
        [SerializeField] Animator animator;
        [SerializeField] private CinemachineInputProvider cmInput;

        private void Start()
        {
	        //Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
	        readEquippedWeapon();
	        PlayerWeaponBehaviour();
        }

        private void PlayerWeaponBehaviour()
        {
	        switch (weaponType)
	        {
		        case null:
			        weaponTypeInt = 0;
			        break;
		        case "Melee1H":
			        weaponTypeInt = 1;
			        break;
		        case "Gun1H":
			        weaponTypeInt = 2;
			        break;
		        case "Gun2H":
			        weaponTypeInt = 3;
			        break;
		        case "Staff":
			        weaponTypeInt = 4;
			        break;
	        }

	        animator.SetInteger("WeaponType", weaponTypeInt);
        }

        private void readEquippedWeapon()
        {
	        // Read Equipped Weapon details
	        if (WeaponInventory.Content[0] != null)
	        {
		        inventoryWeaponID = WeaponInventory.Content[0].ItemID;
		        weaponType = WeaponInventory.Content[0].ShortDescription;
	        }
	        // Read if no weapons Equipped
	        else
	        {
		        inventoryWeaponID = null;
		        weaponType = null;
	        }

	        // If new weapon equip detected
	        if (equippedWeaponID != inventoryWeaponID)
	        {
		        // Set the comparison variables to be the same, as to not trigger until next weapon change
		        equippedWeaponID = inventoryWeaponID;
		        // Get metaData about weapon
		        metaItem = WeaponInventory.Content[0];
		        
		        // Code to hold a new weapon
		        swapWeapons();
	        }
        }
        
        private void destroyHeldWeapon()
        {
	        for (var i = WeaponHolder.transform.childCount - 1; i >= 0; i--)
	        {
		        Destroy(WeaponHolder.transform.GetChild(i).gameObject);
	        }
        }

        private void swapWeapons()
        {
	        // Code to hold a new weapon
	        destroyHeldWeapon();
	        if (equippedWeaponID != null)
		        SetWeapon(WeaponInventory.Content[0]);
        }
        
        public void SetWeapon(InventoryItem metaItem)
        {
	        // Weapon needs to be instantiated Locally!
            GameObject weapon = Instantiate(metaItem.Prefab, Vector3.zero, Quaternion.identity, WeaponHolder.transform); // instantiate newWeapon;

            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
            
            // These GetCompenents arent good. We need to spawn a better prefab than the dropped item.
            BoxCollider[] colliders = weapon.GetComponents<BoxCollider>();
            foreach (BoxCollider collider in colliders)
            {
	            collider.enabled = false;
            }

            weapon.GetComponent<Rigidbody>().detectCollisions = false;
            // If colliders exist, they should ALL be disabled below. or maybe we could just disable the item picker
        }
        
        
        /// <summary>
        	/// Catches MMInventoryEvents and if it's an "inventory loaded" one, equips the first armor and weapon stored in the corresponding inventories
        	/// </summary>
        	/// <param name="inventoryEvent">Inventory event.</param>
        	public virtual void OnMMEvent(MMInventoryEvent inventoryEvent)
        	{
                if (inventoryEvent.InventoryEventType == MMInventoryEventType.InventoryOpens)
                {
	                //Cursor.lockState = CursorLockMode.None;
	                cmInput.enabled = false;
                }
                if (inventoryEvent.InventoryEventType == MMInventoryEventType.InventoryCloses)
                {
	                //Cursor.lockState = CursorLockMode.Locked;
	                cmInput.enabled = true;
                }
                
                
        		/*if (inventoryEvent.InventoryEventType == MMInventoryEventType.InventoryLoaded)
        		{
        			if (inventoryEvent.TargetInventoryName == "RogueArmorInventory")
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
                        Debug.Log("Player weapon event detected!!!! RAISE ALARM");
        				if (WeaponInventory != null)
        				{
        					if (!InventoryItem.IsNull (WeaponInventory.Content [0]))
        					{
                                Debug.Log("Imported code completed Job!");
        						WeaponInventory.Content [0].Equip (PlayerID);
        					}
        				}
        			}
        		}*/
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
    
            public int WeaponTypeInt { get => weaponTypeInt; }
    }
}

