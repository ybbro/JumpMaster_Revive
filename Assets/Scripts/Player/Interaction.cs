using UnityEngine;
using TMPro;
using System;
using System.Linq;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRadius = 1; // üũ ������ �ݰ�
    public float checkDelay = 0.2f; // üũ �����̰� �ʹ� ª���� �ּ� 4������ �ֱ�� �ؾ� �Ѵٴ� ��� �ַ�� ����
    [SerializeField] TextMeshProUGUI textInfo; // ������ ǥ���� �ؽ�Ʈ UI
    public LayerMask layerMask; // Interaction ���̾ �����ϴ� ���̾��ũ
    
    private IInteractable curInteractable;
    
    void Start()
    {
        // �ݺ������� ��ȣ���� ������Ʈ Ž��
        InvokeRepeating("SearchInteractables", 0, checkDelay);
    }

    void SearchInteractables()
    {
        // �ش� ��ũ��Ʈ�� ������ ������Ʈ�� ĳ���� ���ʿ� ��ġ�Ͽ� ĳ���� ���� ��ȣ�ۿ� ������ ��ü���� Ž��
        // �� ���·� ���̸� ��� ���� ��ȣ�ۿ� ������ ������Ʈ��
        Collider[] hits = Physics.OverlapSphere(transform.position,checkRadius,layerMask);
        // �÷��̾� ��ġ
        Vector3 playerPos = GameManager.Instance.Player.transform.position;
        
        // ���� ������Ʈ�� ���ٸ�: ������ ǥ������ ����
        if (hits.Length == 0)
        {
            curInteractable = null;
            textInfo.text = "";
        }
        // ���� ������Ʈ�� �ִٸ�: ���� ǥ��
        else
        {
            // �÷��̾�κ��� ���� ����� ��ȣ�ۿ� ������ ������Ʈ�� 0���� ���Բ� ����
            Array.Sort(hits, (a, b) 
                => Vector3.SqrMagnitude(a.transform.position - playerPos).CompareTo(Vector3.SqrMagnitude(b.transform.position - playerPos)));
            
            // ���� ����� ��ȣ�ۿ� ���� ������Ʈ�� ���� ���
            if (hits[0].TryGetComponent(out curInteractable))
            {
                textInfo.text = curInteractable.GetInfo();
            }
        }
    }

    // ��ȣ�ۿ� ��� �ߵ�
    public void OnInteraction(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            curInteractable?.OnInteract();
        }
    }

    // ������ ��� ���� ���� ����׿� ���� ǥ��
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position,checkRadius);
    }
}
