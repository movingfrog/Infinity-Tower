using System.Runtime.InteropServices;
using TMPro.EditorUtilities;
using Unity.Collections;
using UnityEngine;

public struct TimeData
{
    public Vector2 position;
    public TimeData(Vector2 position)
    {
        this.position = position;
    }
}

public class TimeBody : MonoBehaviour
{
    [Header("시간 기록 관련 변수")]
    [SerializeField, Range(5f, 10f)]
    private float recordTimeInternal = 5f;
    private static float tempRecordTime;
    public static float recordTime
    {
        get => tempRecordTime;
        set
        {
            tempRecordTime = value;
            SyncValue();
        }
    }

    static void SyncValue()
    {
        MAX_recordTime = Mathf.CeilToInt(recordTime / Time.fixedDeltaTime);
    }

    private static float MAX_recordTime;
    private int MAX_CAPACITY;
    private int _writeIndex = 0;
    private int _currentCount = 0;

    NativeArray<TimeData> stateData;

    private void Awake()
    {
        recordTime = recordTimeInternal;
        MAX_CAPACITY = Mathf.CeilToInt(10f / Time.fixedDeltaTime);
        SyncValue();
    }

    private void Start()
    {
        if (TimeManager.Instance == null) UnityEditor.EditorApplication.isPlaying = false;
    } //TimeManager없을 때 예외처리 코드

    private void OnValidate()
    {
        recordTime = recordTimeInternal;
    } //값이 바뀔 때 호출되는 함수(recordTimeInternal이 바뀌면 호출됨);

    private void FixedUpdate()
    {
        if (!TimeManager.Instance.isRewinding)
        {
            Record();
        }
    }

    void Record()
    {
        stateData[_writeIndex] = new TimeData(transform.position);
        _writeIndex = (_writeIndex + 1) % MAX_recordTime;
        if (_currentCount < MAX_recordTime) _currentCount++;
    }

    public void Rewind(int frameAgo)
    {
        int actualFrameAgo = Mathf.Min(frameAgo, MAX_recordTime);
        if (actualFrameAgo < 0) return;

        int readIndex = (_writeIndex -= actualFrameAgo + MAX_recordTime) % MAX_recordTime; //순환 버퍼로 인해 현재의 값의 위치를 찾는 식
        TimeData data = stateData[readIndex];

        transform.position = data.position;
    }

    public void ResetTimeDataAfterRewind(int frameAgo)
    {
        int lastRestoredIndex = (_writeIndex - frameAgo + MAX_recordTime) % MAX_recordTime; //현재 복원된 시점의 인덱스를 계산
        _writeIndex = lastRestoredIndex;
        _currentCount = Mathf.Max(0, _currentCount - frameAgo);
    }

    private void OnDestroy()
    {
        if(stateData.IsCreated) stateData.Dispose();
    }
}
