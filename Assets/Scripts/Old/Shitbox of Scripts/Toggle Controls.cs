using UnityEngine;

namespace BlortNet
{
    public class ToggleControls : MonoBehaviour
    {
        [SerializeField] private GameObject controlGuideGUI;

        public void ToggleGUI()
        {
            if (controlGuideGUI.activeInHierarchy)
            {
                controlGuideGUI.SetActive(false);
            } else controlGuideGUI.SetActive(true);
        }
    }
}
