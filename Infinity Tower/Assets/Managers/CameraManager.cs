using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public CinemachineConfiner2D confiner;

    private void Awake() => Instance = this;

    public void confinerChange(Collider2D polygon) => confiner.BoundingShape2D = polygon;

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
