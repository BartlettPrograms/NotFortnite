using System;
using UnityEngine;

namespace Combat
{
    public class Test_HitResponder : MonoBehaviour, IHitResponder
    {
        [SerializeField] private bool m_attack;
        [SerializeField] private int m_damage = 10;
        [SerializeField] private Comp_Hitbox _hitbox;
        
        int IHitResponder.Damage { get => m_damage; }

        private void Start()
        {
            _hitbox.HitResponder = this;
        }

        private void Update()
        {
            if (m_attack)
            {
                _hitbox.CheckHit();
            }
        }

        bool IHitResponder.CheckHit(HitData data)
        {
            return true;
        }

        void IHitResponder.Response(HitData data)
        {
            
        }
    }
}