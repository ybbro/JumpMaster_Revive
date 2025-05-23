using UnityEngine;
using System.Collections.Generic;

namespace Environment
{
    [RequireComponent(typeof(Collider))] // 오브젝트를 부착할 때 콜라이더가 없다면 부여
    public class Campfire: MonoBehaviour
    {
        public int damage;
        public float damageDelay;
        List<IDamagable> damagables = new List<IDamagable>();
        
        private void Awake()
        {
            // 콜라이더를 트리거로 변경
            TryGetComponent(out Collider col);
            col.isTrigger = true;
        }
        
        private void Start()
        {
            // 대미지를 주는 주기마다, 영역 내 대미지를 줄 수 있는 오브젝트들에 대미지
            InvokeRepeating("DealDamage", 0, damageDelay);
        }
        
        void DealDamage()
        {
            for (int i = 0; i < damagables.Count; i++)
            {
                damagables[i].TakeDamage(damage);
            }
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