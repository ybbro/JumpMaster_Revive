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
    
    // ���
    // ���
    [HideInInspector] public int idleState = Animator.StringToHash("Base Layer.Unarmed.Idle");
    // �ȱ�, �޸��� ����
    [HideInInspector] public int walkState = Animator.StringToHash("Base Layer.Unarmed.Walk");
    [HideInInspector] public int runState = Animator.StringToHash("Base Layer.Unarmed.Run");
    // ���� �ִϸ��̼��� ��Ȳ�� �°� ���� ���� 
    // ���� �� �ְ������� ü��, ���߿� �� ����, ���� 3�ܰ�� ����
    [HideInInspector] public int jumpState = Animator.StringToHash("Base Layer.Unarmed.Jump");
    [HideInInspector] public int fallState = Animator.StringToHash("Base Layer.Unarmed.Falling");
    [HideInInspector] public int landState = Animator.StringToHash("Base Layer.Unarmed.Landing");
    // �޽� ��� ����
    public readonly int restState01 = Animator.StringToHash("Base Layer.Unarmed.Rest01");
    public readonly int restState02 = Animator.StringToHash("Base Layer.Unarmed.Rest02");
    public readonly int restState03 = Animator.StringToHash("Base Layer.Unarmed.Rest03");
    // �����̵� �߰�
    [HideInInspector] public int slideState = Animator.StringToHash("Base Layer.Unarmed.Slide");

    private void Awake()
    {
        TryGetComponent(out anim);
    }
    
    void Update()
    {
        // ���� �ִϸ��̼� ����
        stateInfo_current = anim.GetCurrentAnimatorStateInfo(0);
    }
}
