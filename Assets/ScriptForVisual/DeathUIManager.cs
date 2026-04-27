using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUIManager : MonoBehaviour
{
    public GameObject deathUI;

    private void Start()
    {
        deathUI.SetActive(false);  // Hide at start
    }

    public void ShowDeathUI()
    {
        deathUI.SetActive(true);
        Time.timeScale = 0f;

        // Show normal mouse
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SoundManager.Instance.PlaySFX("Lose");

        // Hide crosshair
        FindObjectOfType<CrosshairController>().crosshair.gameObject.SetActive(false);
    }


    public void RestartGame()
    {
        Time.timeScale = 1f;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveKeys();
            GameManager.Instance.ResetPlayerHealthToDefault();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetPlayerHealthToDefault();
        }

        SceneManager.LoadScene("MainMenu"); // change to your menu scene name
    }
}
