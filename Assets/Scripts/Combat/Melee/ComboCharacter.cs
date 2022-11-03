using System.Collections;
using System.Collections.Generic;
using MoreMountains.InventoryEngine;
using PlayerCombatController;
using UnityEngine;

public class ComboCharacter : MonoBehaviour
{

    private StateMachine meleeStateMachine;
    private CombatManager playerCombat;
    private Comp_Equipment playerEquipment;

    [SerializeField] public Collider hit;

    // Start is called before the first frame update
    void Start()
    {
        meleeStateMachine = GetComponent<StateMachine>();
        playerCombat = GetComponent<CombatManager>();
        playerEquipment = GetComponent<Comp_Equipment>();
        meleeStateMachine.SetNextStateToMain();
        Debug.Log(typeof(IdleCombatState));
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(meleeStateMachine.CurrentState.GetType());
        if (playerCombat.AttackInput() && meleeStateMachine.CurrentState.GetType() == typeof(IdleCombatState))
        {
            // player input detected
            if (playerEquipment.SelectedEquipment == 1)
            {
                // on first tap, pull out sword. dont swing until sword is in hand and get attack input
                playerEquipment.SelectedEquipment = 0;
            } else meleeStateMachine.SetNextState(new GroundEntryState());// fire attack
                
        }
        
        
    }
}
