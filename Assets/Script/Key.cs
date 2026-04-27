using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private string keyID; // 🔥 ตั้งใน Inspector

    private void Start()
    {
        if (GameManager.Instance.HasKey(keyID))
        {
            Destroy(gameObject); // กัน spawn ซ้ำ
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager.Instance.AddKey(keyID);
        Destroy(gameObject);
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX("GetKey");
    }
}