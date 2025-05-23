using System;
using UnityEngine;
using System.Collections;

// ������� ���� �� �ִ� ������Ʈ�鿡 �ο��� �������̽�
public interface IDamagable
{
    void TakeDamage(float damage);
}

public class PlayerStatus : MonoBehaviour, IDamagable
{
    // ü���� UI�� Condition ��ũ��Ʈ�� ��� �ְԲ� >> ü�¹ٿ� ����
    [SerializeField] Condition HP;

    // �÷��̾ ������� �޾��� �� ������ ����
    public event Action OnTakeDamage;
    
    // �������̽��κ��� ������ ������ TakeDamage(float damage)
    // ������� �� �� �ش� �޼��� ȣ��
    public void TakeDamage(float damage)
    {
        // ü�� ��ȭ
        HP.Change(-damage);
        // �÷��̾ ������� �޾��� �� ������ ����
        OnTakeDamage?.Invoke();
    }

    // ü�� ȸ��
    public void Heal(float healAmount)
    {
        HP.Change(healAmount);
    }
}
