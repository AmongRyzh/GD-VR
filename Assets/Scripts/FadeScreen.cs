using System.Collections;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    [SerializeField] bool fadeOnStart = true;
    public float fadeDuration = 0.5f;
    [SerializeField] Color fadeColor;
    MeshRenderer rend;

    private void Start()
    {
        rend = GetComponent<MeshRenderer>();
        if (fadeOnStart) FadeIn();
    }

    public void FadeIn()
    {
        Fade(1, 0);
    }

    public void FadeOut()
    {
        Fade(0, 1);
    }

    private void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeCrt(alphaIn, alphaOut));
    }

    private IEnumerator FadeCrt(float alphaIn, float alphaOut)
    {
        float timer = 0;
        while (timer <= fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);

            rend.material.SetColor("_BaseColor", newColor);

            timer += Time.deltaTime;
            yield return null;
        }

        Color newColor2 = fadeColor;
        newColor2.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);

        rend.material.SetColor("_BaseColor", newColor2);
    }
}
