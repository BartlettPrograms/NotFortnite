using System.Collections.Generic;
using MoreMountains.InventoryEngine;
using MoreMountains.Tools;
using UnityEngine;

namespace UI
{
    public class UIManager: MonoBehaviour
    {
        [SerializeField] private GameObject[] touchUIElements;

        [SerializeField] private InventoryInputManager _inventoryInputManager;


        public virtual void ToggleInventory(bool option)
        {
            if (option)
            {
                Debug.Log("Opening Inventory");
                _inventoryInputManager.OpenInventory();
            }
            else
            {
                Debug.Log("Closing Inventory");
                _inventoryInputManager.CloseInventory();
            }
        }
        
        public void ToggleTouchUI()
        {
            // Checks first element and assumes rest of them are the same
            if (touchUIElements[0].activeInHierarchy)
            {
                ToggleInventory(true);
                foreach (GameObject element in touchUIElements)
                {
                    // Deactivate touch element
                    element.SetActive(false);
                }
            }
            else
            {
                ToggleInventory(false);
                foreach (GameObject element in touchUIElements)
                {
                    // Activate touch element
                    element.SetActive(true);
                }
            }
        }
    }
}