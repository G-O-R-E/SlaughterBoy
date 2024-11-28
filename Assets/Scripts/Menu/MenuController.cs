using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        Input.ResetInputAxes();
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }

    public void QuitGame(string name)
    {
        GameManager.instance.ResetValue();
        Input.ResetInputAxes();
        SceneManager.LoadScene(name);
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PauseGame(GameObject panel)
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseOptions(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void UnPauseGame(GameObject panel)
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }
}