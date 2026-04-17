using UnityEngine;

public class GunPickup : MonoBehaviour
{
    private void Start()
    {
        // 🔥 ถ้าเคยเก็บปืนแล้ว → ไม่ต้องเกิดอีก
        if (GameManager.Instance != null && GameManager.Instance.HasGun())
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ชนแล้ว: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("เจอ Player แล้ว");

            // ✅ บันทึกลง GameManager (สำคัญมาก)
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddGun();
            }

            // ✅ เปิดปืนใน Scene ปัจจุบัน
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.EnableGun();
            }

            Destroy(gameObject);
        }
    }
}