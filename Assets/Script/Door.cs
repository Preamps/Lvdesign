using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] public string nextSceneName;
    [SerializeField] public string doorID;

    [SerializeField]public bool requireKey = true; // ✅ ติ๊กเปิด/ปิดการใช้กุญแจ

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // ✅ ถ้า "ไม่ต้องใช้กุญแจ" → เข้าได้เลย
        if (!requireKey)
        {
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        // ✅ ถ้าเคยปลดล็อกแล้ว
        if (GameManager.Instance.IsDoorUnlocked(doorID))
        {
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        // 🔑 เช็คกุญแจ
        if (GameManager.Instance.HasKey())
        {
            GameManager.Instance.UnlockDoor(doorID);
            Debug.Log("ปลดล็อกประตูแล้ว!");

            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.Log("ต้องมีกุญแจก่อน!");
        }
    }
}