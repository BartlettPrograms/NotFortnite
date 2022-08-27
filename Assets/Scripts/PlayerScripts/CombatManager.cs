using System;
using PhysicsBasedCharacterController;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerCombatController
{
    public class CombatManager : MonoBehaviour
    {
        //Required components
        [SerializeField]
        Animator animator;
        [SerializeField]
        CharacterManager characterManager;
        public InputReader input;
        
        // control variables
        private bool attack;
        
        // Tracking tools
        private bool attacking = false;
        [SerializeField] private float cooldownTime = 2f;
        private float nextFireTime = 0f;
        public static int noOfClicks = 0;
        private float lastClickedTime = 0f;
        private float maxComboDelay = 1f;
        
        void Start()
        {

        }
        
        void Update()
        {
            attack = input.attack;
        }

        private void FixedUpdate()
        {
            MoveAttack();

            // Enable movement script when attack finished
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                {
                    characterManager.enabled = true;
                }
            }
        }

        private void MoveAttack()
        {
            if (attack && !attacking)
            {
                attacking = true;
                //Debug.Log("Attack Success");
                animator.SetBool("IsAttacking", true);
            } else if (!attack && attacking)
            {
                attacking = false;
                //Debug.Log("Attack Deactivated");
                animator.SetBool("IsAttacking", false);
            }
        }
    }
}