using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ClearKeys();
            GameManager.Instance.ResetPlayerHealthToDefault();
        }
        else
        {
            PlayerPrefs.DeleteKey("PlayerKeys");
            PlayerPrefs.Save();
        }

        StartCoroutine(SceneFader.Instance.FadeOut("Room1"));
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
