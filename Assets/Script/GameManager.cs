using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private HashSet<string> keys = new HashSet<string>(); // 🔑 หลายดอก
    private HashSet<string> unlockedDoors = new HashSet<string>();

    private bool hasGun = false; // 🔫 เพิ่มตรงนี้

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 🔑 KEY
    public void AddKey(string keyID)
    {
        keys.Add(keyID);
        Debug.Log("เก็บกุญแจ: " + keyID);
    }

    public bool HasKey(string keyID)
    {
        return keys.Contains(keyID);
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