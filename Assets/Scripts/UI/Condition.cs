using UnityEngine;

public class Condition : MonoBehaviour
{
    public float
        current, // 현재
        init, // 초기
        max, // 최대
        passive; // 자연적으로 변화할 값
    
    void Start()
    {
        // 초기 값으로 초기화
        current = init;
    }
    
    void Update()
    {
        // 자연적으로 변화하는 값
        Change(passive * Time.deltaTime);
        // HP바 표시 변화
        transform.localScale = Vector3.right * GetPercentage() + Vector3.up + Vector3.forward;
    }

    float GetPercentage()
    {
        // HP 비율
        return current / max;
    }
    
    public void Change(float value)
    {   // 최대, 최소값을 벗어나지 않게 clamp
        current = Mathf.Clamp(current + value, 0, max);
    }
}
