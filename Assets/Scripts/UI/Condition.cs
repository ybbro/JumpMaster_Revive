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
    {   // 최대, 최소값을 벗어나지 않게 clamp
        current = Mathf.Clamp(current + value, 0, max);
    }
}
