using MoreMountains.InventoryEngine;
using MoreMountains.Tools;
using PhysicsBasedCharacterController;
using PlayerCombatController;
using UI;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private CharacterManager _characterController;
    [SerializeField] private Comp_Equipment _equipment;
    [SerializeField] private CombatManager _combatManager;
    [SerializeField] private MMTouchRepositionableJoystick _leftJoyStick;

    [SerializeField] private InventoryInputManager _inventoryInputManager;
    [SerializeField] private UIManager _uiManager;

    public void HandleRTButton()
    {
        if (_equipment.SelectedEquipment == 0)
        {
            _equipment.SelectedEquipment = 1;
            _characterController.SetStrafing(true);
        }
        else // There is only two options. Otherwise should else if (selectedequipment == 1)
        {
            _characterController.ToggleStrafing();
        }
    }

    public void HandleXButton()
    {
        _characterController.SetStrafing(false);
        _combatManager.SetAttackTrue();
    }

    public void HandleInventoryButton()
    {
        // Do: close inventory - turn character controls on
        if (_inventoryInputManager.InventoryIsOpen)
        {
            _uiManager.ToggleInventory(false);
            _uiManager.ToggleTouchUI(true);
            _leftJoyStick.enabled = true;
        }
        else // Do: open inventory - turn character controls off
        {
            _uiManager.ToggleInventory(true);
            _uiManager.ToggleTouchUI(false);
            //_characterController.enabled = false;
            _leftJoyStick.enabled = false;
            _leftJoyStick.NormalizedValue = Vector2.zero;
            _leftJoyStick.RawValue = Vector2.zero;
        }
            
    }
}
