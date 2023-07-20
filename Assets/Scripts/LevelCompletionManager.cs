using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletionManager : MonoBehaviour
{
    public void LoadLevel(bool loadMenu)
    {
        FindObjectOfType<FadeScreen>().FadeOut();
        StartCoroutine(LoadLevelCrt(loadMenu));
    }

    IEnumerator LoadLevelCrt(bool loadMenu)
    {
        yield return new WaitForSeconds(FindObjectOfType<FadeScreen>().fadeDuration);
        if (loadMenu) SceneManager.LoadScene(0);
        else SceneManager.LoadScene(1);
    }
}
