using UnityEngine;

public class Condition : MonoBehaviour
{
    public float
        current, // ����
        init, // �ʱ�
        max, // �ִ�
        passive; // �ڿ������� ��ȭ�� ��
    
    void Start()
    {
        // �ʱ� ������ �ʱ�ȭ
        current = init;
    }
    
    void Update()
    {
        // �ڿ������� ��ȭ�ϴ� ��
        Change(passive * Time.deltaTime);
        // HP�� ǥ�� ��ȭ
        transform.localScale = Vector3.right * GetPercentage() + Vector3.up + Vector3.forward;
    }

    float GetPercentage()
    {
        // HP ����
        return current / max;
    }
    
    public void Change(float value)
    {   // �ִ�, �ּҰ��� ����� �ʰ� clamp
        current = Mathf.Clamp(current + value, 0, max);
    }
}
