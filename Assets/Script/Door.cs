using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] public string SceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory inv = other.GetComponent<Inventory>();

            if (inv != null && inv.HasKey())
            {
                SceneManager.LoadScene(SceneName);
            }
            else
            {
                Debug.Log("ต้องมีกุญแจก่อน!");
            }
        }
    }
}