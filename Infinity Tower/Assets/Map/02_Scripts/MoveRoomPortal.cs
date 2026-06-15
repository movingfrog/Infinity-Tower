using UnityEngine;

public enum PortalType
{
    In,
    Out,
}

public class MoveRoomPortal : Portal
{
    [Header("포탈 속성")]
    [field: SerializeField]
    public PortalType PortalType { get; private set; }
    public virtual bool IsConnected => arrivePos != null;

    [field: SerializeField]
    public bool LastPortal { get; private set; }

    public void ConnectTo(Portal otherPortal)
    {
        arrivePos = otherPortal;
    }
}
