using System.Collections.Generic;
using UnityEngine;

public class PortalConnectSystem : MonoBehaviour
{
    [Header("Final Destination")]
    [SerializeField]
    private GameObject finalExitRoom; // 무조건 마지막에 가야 하는 특정 방 (부모 오브젝트)

    private List<MoveRoomPortal> tempComponents = new List<MoveRoomPortal>();

    // 방(부모)별로 포탈을 분류하기 위한 딕셔너리
    private Dictionary<Transform, RoomPortalGroup> roomDataMap =
        new Dictionary<Transform, RoomPortalGroup>();
    private List<Transform> regularRoomList = new List<Transform>(); // 랜덤으로 섞을 일반 방 리스트

    // 방 하나가 가질 In/Out 포탈 구조체
    private class RoomPortalGroup
    {
        public MoveRoomPortal InPortal;
        public MoveRoomPortal OutPortal;
    }

    private void Awake()
    {
        // 1. 모든 포탈 수집
        GetComponentsInChildren(true, tempComponents);

        // 2. 부모(방)를 기준으로 포탈들을 그룹화
        foreach (var comp in tempComponents)
        {
            MoveRoomPortal portal = comp;
            if (portal == null)
                continue;

            Transform roomTransform = portal.transform.parent;

            if (!roomDataMap.ContainsKey(roomTransform))
            {
                roomDataMap[roomTransform] = new RoomPortalGroup();
                // 최종 목적지 방이 아니라면 랜덤 셔플 리스트에 추가
                if (roomTransform.gameObject != finalExitRoom)
                {
                    regularRoomList.Add(roomTransform);
                }
            }

            if (portal.PortalType == PortalType.In)
                roomDataMap[roomTransform].InPortal = portal;
            else
                roomDataMap[roomTransform].OutPortal = portal;
        }

        // 3. 일반 방들의 순서를 무작위로 마구 섞습니다 (피셔-예이츠 셔플)
        ShuffleList(regularRoomList);

        // 4. 모든 방을 순회하도록 꼬리물기 연결 시작
        ConnectAllRoomsSequentially();
    }

    private void ConnectAllRoomsSequentially()
    {
        // 일반 방들끼리 먼저 순서대로 꼬리를 뭅니다.
        // 예: 0번방 Out -> 1번방 In / 1번방 Out -> 2번방 In ...
        for (int i = 0; i < regularRoomList.Count - 1; i++)
        {
            Transform currentRoom = regularRoomList[i];
            Transform nextRoom = regularRoomList[i + 1];

            MoveRoomPortal currentOut = roomDataMap[currentRoom].OutPortal;
            MoveRoomPortal nextIn = roomDataMap[nextRoom].InPortal;

            if (currentOut != null && nextIn != null)
            {
                currentOut.ConnectTo(nextIn);
                nextIn.ConnectTo(currentOut); // 양방향 이동 가능하도록
            }
        }

        // 5. [가장 중요] 셔플된 일반 방의 '가장 마지막 Out 포탈'을 '특정 최종 방의 In 포탈'과 연결
        if (regularRoomList.Count > 0 && finalExitRoom != null)
        {
            Transform lastRegularRoom = regularRoomList[regularRoomList.Count - 1];
            Transform exitRoomTransform = finalExitRoom.transform;

            MoveRoomPortal finalOutPortal = roomDataMap[lastRegularRoom].OutPortal;
            MoveRoomPortal specificInPortal = roomDataMap[exitRoomTransform].InPortal;

            if (finalOutPortal != null && specificInPortal != null)
            {
                finalOutPortal.ConnectTo(specificInPortal);
                specificInPortal.ConnectTo(finalOutPortal);

                Debug.Log(
                    $"[포탈 시스템] 셔플 결과 마지막 방({lastRegularRoom.name})의 Out포탈이 특정 최종 방({finalExitRoom.name})의 In포탈과 연결되었습니다!"
                );
            }
        }
    }

    // 리스트 셔플 알고리즘
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
