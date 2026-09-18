using Unity.Cinemachine;
using UnityEngine;

public class CamConfinerChanger : MonoBehaviour
{
    CinemachineConfiner2D confiner;

    private void Start()
    {
        confiner = GameManager.Instance.confiner;
    }

    public void RecheckConfinerImmediate()
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);
        Debug.Log($"[Confiner Recheck] 겹친 개수: {hits.Length}");
        foreach (var hit in hits)
        {
            Debug.Log($" - {hit.name} (tag: {hit.tag})");
            if (hit.gameObject.CompareTag("Confiner"))
            {
                Debug.Log($"[Confiner Recheck] 선택된 Confiner: {hit.name}");
                WorkerHub<CameraMoveWorker>.Instance.confinerChange(hit, confiner);
                break;
            }
        }
    }
}
