using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField] InputActionProperty pauseAction;
    [SerializeField] Canvas pauseCanvas;
    bool paused = false;

    private void Update()
    {
        if (pauseAction.action.WasPerformedThisFrame())
        {
            SwitchPause(!paused);
        }
    }

    public void SwitchPause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
            pauseCanvas.gameObject.SetActive(true);
            FindObjectOfType<AudioSource>().Pause();
        }
        else
        {
            Time.timeScale = 1;
            pauseCanvas.gameObject.SetActive(false);
            FindObjectOfType<AudioSource>().Play();
        }
    }

    public void LoadMenu()
    {
        StartCoroutine(LoadMenuCrt());
    }

    IEnumerator LoadMenuCrt()
    {
        Time.timeScale = 1f;
        FindObjectOfType<Player>().shouldNotMove = true;
        FindObjectOfType<FadeScreen>().FadeOut();
        yield return new WaitForSeconds(FindObjectOfType<FadeScreen>().fadeDuration);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }    
}
