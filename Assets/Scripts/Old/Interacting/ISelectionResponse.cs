using UnityEngine;

namespace BlortNet
{
    internal interface ISelectionResponse
    {
        void OnSelect(Transform selection);
        void OnDeselect(Transform selection);
    }
}