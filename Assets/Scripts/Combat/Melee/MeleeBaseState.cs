using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using PlayerCombatController;
using UnityEngine;

public class MeleeBaseState : State
{
    public float duration;
    public CombatManager playerCombat;
    protected Animator animator;
    protected bool shouldCombo;
    protected int attackIndex;


    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        playerCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatManager>();
        animator = playerCombat.GetComponentInChildren<Animator>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        // if x button pressed {shouldCombo = true;}
        if (playerCombat.AttackInput())
        {
            shouldCombo = true;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private void setAnimationLocked()
    {
        
    }

    private void setAnimationUnlocked()
    {
        
    }
}
