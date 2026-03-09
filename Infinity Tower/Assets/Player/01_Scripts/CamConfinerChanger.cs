using UnityEngine;

public class CamConfinerChanger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Confiner"))
        {
            CameraManager.Instance.confinerChange(collision);
        }
    }
}
