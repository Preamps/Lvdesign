using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ClearKeys();
            GameManager.Instance.ClearSeenDialogues();
            GameManager.Instance.ResetPlayerHealthToDefault();
        }
        else
        {
            PlayerPrefs.DeleteKey("PlayerKeys");
            PlayerPrefs.DeleteKey("SeenDialogues");
            PlayerPrefs.Save();
        }

        StartCoroutine(SceneFader.Instance.FadeOut("Room1"));
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
