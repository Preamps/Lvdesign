using UnityEngine;

public class GunPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log("ชนแล้ว: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("เจอ Player แล้ว");

            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.EnableGun();
                Destroy(gameObject);
            }
        }
    }
}