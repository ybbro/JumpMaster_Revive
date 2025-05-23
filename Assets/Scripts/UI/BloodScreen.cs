using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BloodScreen : MonoBehaviour
{
    private Image image;
    public float effectTime;
    
    void Start()
    {
        // �̹��� ������Ʈ�� ��������,
        // �÷��̾ ������� �޾��� �� ȣ���� ��ɿ� AppearBloodScreen() �߰�
        if (TryGetComponent(out image))
            GameManager.Instance.Player.status.OnTakeDamage += AppearBloodScreen;
    }
    
    // �ڷ�ƾ ���� ���θ� �ľ��ϱ� ���� ���
    private Coroutine coroutine;
    
    // �ڷ�ƾ ���� �� ���������� ���� ������ �����ϸ�, �ǰ� ȿ�� ����
    void AppearBloodScreen()
    {
        if(coroutine != null)
            StopCoroutine(coroutine);
        
        coroutine = StartCoroutine(BloodScreenEffect());
    }
    
    // �ǰ� ���� ǥ�ø� ���� ȿ��
    // �ð��� �������� ���������� ������ 0�� �Ǹ� �̹��� ������Ʈ ��Ȱ��ȭ
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