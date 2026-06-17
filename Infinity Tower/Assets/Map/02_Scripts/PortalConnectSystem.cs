using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalConnectSystem : MonoBehaviour
{
    [Header("Final Destination")]
    [SerializeField]
    private GameObject finalExitRoom; // 인스펙터에서 그대로 지정해두는 최종 방

    private List<MoveRoomPortal> tempComponents = new List<MoveRoomPortal>();

    // 방(부모)별 포탈 그룹 관리를 위한 딕셔너리
    private Dictionary<Transform, RoomPortalGroup> roomDataMap =
        new Dictionary<Transform, RoomPortalGroup>();

    // 분류될 방 트리거 변수들
    private Transform startShopRoom = null;
    private List<Transform> regularRoomList = new List<Transform>();

    private class RoomPortalGroup
    {
        public MoveRoomPortal InPortal;
        public MoveRoomPortal OutPortal;
    }

    private void Start()
    {
        // 런타임에 상점이 스폰되는 시간을 고려해 Awake 대신 Start에서 수집합니다.
        StartCoroutine(waitOneFrame());
    }

    IEnumerator waitOneFrame()
    {
        yield return null;

        InitializeAndConnect();
    }

    public void InitializeAndConnect()
    {
        tempComponents.Clear();
        roomDataMap.Clear();
        regularRoomList.Clear();
        startShopRoom = null;

        // 1. 맵 내의 모든 포탈 컴포넌트 수집 (새로 스폰된 상점 포탈도 포함됨)
        GetComponentsInChildren(true, tempComponents);

        // 2. 부모(방)를 기준으로 포탈 그룹화
        foreach (var comp in tempComponents)
        {
            MoveRoomPortal portal = comp;
            if (portal == null)
                continue;

            Transform roomTransform = portal.transform.parent;

            if (!roomDataMap.ContainsKey(roomTransform))
            {
                roomDataMap[roomTransform] = new RoomPortalGroup();
            }

            if (portal.PortalType == PortalType.In)
                roomDataMap[roomTransform].InPortal = portal;
            else
                roomDataMap[roomTransform].OutPortal = portal;
        }

        // 3. [핵심 조건] 포탈 구성 상태를 보고 상점 방과 일반 방을 분류합니다.
        foreach (var roomTransform in roomDataMap.Keys)
        {
            // 인스펙터로 지정한 최종 방은 일반 셔플 리스트에서 제외합니다.
            if (roomTransform.gameObject == finalExitRoom)
                continue;

            // 조건: "InPortal이 없고 OutPortal만 존재한다" -> 이 방이 바로 새로 스폰된 상점 방!
            if (
                roomDataMap[roomTransform].InPortal == null
                && roomDataMap[roomTransform].OutPortal != null
            )
            {
                startShopRoom = roomTransform;
            }
            else
            {
                // 둘 다 있거나 일반적인 방들은 셔플 리스트로 분류
                regularRoomList.Add(roomTransform);
            }
        }

        // 4. 일반 방들의 순서만 무작위로 섞음 (피셔-예이츠 셔플)
        ShuffleList(regularRoomList);

        // 5. 전체 방 순서대로 안전하게 연결 시작
        ConnectAllRoomsStructure();
    }

    private void ConnectAllRoomsStructure()
    {
        if (startShopRoom == null || finalExitRoom == null)
        {
            Debug.LogError(
                $"[포탈 시스템] 연결 실패! 자동 스캔된 상점 방: {startShopRoom != null}, 고정 최종 방: {finalExitRoom != null}"
            );
            return;
        }

        Transform exitTransform = finalExitRoom.transform;

        // 일반 방이 하나도 없는 특수 상황 예외 처리 (상점 -> 최종방 바로 연결)
        if (regularRoomList.Count == 0)
        {
            LinkTwoRooms(startShopRoom, exitTransform);
            return;
        }

        // -------------------------------------------------------------
        // 과정 A: [새로 스폰된 상점 방의 Out] ➔ [셔플된 첫 번째 일반 방의 In] 연결
        // -------------------------------------------------------------
        Transform firstRegularRoom = regularRoomList[0];
        LinkTwoRooms(startShopRoom, firstRegularRoom);

        // -------------------------------------------------------------
        // 과정 B: 중간 일반 방들끼리 순서대로 꼬리물기 (중간 경로 완전 순회)
        // -------------------------------------------------------------
        for (int i = 0; i < regularRoomList.Count - 1; i++)
        {
            LinkTwoRooms(regularRoomList[i], regularRoomList[i + 1]);
        }

        // -------------------------------------------------------------
        // 과정 C: [셔플된 마지막 일반 방의 Out] ➔ [고정된 최종 방의 In] 연결
        // -------------------------------------------------------------
        Transform lastRegularRoom = regularRoomList[regularRoomList.Count - 1];
        LinkTwoRooms(lastRegularRoom, exitTransform);

        Debug.Log(
            $"[포탈 시스템] 하이브리드 연결 성공! 스폰된 상점({startShopRoom.name}) ➔ (일반방 {regularRoomList.Count}개 순회) ➔ 고정 최종방({finalExitRoom.name})"
        );
    }

    // 두 방의 Out 포탈과 In 포탈을 안전하게 연결해주는 메서드
    private void LinkTwoRooms(Transform fromRoom, Transform toRoom)
    {
        if (roomDataMap.ContainsKey(fromRoom) && roomDataMap.ContainsKey(toRoom))
        {
            MoveRoomPortal outPortal = roomDataMap[fromRoom].OutPortal;
            MoveRoomPortal inPortal = roomDataMap[toRoom].InPortal;

            if (outPortal != null && inPortal != null)
            {
                outPortal.ConnectTo(inPortal);

                // 역방향(되돌아가기)은 두 방 모두 상점/출구가 아닐 때(일반 루프방일 때)만 안전하게 생성
                if (roomDataMap[fromRoom].InPortal != null && roomDataMap[toRoom].OutPortal != null)
                {
                    inPortal.ConnectTo(outPortal);
                }
            }
        }
    }

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
