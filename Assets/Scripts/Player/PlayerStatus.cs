using System;
using UnityEngine;
using System.Collections;

// 대미지를 받을 수 있는 오브젝트들에 부여할 인터페이스
public interface IDamagable
{
    void TakeDamage(float damage);
}

public class PlayerStatus : MonoBehaviour, IDamagable
{
    // 체력을 UI의 Condition 스크립트가 들고 있게끔 >> 체력바와 연동
    [SerializeField] Condition HP;

    // 플레이어가 대미지를 받았을 때 수행할 내용
    public event Action OnTakeDamage;
    
    // 인터페이스로부터 구현이 강제된 TakeDamage(float damage)
    // 대미지를 줄 때 해당 메서드 호출
    public void TakeDamage(float damage)
    {
        // 체력 변화
        HP.Change(-damage);
        // 플레이어가 대미지를 받았을 때 수행할 내용
        OnTakeDamage?.Invoke();
    }

    // 체력 회복
    public void Heal(float healAmount)
    {
        HP.Change(healAmount);
    }
}
