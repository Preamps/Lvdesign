using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public RectTransform crosshair;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        if (crosshair == null) return;

        crosshair.position = Input.mousePosition;
    }

    // 🔥 เปิด/ปิด crosshair
    public void SetActive(bool isActive)
    {
        crosshair.gameObject.SetActive(isActive);
    }
}
