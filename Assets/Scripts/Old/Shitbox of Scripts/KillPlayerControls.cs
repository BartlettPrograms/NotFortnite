using PhysicsBasedCharacterController;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BlortNet
{
    public class KillPlayerControls : MonoBehaviour
    {
        [SerializeField] private PlayerInput sphereControlls;
        [SerializeField] private InputReader standingControls;

        public void KillControls()
        {
            sphereControlls.DeactivateInput();
            standingControls.gameObject.SetActive(false);
        }
    }
}
