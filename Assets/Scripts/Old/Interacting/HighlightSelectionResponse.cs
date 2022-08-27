using UnityEngine;

namespace BlortNet
{
    internal class HighlightSelectionResponse : MonoBehaviour, ISelectionResponse
    {
        [SerializeField] public Material highlightMaterial;
        [SerializeField] public Material defaultMaterial;

        public void OnSelect(Transform selection)
        {
            var selectionRenderer = selection.GetComponent<Renderer>();
            if (selectionRenderer != null)
            {
                selectionRenderer.material = this.highlightMaterial;
            }
        }

        public void OnDeselect(Transform selection)
        {
            
            var selectionRenderer = selection.GetComponent<Renderer>();
            if (selectionRenderer != null)
            {
                selectionRenderer.material = this.defaultMaterial;
            }
        }
    }
}