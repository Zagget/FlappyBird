using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour
{
    public static IEnumerator FadeInOrOut(SpriteRenderer sr, float fadeDuration, float startAlpha, float endAlpha)
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, startAlpha);

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);

            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

            yield return null;
        }
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, endAlpha);
    }
}