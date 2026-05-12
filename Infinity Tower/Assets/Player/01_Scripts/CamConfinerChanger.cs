using Unity.Cinemachine;
using UnityEngine;

public class CamConfinerChanger : MonoBehaviour
{
    CinemachineConfiner2D confiner;

    private void Start()
    {
        confiner = GameManager.Instance.confiner;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Confiner"))
        {
            WorkerHub<CameraMoveWorker>.Instance.confinerChange(collision, confiner);
        }
    }
}
