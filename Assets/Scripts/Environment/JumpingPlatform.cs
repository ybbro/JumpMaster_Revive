using UnityEngine;

public class JumpingPlatform : MonoBehaviour
{
    // 해당 플랫폼을 밟는 오브젝트에 얼마만큼 힘을 가할지
    public float jumpForce;
    
    // 플레이어가 닿으면 점프대의 힘만큼 점프
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            GameManager.Instance.Player.control.Jump(jumpForce);
    }
}
