using UnityEngine;

public class FuseItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.CompareTag("Player"))
        {
           
            GameManager.Instance.AddFuse();
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlaySFX("FusePick");

            Destroy(gameObject);
        }
    }
}