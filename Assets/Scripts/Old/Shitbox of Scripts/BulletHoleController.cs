using UnityEngine;

public class BulletHoleController : MonoBehaviour
{
    private void Start()
    {
        Invoke("destroy", 0.3f);
    }

    private void destroy()
    {
        Destroy(this.gameObject);
    }
}
