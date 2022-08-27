using System;
using UnityEngine;

namespace BlortNet
{
    public class SelectionsManager : MonoBehaviour
    {
        [SerializeField] private string selectableTag = "Selectable";

        private ISelectionResponse _selectionResponse;

        private Transform _selection;

        private void Awake()
        {
            _selectionResponse = GetComponent<ISelectionResponse>();
        }

        void Update()
        {
            // Deselection/Selection response
            if (_selection != null)
            {
                _selectionResponse.OnDeselect(_selection);
            }

            #region MyRegion

            // Create a Ray
            var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            
            // Selection Determination
            _selection = null;
            if (Physics.Raycast(ray, out var hit))
            {
                var selection = hit.transform;
                if (selection.CompareTag(selectableTag))
                {
                    _selection = selection;
                }
            }

            #endregion

            // Deselection/Selection response
            if (_selection != null)
            {
                _selectionResponse.OnSelect(_selection);
            }
        }
    }
}
