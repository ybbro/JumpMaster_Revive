using System;
using UnityEngine;

public interface IDamagable
{
    void TakeDamage(float damage);
}

public class PlayerStatus : MonoBehaviour, IDamagable
{
    [SerializeField] Condition HP;

    public event Action OnTakeDamage;

    public void TakeDamage(float damage)
    {
        HP.Change(-damage);
        OnTakeDamage?.Invoke();
    }
}
