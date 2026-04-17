using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] public string nextSceneName;
    [SerializeField] public string doorID;

    [SerializeField] public bool requireKey = true;
    [SerializeField] private string requiredKeyID; // 🔥 เพิ่ม

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // ❌ ไม่ใช้กุญแจ
        if (!requireKey)
        {
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        // ✅ เคยปลดล็อกแล้ว
        if (GameManager.Instance.IsDoorUnlocked(doorID))
        {
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        // 🔑 เช็คกุญแจ "เฉพาะดอก"
        if (GameManager.Instance.HasKey(requiredKeyID))
        {
            GameManager.Instance.UnlockDoor(doorID);
            Debug.Log("ปลดล็อกประตูแล้ว!");

            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.Log("ต้องใช้กุญแจ: " + requiredKeyID);
        }
    }
}