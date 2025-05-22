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
        current = init;
    }
    
    void Update()
    {
        Change(passive * Time.deltaTime);
        transform.localScale = Vector3.right * GetPercentage() + Vector3.up + Vector3.forward;
    }

    float GetPercentage()
    {
        return current / max;
    }
    
    public void Change(float value)
    {   // �ִ�, �ּҰ��� ����� �ʰ� clamp
        current = Mathf.Clamp(current + value, 0, max);
    }
}
