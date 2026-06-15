using System.Collections.Generic;
using UnityEngine;

public class PortalConnectSystem : MonoBehaviour
{
    [SerializeField]
    private MoveRoomPortal LastRoomPortal;

    // 분류용 리스트
    private List<MoveRoomPortal> inPortals = new List<MoveRoomPortal>();
    private List<MoveRoomPortal> outPortals = new List<MoveRoomPortal>();

    private void Awake()
    {
        // 1. 자식 오브젝트에서 포탈 컴포넌트 수집
        var portals = GetComponentsInChildren<MoveRoomPortal>(true);

        foreach (var portal in portals)
        {
            if (portal == null || portal.LastPortal)
                continue;

            if (portal.PortalType == PortalType.In)
                inPortals.Add(portal);
            else if (portal.PortalType == PortalType.Out)
                outPortals.Add(portal);
        }

        // 2. 완벽한 랜덤을 위해 두 리스트의 순서를 무작위로 섞습니다.
        ShuffleList(inPortals);
        ShuffleList(outPortals);

        ConnectPortal();
    }

    private void ConnectPortal()
    {
        // 이미 섞여있기 때문에, 맨 뒤에서부터 하나씩 꺼내기만 해도 완벽한 랜덤입니다.
        while (outPortals.Count > 0)
        {
            int lastOutIndex = outPortals.Count - 1;
            MoveRoomPortal currentOut = outPortals[lastOutIndex];
            outPortals.RemoveAt(lastOutIndex);

            MoveRoomPortal targetIn = null;
            int chosenInIndex = -1;

            // 이미 랜덤하게 섞인 In 포탈 리스트를 순회하며 조건에 맞는 대상을 찾습니다.
            for (int i = 0; i < inPortals.Count; i++)
            {
                // 조건: 같은 방(부모)이 아닌 경우에만 짝으로 인정
                if (currentOut.transform.parent != inPortals[i].transform.parent)
                {
                    targetIn = inPortals[i];
                    chosenInIndex = i;
                    break;
                }
            }

            if (inPortals.Count == 0)
            {
                currentOut.ConnectTo(LastRoomPortal);
                LastRoomPortal.ConnectTo(targetIn);
                continue;
            }

            // 조건에 맞는 포탈을 찾았다면 연결하고 In 리스트에서 안전하게 제거
            if (targetIn != null && chosenInIndex != -1)
            {
                currentOut.ConnectTo(targetIn);
                targetIn.ConnectTo(currentOut);

                inPortals.RemoveAt(chosenInIndex);
            }
            else
            {
                // 조건에 맞는 포탈이 없을 때의 예외 처리 (무한 루프 없이 안전하게 넘어감)
                Debug.LogWarning(
                    $"{currentOut.gameObject.name}: 같은 부모가 아닌 랜덤 In 포탈을 찾지 못했습니다."
                );
            }
        }
    }

    // 리스트의 순서를 무작위로 섞어주는 피셔-예이츠 셔플(Fisher-Yates Shuffle) 알고리즘
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[rnd];
            list[rnd] = temp;
        }
    }
}
