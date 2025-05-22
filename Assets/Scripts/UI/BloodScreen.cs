using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BloodScreen : MonoBehaviour
{
    private Image image;
    public float effectTime;
    
    void Start()
    {
        if (TryGetComponent(out image))
            CharacterManager.Instance.Player.status.OnTakeDamage += AppearBloodScreen;
    }
    
    private Coroutine coroutine;
    public void AppearBloodScreen()
    {
        if(coroutine != null)
            StopCoroutine(coroutine);
        
        coroutine = StartCoroutine(FadeAway());
    }

    IEnumerator FadeAway()
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