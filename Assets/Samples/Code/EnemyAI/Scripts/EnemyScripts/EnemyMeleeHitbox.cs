using System.Collections.Generic;
using Combat;
using EnemyAI;
using PlayerLockonCamTesting;
using UnityEditor;
using UnityEngine;

public class EnemyMeleeHitbox : MonoBehaviour, ITargetable, IHurtResponder
{
    [SerializeField] private bool m_targetable = true;
    [SerializeField] private Transform m_targetTransform;
    [SerializeField] private Rigidbody m_rbEnemy;

    private EnemyHealth _enemyHealth;
    private GameObject _player;
    private CapsuleCollider _enemyCollider;

    private List<Comp_Hurtbox> m_hurtboxes = new List<Comp_Hurtbox>();

    bool ITargetable.Targetable { get => m_targetable; }
    Transform ITargetable.TargetTransform { get => m_targetTransform; }
    private void Start()
    {
        m_hurtboxes = new List<Comp_Hurtbox>(GetComponentsInChildren<Comp_Hurtbox>());
        foreach (Comp_Hurtbox _hurtbox in m_hurtboxes)
            _hurtbox.HurtResponder = this;
        _player = GameObject.FindWithTag("Player");
        _enemyCollider = this.gameObject.GetComponent<CapsuleCollider>();
    }

    public bool CheckHit(HitData hitData)
    {
        return true;
    }

    public void Response(HitData data)
    {
        Debug.Log("HitResponse: " + data.damage);
        Vector3 _force = -data.hitNormal * data.damage;
        Vector3 _point = data.hitPoint;
        m_rbEnemy.AddForceAtPosition(_force, _point, ForceMode.Impulse);
        _enemyHealth = m_rbEnemy.transform.GetComponent<EnemyHealth>();
        _enemyHealth.TakeDamage(data.hitPoint, data.hitNormal, data.damage, _enemyCollider, _player);
    }
}
