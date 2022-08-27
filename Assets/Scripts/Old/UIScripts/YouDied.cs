using UnityEngine;
using UnityEngine.UI;

namespace BlortNet
{
    public class YouDied : MonoBehaviour
    {
        [SerializeField] private GameObject deathTextGameObject;
        public void displayDeathText()
        {
            deathTextGameObject.SetActive(true);
        }
    }
}
