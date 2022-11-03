using System;
using System.Collections.Generic;
using PlayerLockonCamTesting;
using UnityEngine;

namespace Combat
{
    public class Comp_PunchingBag : MonoBehaviour, ITargetable, IHurtResponder
    {
        [SerializeField] private bool m_targetable = true;
        [SerializeField] private Transform m_targetTransform;
        [SerializeField] private Rigidbody m_rbBag;

        private List<Comp_Hurtbox> m_hurtboxes = new List<Comp_Hurtbox>();
        
        bool ITargetable.Targetable { get => m_targetable; }
        Transform ITargetable.TargetTransform { get => m_targetTransform; }
        private void Start()
        {
            m_hurtboxes = new List<Comp_Hurtbox>(GetComponentsInChildren<Comp_Hurtbox>());
            foreach (Comp_Hurtbox _hurtbox in m_hurtboxes)
                 _hurtbox.HurtResponder = this;
        }

        public bool CheckHit(HitData hitData)
        {
            return true;
        }

        public void Response(HitData data)
        {
            Debug.Log("HitResponse");
            Vector3 _force = -data.hitNormal * data.damage;
            Vector3 _point = data.hitPoint;
            m_rbBag.AddForceAtPosition(_force, _point, ForceMode.Impulse);
        }
    }
}