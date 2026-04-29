using UnityEngine;

public class Inventory : MonoBehaviour
{
    private bool hasKey = false;
    private bool hasCheese = false;

    public void AddKey()
    {
        hasKey = true;
        Debug.Log("เก็บกุญแจแล้ว!");
    }

    public bool HasKey()
    {
        return hasKey;
    }

    public void AddCheese()
    {
        hasCheese = true;
        Debug.Log("Added cheese to inventory");
    }

    public bool HasCheese()
    {
        return hasCheese;
    }

    public void RemoveCheese()
    {
        hasCheese = false;
        Debug.Log("Removed cheese from inventory");
    }
}