using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private const string KeysPrefsKey = "PlayerKeys";
    private const string SeenDialoguePrefsKey = "SeenDialogues";

    private HashSet<string> keys = new HashSet<string>(); // 🔑 หลายดอก
    private HashSet<string> seenDialogues = new HashSet<string>();
    private HashSet<string> unlockedDoors = new HashSet<string>();

    private bool hasGun = false; // 🔫 เพิ่มตรงนี้
    [SerializeField] private int defaultPlayerHealth = 100;
    private int savedPlayerHealth = -1;

    private string pendingSpawnDoorID;
    private float blockDoorTriggerUntil;
    
    public int currentFuses = 0; //ขันโตกเพิ่มมาระบบเก็บ Fuse

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSavedKeys();
            LoadSeenDialogues();
            SceneManager.sceneLoaded += OnSceneLoaded;
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // 🔑 KEY
    public void AddKey(string keyID)
    {
        keys.Add(keyID);
        SaveKeys();
        Debug.Log("เก็บกุญแจ: " + keyID);
    }

    public bool HasKey(string keyID)
    {
        return keys.Contains(keyID);
    }

    public void SaveKeys()
    {
        string[] allKeys = new string[keys.Count];
        keys.CopyTo(allKeys);

        string serializedKeys = string.Join("|", allKeys);
        PlayerPrefs.SetString(KeysPrefsKey, serializedKeys);
        PlayerPrefs.Save();
    }

    public void ClearKeys()
    {
        keys.Clear();
        PlayerPrefs.DeleteKey(KeysPrefsKey);
        PlayerPrefs.Save();
    }

    public void ClearSeenDialogues()
    {
        seenDialogues.Clear();
        PlayerPrefs.DeleteKey(SeenDialoguePrefsKey);
        PlayerPrefs.Save();
    }

    // 💬 DIALOGUE
    public void MarkDialogueSeen(string dialogueID)
    {
        if (string.IsNullOrEmpty(dialogueID)) return;

        seenDialogues.Add(dialogueID);
        SaveSeenDialogues();
    }

    public bool HasSeenDialogue(string dialogueID)
    {
        return !string.IsNullOrEmpty(dialogueID) && seenDialogues.Contains(dialogueID);
    }

    private void SaveSeenDialogues()
    {
        string[] allDialogues = new string[seenDialogues.Count];
        seenDialogues.CopyTo(allDialogues);

        string serializedDialogues = string.Join("|", allDialogues);
        PlayerPrefs.SetString(SeenDialoguePrefsKey, serializedDialogues);
        PlayerPrefs.Save();
    }

    private void LoadSeenDialogues()
    {
        if (!PlayerPrefs.HasKey(SeenDialoguePrefsKey)) return;

        string serializedDialogues = PlayerPrefs.GetString(SeenDialoguePrefsKey, string.Empty);
        if (string.IsNullOrEmpty(serializedDialogues)) return;

        string[] loadedDialogues = serializedDialogues.Split('|');
        for (int i = 0; i < loadedDialogues.Length; i++)
        {
            string dialogueID = loadedDialogues[i];
            if (!string.IsNullOrEmpty(dialogueID))
            {
                seenDialogues.Add(dialogueID);
            }
        }
    }

    private void LoadSavedKeys()
    {
        if (!PlayerPrefs.HasKey(KeysPrefsKey)) return;

        string serializedKeys = PlayerPrefs.GetString(KeysPrefsKey, string.Empty);
        if (string.IsNullOrEmpty(serializedKeys)) return;

        string[] loadedKeys = serializedKeys.Split('|');
        for (int i = 0; i < loadedKeys.Length; i++)
        {
            string keyID = loadedKeys[i];
            if (!string.IsNullOrEmpty(keyID))
            {
                keys.Add(keyID);
            }
        }
    }

    // 🔫 GUN
    public void AddGun()
    {
        hasGun = true;
        Debug.Log("เก็บปืนแล้ว!");
    }

    public bool HasGun()
    {
        return hasGun;
    }

    // ❤️ PLAYER HP (persist across scene changes)
    public void SetPlayerHealth(int hp)
    {
        savedPlayerHealth = Mathf.Max(0, hp);
    }

    public int GetPlayerHealthOrDefault()
    {
        return savedPlayerHealth >= 0 ? savedPlayerHealth : defaultPlayerHealth;
    }

    public void ResetPlayerHealthToDefault()
    {
        savedPlayerHealth = defaultPlayerHealth;
    }

    // 🚪 DOOR
    public void UnlockDoor(string doorID)
    {
        unlockedDoors.Add(doorID);
    }

    public bool IsDoorUnlocked(string doorID)
    {
        return unlockedDoors.Contains(doorID);
    }

    public void PrepareSpawnAtDoor(string targetDoorID)
    {
        pendingSpawnDoorID = targetDoorID;
        blockDoorTriggerUntil = Time.unscaledTime + 0.35f;
    }

    public bool IsDoorTransitionBlocked()
    {
        return Time.unscaledTime < blockDoorTriggerUntil;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (string.IsNullOrEmpty(pendingSpawnDoorID)) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            pendingSpawnDoorID = null;
            return;
        }

        Door[] doors = FindObjectsOfType<Door>();
        for (int i = 0; i < doors.Length; i++)
        {
            Door door = doors[i];
            if (door == null || door.doorID != pendingSpawnDoorID) continue;

            Vector3 spawnPos = door.GetSpawnPosition();
            player.transform.position = spawnPos;

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.position = spawnPos;
                rb.velocity = Vector2.zero;
            }

            pendingSpawnDoorID = null;
            return;
        }

        pendingSpawnDoorID = null;
    }
    
    public void AddFuse()
    {
        currentFuses++;
        Debug.Log("Collected Fuse" + currentFuses);
    }
}