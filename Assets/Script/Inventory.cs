using UnityEngine;

public class Inventory : MonoBehaviour
{
    private bool hasKey = false;

    public void AddKey()
    {
        hasKey = true;
        Debug.Log("เก็บกุญแจแล้ว!");
    }

    public bool HasKey()
    {
        return hasKey;
    }
}