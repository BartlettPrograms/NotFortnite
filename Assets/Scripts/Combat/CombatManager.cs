using System.Collections.Generic;
using Combat;
using MoreMountains.InventoryEngine;
using PhysicsBasedCharacterController;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerCombatController
{
    public class CombatManager : MonoBehaviour, IHitResponder
    {
        //Required components
        [SerializeField]
        Animator _animator;
        [SerializeField]
        CharacterManager characterManager;
        public InputReader input;

        [Header("Attacking")]
        [SerializeField] private int _damage = 10;
        [SerializeField] private GameObject _hitSparkPrefab;
        [SerializeField] private Comp_Equipment Equipment;
        
        [Header("Hitbox")]
        [SerializeField] private Comp_Hitbox _hitboxLeftFist;
        [SerializeField] private GameObject _weaponHolder;
        private Comp_Hitbox _hitboxMeleeWeapon;
        
        // Processing
        private Comp_SMBEventCurrator _eventCurrator;
        
        // control variables
        private bool attack;
        private bool attacking = false;
        private bool _jabbing;
        private bool _swingingSword;

        // Animation State Names
        private string _animJab;
        private string[] animations;

        private List<GameObject> _objectsHit = new List<GameObject>();
        
        int IHitResponder.Damage { get => _damage; }

        public bool IsAttacking { get => attacking; } // animator uses this
        //public bool AttackInput { get => attack; }

        public void GetMeleeWepHitbox()
        {
            _hitboxMeleeWeapon = _weaponHolder.GetComponentInChildren<Comp_Hitbox>();
            _hitboxMeleeWeapon.HitResponder = this;
        }

        void Start()
        {
            _eventCurrator = _animator.GetComponent<Comp_SMBEventCurrator>();
            _eventCurrator.Event.AddListener(OnSMBEvent);
            animations = new string[] {"Base Layer.Punch1", "Base Layer.Swing1", "Base Layer.PistolShot1", "Base Layer.RifleShot1", "Base Layer.Fireball1"};

            //Physics.IgnoreLayerCollision(5, 5);
            _hitboxLeftFist.HitResponder = this;
        }

        void Update()
        {
            //attack = input.attack;'
            
        }

        private void FixedUpdate()
        {
            MoveAttack();
        }

        public bool AttackInput()
        {
            if (attack)
            {
                characterManager.SetAnimationLock = true;
                attack = false;
                return true;
            }
            else
                return false;
        }

        public void MoveAttack()
        {
            // Hitbox handling. simple functions just turn the right hitbox on for correct animation when bool = true
            //Jab check hits
            if (_jabbing)
            {
                _hitboxLeftFist.CheckHit();
            }
            // Melee Weapon check hits
            if (_swingingSword)
            {
                // If sword exists, check hit as normal
                if (_hitboxMeleeWeapon)
                {
                    _hitboxMeleeWeapon.CheckHit();
                }
                else // Else first time, find sword and enable the hit responder. Then proceed as normal
                {
                    GetMeleeWepHitbox();
                    _hitboxMeleeWeapon.CheckHit();
                }
            }
        }

        public void UnlockAnimation()
        {
            characterManager.SetAnimationLock = false;
        }
        
        public void SetAttackTrue()
        {
            attack = true;
        }
        public void SetAttackFalse()
        {
            attack = false;
        }
        
        private void OnSMBEvent(string eventName)
        {
            switch (eventName)
            {
                // This recieves event names from the animator to time turning hitboxes on and off
                // Uses components attatched to animator
                // Punching Events
                case "JabStart":
                    _objectsHit.Clear();
                    _jabbing = true;
                    break;
                case "JabEnd":
                    _jabbing = false;
                    break;
                
                // Sword Events
                case "SwingSwordStart":
                    _objectsHit.Clear();
                    _swingingSword = true;
                    break;
                case  "SwingSwordEnd":
                    _swingingSword = false;
                    break;
                
                // Applies to all attack events
                case "AnimationEnd":
                    characterManager.SetAnimationLock = false;
                    break;
            }
        }

        public int Damage { get; }
        
        public bool CheckHit(HitData data)
        {
            if (data.hurtbox.Owner == gameObject)   { return false; }
            else if (_objectsHit.Contains(data.hurtbox.Owner))   { return false; }
            else   { return true; }
            
        }

        public void Response(HitData data)
        {
            _objectsHit.Add(data.hurtbox.Owner);
            Instantiate(_hitSparkPrefab, data.hitPoint, Quaternion.identity);
        }
    }
}