using UnityEngine;

namespace PlayerLockonCamTesting
{
    public interface ITargetable
    {
        public bool Targetable { get; }
        public Transform TargetTransform { get; }
    }
}