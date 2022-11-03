using MoreMountains.InventoryEngine;
using PhysicsBasedCharacterController;
using PlayerCombatController;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private CharacterManager _characterController;
    [SerializeField] private Comp_Equipment _equipment;
    [SerializeField] private CombatManager _combatManager;

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
}
