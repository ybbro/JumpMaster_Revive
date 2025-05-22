using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class Interaction : MonoBehaviour
{
    public float checkRadius = 2;
    public float checkDelay = 0.05f;
    [SerializeField] TextMeshProUGUI textInfo;
    public LayerMask layerMask;
    
    private IInteractable curInteractable;
    
    void Start()
    {
        // 반복적으로 상호가능 오브젝트 탐색
        InvokeRepeating("SearchInteractables", 0, checkDelay);
    }

    void SearchInteractables()
    {
        // 해당 스크립트를 부착한 오브젝트가 캐릭터 앞쪽에 위치하여 캐릭터 앞의 상호작용 가능한 물체들을 탐색
        // 구 형태로 레이를 쏘아 맞은 상호작용 가능한 오브젝트들
        Collider[] hits = Physics.OverlapSphere(transform.position,checkRadius,layerMask);
        // 플레이어 위치
        Vector3 playerPos = CharacterManager.Instance.Player.transform.position;
        
        // 맞은 오브젝트가 없다면: 정보를 표시하지 않음
        if (hits.Length == 0)
        {
            curInteractable = null;
            textInfo.text = "";
        }
        // 맞은 오브젝트가 있다면: 정보 표시
        else
        {
            // 플레이어로부터 가장 가까운 상호작용 가능한 오브젝트가 0번에 오게끔 정렬
            Array.Sort(hits, (a, b) 
                => Vector3.SqrMagnitude(a.transform.position - playerPos).CompareTo(Vector3.SqrMagnitude(b.transform.position - playerPos)));
            
            // 가장 가까운 상호작용 가능 오브젝트의 정보 출력
            if (hits[0].TryGetComponent(out curInteractable))
            {
                textInfo.text = curInteractable.GetInfo();
            }
        }
    }

    // 범위가 어떤지 보기 위한 디버그용 영역 표시
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position,checkRadius);
    }
}
