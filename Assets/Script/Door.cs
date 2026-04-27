using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Door : MonoBehaviour
{
    [SerializeField] public string nextSceneName;
    [SerializeField] public string doorID;
    [SerializeField] private string targetDoorIDInNextScene;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Vector2 fallbackSpawnOffset = new Vector2(0f, -1.2f);

    [SerializeField] public bool requireKey = true;
    [SerializeField] private string requiredKeyID;

    [SerializeField] private bool requireInteractKey = true;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private TMP_Text interactPromptText;
    [SerializeField] private string interactPromptMessage = "Press E";
    [SerializeField] private string lockedPromptMessage = "Door locked, need key...";
    [SerializeField] private float lockedPromptDuration = 1.5f;

    private bool playerInRange;
    private Coroutine lockedPromptRoutine;

    private void Start()
    {
        SetPromptVisible(false);
    }

    private void Update()
    {
        if (!requireInteractKey || !playerInRange) return;

        if (Input.GetKeyDown(interactKey))
        {
            TryOpenDoor();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        SetPromptVisible(requireInteractKey);

        if (!requireInteractKey)
        {
            TryOpenDoor();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        SetPromptVisible(false);
    }

    private void TryOpenDoor()
    {
        SetPromptVisible(false);

        if (GameManager.Instance != null && GameManager.Instance.IsDoorTransitionBlocked()) return;

        string targetDoorID = string.IsNullOrEmpty(targetDoorIDInNextScene) ? doorID : targetDoorIDInNextScene;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PrepareSpawnAtDoor(targetDoorID);
        }

        // ❌ ไม่ใช้กุญแจ
        if (!requireKey)
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlaySFX("OpenDoor");
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        // ✅ เคยปลดล็อกแล้ว
        if (GameManager.Instance.IsDoorUnlocked(doorID))
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlaySFX("OpenDoor");
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        // 🔑 เช็คกุญแจ "เฉพาะดอก"
        if (GameManager.Instance.HasKey(requiredKeyID))
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlaySFX("OpenDoor");
            GameManager.Instance.UnlockDoor(doorID);
            Debug.Log("ปลดล็อกประตูแล้ว!");

            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.Log("ต้องใช้กุญแจ: " + requiredKeyID);
            ShowLockedPrompt();
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlaySFX("LockDoor");
        }
    }

    public Vector3 GetSpawnPosition()
    {

        if (spawnPoint != null)
            return spawnPoint.position;

        return transform.position + (Vector3)fallbackSpawnOffset;

      
    }

    private void SetPromptVisible(bool visible)
    {
        if (interactPromptText == null) return;

        interactPromptText.text = interactPromptMessage;
        interactPromptText.gameObject.SetActive(visible);
    }

    private void ShowLockedPrompt()
    {
        if (interactPromptText == null) return;

        if (lockedPromptRoutine != null)
        {
            StopCoroutine(lockedPromptRoutine);
        }

        lockedPromptRoutine = StartCoroutine(ShowLockedPromptRoutine());
    }

    private IEnumerator ShowLockedPromptRoutine()
    {
        interactPromptText.text = lockedPromptMessage;
        interactPromptText.gameObject.SetActive(true);

        yield return new WaitForSeconds(lockedPromptDuration);

        if (playerInRange && requireInteractKey)
        {
            interactPromptText.text = interactPromptMessage;
            interactPromptText.gameObject.SetActive(true);
        }
        else
        {
            interactPromptText.gameObject.SetActive(false);
        }

        lockedPromptRoutine = null;

       
    }
}