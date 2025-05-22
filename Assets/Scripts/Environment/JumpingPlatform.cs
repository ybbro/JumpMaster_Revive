using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPlatform : MonoBehaviour
{
    // �ش� �÷����� ��� ������Ʈ�� �󸶸�ŭ ���� ������
    public float jumpForce;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            CharacterManager.Instance.Player.control.Jump(jumpForce);
    }
}
