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
                //Debug.Log("Opening Inventory");
                _inventoryInputManager.OpenInventory();
            }
            else
            {
                //Debug.Log("Closing Inventory");
                _inventoryInputManager.CloseInventory();
            }
        }
        
        public void ToggleTouchUI(bool option)
        {
            foreach (GameObject element in touchUIElements)
            {
                // Activate touch element
                element.SetActive(option);
            }
        }
    }
}