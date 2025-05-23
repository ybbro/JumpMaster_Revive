using UnityEngine;

public class JumpingPlatform : MonoBehaviour
{
    // �ش� �÷����� ��� ������Ʈ�� �󸶸�ŭ ���� ������
    public float jumpForce;
    
    // �÷��̾ ������ �������� ����ŭ ����
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            GameManager.Instance.Player.control.Jump(jumpForce);
    }
}
