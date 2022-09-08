using System.Collections.Generic;
using Combat;
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
        
        // Processing
        private Comp_SMBEventCurrator _eventCurrator;
        
        // control variables
        private bool attack;
        private bool attacking = false;
        private bool _jabbing;
        
        // Animation State Names
        private string _animJab = "Base Layer.Punch1";

        private List<GameObject> _objectsHit = new List<GameObject>();
        
        int IHitResponder.Damage { get => _damage; }

        void Start()
        {
            _eventCurrator = _animator.GetComponent<Comp_SMBEventCurrator>();
            _eventCurrator.Event.AddListener(OnSMBEvent);

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

        private void MoveAttack()
        {
            if (!characterManager.AnimationLock)
            {
                if (attack)
                {
                    characterManager.SetAnimationLock = true;
                    _animator.CrossFadeInFixedTime(_animJab, 0.1f, 0, 0);
                }
            }
            if (_jabbing)
            {
                _hitboxLeftFist.CheckHit();
            }
        }
        private void OnSMBEvent(string eventName)
        {
            //Debug.Log("Listener detected event");
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
            //Debug.Log("CheckingHit");
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