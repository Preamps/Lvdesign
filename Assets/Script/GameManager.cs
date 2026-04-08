using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool hasKey = false;

    // เก็บสถานะประตู (แต่ละบาน)
    private HashSet<string> unlockedDoors = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ✅ ข้าม scene แล้วไม่หาย
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 🔑 KEY
    public void AddKey()
    {
        hasKey = true;
        Debug.Log("เก็บกุญแจแล้ว!");
    }

    public bool HasKey()
    {
        return hasKey;
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
}