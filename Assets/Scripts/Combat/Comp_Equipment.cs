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
        [SerializeField] private Inventory[] equippedWeapons; // Listen to what is in here...
        private InventoryItem metaItem;
        public string PlayerID = "Player1";

        // Currently held weapon
        [SerializeField] private int selectedEquipment = 0;
        public int SelectedEquipment
        { get => selectedEquipment; set { selectedEquipment = value; } }
        
        // Equipped Weapon Variables
        //private bool weaponEquipped;
        private string equippedWeaponID;
        private string inventoryWeaponID;
        private int equippedWeaponTypeInt = 0;
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

        private void FixedUpdate()
        {
	        if (equippedWeapons[selectedEquipment].Content[0] != null)
	        {
		        readEquippedWeapon();
		        PlayerWeaponBehaviour();
	        } else
	        {
		        inventoryWeaponID = null;
		        weaponType = null;
	        }
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

	        if (weaponTypeInt != equippedWeaponTypeInt)
	        {
		        equippedWeaponTypeInt = weaponTypeInt;
		        animator.SetInteger("WeaponType", weaponTypeInt);
		        if (weaponType == null)
		        {
			        animator.CrossFadeInFixedTime("Base Unarmed", 0.25f);
		        }
		        else
			        animator.CrossFadeInFixedTime("Base " + weaponType, 0.25f);
	        }
        }

        private void readEquippedWeapon()
        {
	        // equippedWeapons[selectedEquipment]
	        // weapons equipped[current weapon that should be held in hand]
	        
	        // Read Equipped Weapon details
	        inventoryWeaponID = equippedWeapons[selectedEquipment].Content[0].ItemID;
	        weaponType = equippedWeapons[selectedEquipment].Content[0].ShortDescription;

	        // If new weapon equip detected
            if (equippedWeaponID != inventoryWeaponID)
            {
                // Set the comparison variables to be the same, as to not trigger until next weapon change
                equippedWeaponID = inventoryWeaponID;
                // Get metaData about weapon
                metaItem = equippedWeapons[selectedEquipment].Content[0];
                
                // Code to hold a new weapon
                swapWeapons(equippedWeapons[selectedEquipment]);
            }
        }
        
        private void destroyHeldWeapon()
        {
	        for (var i = WeaponHolder.transform.childCount - 1; i >= 0; i--)
	        {
		        Destroy(WeaponHolder.transform.GetChild(i).gameObject);
	        }
        }

        private void swapWeapons(Inventory inv)
        {
	        // Code to hold a new weapon
	        Debug.Log("DestroyingWeps");
	        destroyHeldWeapon();
	        if (equippedWeaponID != null)
		        Debug.Log($"Spawning: {inv.Content[0]}");
		        SetWeapon(inv.Content[0]);
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
            GunSystemv2 gunScript = weapon.GetComponent<GunSystemv2>();
            if (gunScript)
            {
	            gunScript.enabled = true;
            }
            // If colliders exist, they should ALL be disabled below. or maybe we could just disable the item picker
        }

        public void PullOutGun()
        {
	        selectedEquipment = 1;
        }
        
        
        /// <summary>
        	/// Catches MMInventoryEvents and if it's an "inventory loaded" one, toggles the cameras ability to rotation 
        	/// </summary>
        	/// <param name="inventoryEvent">Inventory event.</param>
        	public virtual void OnMMEvent(MMInventoryEvent inventoryEvent)
        	{
                if (inventoryEvent.InventoryEventType == MMInventoryEventType.InventoryOpens)
                {
	                //Cursor.lockState = CursorLockMode.None;
	                //cmInput.enabled = false;
                }
                if (inventoryEvent.InventoryEventType == MMInventoryEventType.InventoryCloses)
                {
	                //Cursor.lockState = CursorLockMode.Locked;
	                //cmInput.enabled = true;
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

