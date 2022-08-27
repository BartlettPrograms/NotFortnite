/*using UnityEngine;

namespace BlortNet
{
    public class OutlineSelectionResponse : MonoBehaviour, ISelectionResponse
    {
        public void OnSelect(Transform selection)
        {
            var outline = selection.GetComponent<Outline>();
            if (outline != null) outline.OutlineWidth = 10;
        }

        public void OnDeselect(Transform selection)
        {
            var outline = selection.GetComponent<Outline>();
            if (outline != null) outline.OutlineWidth = 0;
        }
    }
}
*/