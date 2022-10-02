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
        [SerializeField] private Comp_Hitbox _hitboxLeftFist;
        [SerializeField] private GameObject _hitSparkPrefab;

        [SerializeField] private Comp_Equipment Equipment;
        
        // Processing
        private Comp_SMBEventCurrator _eventCurrator;
        
        // control variables
        private bool attack;
        private bool attacking = false;
        private bool _jabbing;
        
        // Animation State Names
        private string _animJab;
        [SerializeField] 
        private string[] animations;

        private List<GameObject> _objectsHit = new List<GameObject>();
        
        int IHitResponder.Damage { get => _damage; }

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
            attack = input.attack;
        }

        private void FixedUpdate()
        {
            MoveAttack();
        }

        public void MoveAttack()
        {
            if (!characterManager.AnimationLock)
            {
                if (attack)
                {
                    characterManager.SetAnimationLock = true;
                    _animator.CrossFadeInFixedTime(animations[Equipment.WeaponTypeInt], 0.1f, 0, 0);
                    //Debug.Log(Equipment.WeaponTypeInt);
                }
            }
            if (_jabbing)
            {
                _hitboxLeftFist.CheckHit();
            }
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
                case "JabStart":
                    _objectsHit.Clear();
                    _jabbing = true;
                    break;
                
                case "JabEnd":
                    _jabbing = false;
                    break;
                
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