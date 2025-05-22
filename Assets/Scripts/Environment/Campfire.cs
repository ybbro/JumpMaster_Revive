using UnityEngine;
using System.Collections.Generic;

namespace Environment
{
    [RequireComponent(typeof(Collider))]
    public class Campfire: MonoBehaviour
    {
        public int damage;
        public float damageDelay;
        List<IDamagable> damagables = new List<IDamagable>();
        private void Start()
        {
            InvokeRepeating("DealDamage", 0, damageDelay);
        }

        void DealDamage()
        {
            for (int i = 0; i < damagables.Count; i++)
            {
                damagables[i].TakeDamage(damage);
            }
        }
        
        private void Awake()
        {
            TryGetComponent(out Collider col);
            col.isTrigger = true;
        }

        
        // 캠프파이어 불에 출입하는 대미지를 줄 수 있는 오브젝트들에 대해 이를 등록/해제
        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out IDamagable damagable))
                damagables.Add(damagable);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if(other.TryGetComponent(out IDamagable damagable))
                damagables.Remove(damagable);
        }
    }
}