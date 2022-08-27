using UnityEngine;

public class BulletHoleController : MonoBehaviour
{
    private void Start()
    {
        Invoke("destroy", 20);
    }

    private void destroy()
    {
        Destroy(this.gameObject);
    }
}
