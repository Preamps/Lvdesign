using UnityEngine;

public class FuseBox : MonoBehaviour
{
    public int requiredFuses = 4;
    public GameObject door;    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           
            if (GameManager.Instance.currentFuses >= requiredFuses)
            {
                OpenDoor();
            }
            else
            {
                Debug.Log("Fuse not enough!" + (requiredFuses - GameManager.Instance.currentFuses));
            }
        }
    }

    void OpenDoor()
    {
        Debug.Log("Door opened!");
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX("FuseDone");

        door.SetActive(false); 
        
    }
}