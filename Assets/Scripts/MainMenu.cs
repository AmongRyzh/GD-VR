using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    int levelToLoad;
    [SerializeField] GameObject levelInfo;
    [SerializeField] Text levelName;
    [SerializeField] CurrentLevel currentLevel;

    Level savedLevelInfo;

    private void Start()
    {
        levelInfo.SetActive(false);
    }

    public void SetLevelInfo(Level info)
    {
        levelInfo.SetActive(true);

        levelName.text = info.levelName;

        savedLevelInfo = info;
    }

    public void LoadLevel()
    {
        currentLevel.currentLevelInfo = savedLevelInfo;
        FindObjectOfType<FadeScreen>().FadeOut();
        StartCoroutine(WaitForFadeScreen());
    }

    IEnumerator WaitForFadeScreen()
    {
        yield return new WaitForSeconds(FindObjectOfType<FadeScreen>().fadeDuration);
        SceneManager.LoadScene(1);
    }    
}
