using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BloodScreen : MonoBehaviour
{
    private Image image;
    public float effectTime;
    
    void Start()
    {
        // 이미지 컴포넌트를 가져오며,
        // 플레이어가 대미지를 받았을 때 호출할 기능에 AppearBloodScreen() 추가
        if (TryGetComponent(out image))
            GameManager.Instance.Player.status.OnTakeDamage += AppearBloodScreen;
    }
    
    // 코루틴 진행 여부를 파악하기 위해 사용
    private Coroutine coroutine;
    
    // 코루틴 진행 중 재진입으로 인한 오류를 방지하며, 피격 효과 시작
    void AppearBloodScreen()
    {
        if(coroutine != null)
            StopCoroutine(coroutine);
        
        coroutine = StartCoroutine(BloodScreenEffect());
    }
    
    // 피격 상태 표시를 위한 효과
    // 시간이 지날수록 투명해지며 투명도가 0이 되면 이미지 컴포넌트 비활성화
    IEnumerator BloodScreenEffect()
    {
        float startAlpha = 0.5f;
        image.color = new Color(image.color.r, image.color.g, image.color.b, startAlpha);
        float alpha = startAlpha;
        image.enabled = true;

        while (alpha > 0)
        {
            alpha -= startAlpha * Time.deltaTime / effectTime;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return null;
        }
        
        image.enabled = false;
    }
    
}