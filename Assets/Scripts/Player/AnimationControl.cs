using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityChan;

public enum AnimState
{
    Idle,
    Move,
    Running,
    Jump,
    Fall,
    Land,
    Rest,
    RestRandom,
}

[RequireComponent(typeof(Animator))]
public class AnimationControl : MonoBehaviour
{
    Animator anim;
    
    public Animator Animator => anim;

    public int[] state = new int[]
    {
        Animator.StringToHash("Idle"),
        Animator.StringToHash("Move"),
        Animator.StringToHash("Running"),
        Animator.StringToHash("Jump"),
        Animator.StringToHash("Fall"),
        Animator.StringToHash("Land"),
        Animator.StringToHash("Rest"),
        Animator.StringToHash("RestRandom"),
    };
    
    public AnimatorStateInfo stateInfo_current { get; private set; }
    
    // 모션
    // 대기
    [HideInInspector] public int idleState = Animator.StringToHash("Base Layer.Unarmed.Idle");
    // 걷기, 달리기 구분
    [HideInInspector] public int walkState = Animator.StringToHash("Base Layer.Unarmed.Walk");
    [HideInInspector] public int runState = Animator.StringToHash("Base Layer.Unarmed.Run");
    // 점프 애니메이션을 상황에 맞게 쓰기 위해 
    // 점프 후 최고점까지 체공, 공중에 뜬 상태, 착지 3단계로 분할
    [HideInInspector] public int jumpState = Animator.StringToHash("Base Layer.Unarmed.Jump");
    [HideInInspector] public int fallState = Animator.StringToHash("Base Layer.Unarmed.Falling");
    [HideInInspector] public int landState = Animator.StringToHash("Base Layer.Unarmed.Landing");
    // 휴식 모션 구분
    public readonly int restState01 = Animator.StringToHash("Base Layer.Unarmed.Rest01");
    public readonly int restState02 = Animator.StringToHash("Base Layer.Unarmed.Rest02");
    public readonly int restState03 = Animator.StringToHash("Base Layer.Unarmed.Rest03");
    // 슬라이딩 추가
    [HideInInspector] public int slideState = Animator.StringToHash("Base Layer.Unarmed.Slide");

    private void Awake()
    {
        TryGetComponent(out anim);
    }
    
    void Update()
    {
        // 현재 애니메이션 상태
        stateInfo_current = anim.GetCurrentAnimatorStateInfo(0);
    }
}
