using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPlatform : MonoBehaviour
{
    // 해당 플랫폼을 밟는 오브젝트에 얼마만큼 힘을 가할지
    public float jumpForce;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            CharacterManager.Instance.Player.control.Jump(jumpForce);
    }
}
