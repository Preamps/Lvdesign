using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] private string dialogueID = "dialogue_1";
    [SerializeField] private string dialogueText = "Dialogue";
    [SerializeField] private TextMeshProUGUI dialogueUIText; // Assign in inspector
    [SerializeField] private float displayDuration = 3f; // How long to show text
    [SerializeField] private float typewriterDelay = 0.05f; // Delay between characters

    private float timeLeft = 0f;
    private bool isDisplaying = false;
    private bool isTypewriting = false;
    private bool hasTriggered = false;
    private Coroutine typewriterCoroutine;

    private void Update()
    {
        if (isDisplaying && !isTypewriting)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0f)
            {
                HideDialogue();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered)
            return;

        if (GameManager.Instance != null && GameManager.Instance.HasSeenDialogue(dialogueID))
            return;

        Player player = collision.GetComponent<Player>();
        if (player == null)
            player = collision.GetComponentInParent<Player>();

        if (player != null)
        {
            hasTriggered = true;
            Collider2D triggerCollider = GetComponent<Collider2D>();
            if (triggerCollider != null)
                triggerCollider.enabled = false;

            if (GameManager.Instance != null)
                GameManager.Instance.MarkDialogueSeen(dialogueID);

            ShowDialogue();
        }
    }

    private void ShowDialogue()
    {
        if (dialogueUIText == null)
        {
            Debug.LogWarning("DialogueDisplay: No UI Text assigned!");
            return;
        }

        dialogueUIText.gameObject.SetActive(true);
        isDisplaying = true;

        // Stop any existing typewriter coroutine
        if (typewriterCoroutine != null)
            StopCoroutine(typewriterCoroutine);

        // Start typewriter effect
        typewriterCoroutine = StartCoroutine(TypewriterEffect());
    }

    private IEnumerator TypewriterEffect()
    {
        isTypewriting = true;
        dialogueUIText.text = "";

        foreach (char character in dialogueText)
        {
            dialogueUIText.text += character;
            yield return new WaitForSeconds(typewriterDelay);
        }

        isTypewriting = false;
        // Start timer after text is fully revealed
        timeLeft = displayDuration;
    }

    private void HideDialogue()
    {
        if (dialogueUIText != null)
        {
            dialogueUIText.gameObject.SetActive(false);
        }

        // Stop typewriter coroutine if running
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
            typewriterCoroutine = null;
        }

        isDisplaying = false;
        Destroy(gameObject);
    }
}
