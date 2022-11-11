using UnityEngine;

namespace Combat.Guns
{
    public class MuzzleFlashController : MonoBehaviour
    {
        private float time = 0f;
        [SerializeField] private float timeToLive = 0.15f;

        private void OnEnable()
        {
            time = 0f;
        }

        private void FixedUpdate()
        {
            time += Time.deltaTime;

            if (time >= timeToLive)
            {
                gameObject.SetActive(false);
            }
        }
    }
}