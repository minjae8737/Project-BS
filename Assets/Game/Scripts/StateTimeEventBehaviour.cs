using System;
using UnityEngine;

public class StateTimeEventBehaviour : StateMachineBehaviour
{
    [Serializable]
    private struct TimedEvent
    {
        public AniClipEventName eventName;
        [Range(0f, 1f)] public float normalizedTime;
    }

    [SerializeField] private TimedEvent[] events;

    private IStateTimeEventReceiver receiver;
    private bool receiverCached;
    private float previousTime;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Behaviour 인스턴스는 Animator마다 따로 생성되므로 캐싱해도 안전
        if (!receiverCached)
        {
            receiver = animator.GetComponent<IStateTimeEventReceiver>();
            receiverCached = true;
        }

        // 0 지점 이벤트도 첫 Update에서 잡히도록 0보다 살짝 작게 시작
        previousTime = -0.0001f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (receiver == null || events == null) return;

        float currentTime = stateInfo.normalizedTime;

        for (int i = 0; i < events.Length; i++)
        {
            if (Crossed(previousTime, currentTime, events[i].normalizedTime, stateInfo.loop))
            {
                receiver.OnStateTimeEvent(events[i].eventName);
            }
        }

        previousTime = currentTime;
    }

    private static bool Crossed(float prev, float cur, float t, bool loop)
    {
        // Loop가 아닌 상태도 normalizedTime은 1을 넘어 계속 증가하므로 단순 비교
        if (!loop) return prev < t && cur >= t;

        // Loop 상태: 정수부(루프 횟수)까지 고려해 매 루프마다 한 번씩 발생
        return Mathf.FloorToInt(cur - t) > Mathf.FloorToInt(prev - t);
    }
}