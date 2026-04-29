using UnityEngine;
using UnityEngine.SceneManagement;

// Manager for cheese puzzle state
public class CheesePuzzle : MonoBehaviour
{
    public static CheesePuzzle Instance;
    private bool puzzleComplete = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset puzzle when returning to main menu
        if (scene.name == "MainMenu" || scene.name == "Menu")
        {
            ResetPuzzle();
        }
    }

    public void CompletePuzzle()
    {
        puzzleComplete = true;
        Debug.Log("Cheese puzzle completed!");
    }

    public bool IsPuzzleComplete()
    {
        return puzzleComplete;
    }

    public void ResetPuzzle()
    {
        puzzleComplete = false;
        Debug.Log("Cheese puzzle reset!");
    }
}
