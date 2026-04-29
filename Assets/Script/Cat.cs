using UnityEngine;

public class Cat : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private KeyCode giveKey = KeyCode.E;
    [SerializeField] private GameObject keySpawnPrefab;
    [SerializeField] private Transform keySpawnPosition;
    private bool canInteract = false;
    private Player player;
    private bool cheeseLocked = false;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Inventory inventory = collision.GetComponent<Inventory>();
            if (inventory != null && inventory.HasCheese())
            {
                canInteract = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = false;
        }
    }

    private void Update()
    {
        if (canInteract && Input.GetKeyDown(giveKey) && !cheeseLocked)
        {
            GiveCheeseToCat();
        }
    }

    private void GiveCheeseToCat()
    {
        cheeseLocked = true;

        if (player != null)
        {
            Inventory inventory = player.GetComponent<Inventory>();
            if (inventory != null && inventory.HasCheese())
            {
                inventory.RemoveCheese();
                Debug.Log("Gave cheese to cat! Key appears!");
                if (SoundManager.Instance != null)
                    SoundManager.Instance.PlaySFX("Meow");

                // Spawn the key
                SpawnKey();
                canInteract = false;
            }
        }
    }

    private void SpawnKey()
    {
        Vector3 spawnPos = keySpawnPosition != null ? keySpawnPosition.position : transform.position + Vector3.up * 2f;

        if (keySpawnPrefab != null)
        {
            Instantiate(keySpawnPrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Key prefab not assigned!");
        }
    }

    public void OnGUI()
    {
        if (canInteract && !cheeseLocked)
        {
            GUI.Label(new Rect(10, 10, 200, 30), "Press E to give cheese to cat");
        }
    }
}
