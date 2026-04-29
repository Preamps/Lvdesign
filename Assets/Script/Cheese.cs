using UnityEngine;

public class Cheese : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private KeyCode pickupKey = KeyCode.E;
    private bool canInteract = false;
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = true;
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
        if (canInteract && Input.GetKeyDown(pickupKey))
        {
            PickupCheese();
        }
    }

    private void PickupCheese()
    {
        if (player != null)
        {
            Inventory inventory = player.GetComponent<Inventory>();
            if (inventory != null)
            {
                inventory.AddCheese();
                Debug.Log("Picked up cheese!");
                if (SoundManager.Instance != null)
                    SoundManager.Instance.PlaySFX("Cheese");
                gameObject.SetActive(false); // Hide the cheese
            }
        }
    }

    public void OnGUI()
    {
        if (canInteract && !gameObject.activeSelf == false)
        {
            GUI.Label(new Rect(10, 10, 200, 30), "Press E to pick up cheese");
        }
    }
}
