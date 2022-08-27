using UnityEngine;

public class MuzzleFlashController : MonoBehaviour
{
    private void Start()
    {
        Invoke("destroy", 0.5f);
    }

    private void destroy()
    {
        Destroy(this.gameObject);
    }
}