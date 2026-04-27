using UnityEngine;

[DisallowMultipleComponent]
public class PlayerDarknessLight : MonoBehaviour
{
    [Header("Darkness")]
    [Range(0f, 1f)] public float darknessAlpha = 0.92f;
    [Range(0.01f, 0.9f)] public float lightRadius = 0.22f;
    [Range(0.01f, 0.9f)] public float edgeSoftness = 0.18f;

    [Header("Overlay Size")]
    [Min(8f)] public float overlayDiameterWorld = 40f;
    [Min(64)] public int textureSize = 512;

    [Header("Sorting")]
    public string sortingLayerName = "Default";
    public int sortingOrder = 5000;

    private GameObject overlayObject;
    private SpriteRenderer overlayRenderer;

    private void Awake()
    {
        CreateOrRefreshOverlay();
    }

    private void LateUpdate()
    {
        if (overlayObject == null)
            return;

        overlayObject.transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    private void OnValidate()
    {
        lightRadius = Mathf.Clamp(lightRadius, 0.01f, 0.9f);
        edgeSoftness = Mathf.Clamp(edgeSoftness, 0.01f, 0.9f);
        textureSize = Mathf.Max(64, textureSize);
        overlayDiameterWorld = Mathf.Max(8f, overlayDiameterWorld);
        darknessAlpha = Mathf.Clamp01(darknessAlpha);

        if (!Application.isPlaying)
            return;

        CreateOrRefreshOverlay();
    }

    private void CreateOrRefreshOverlay()
    {
        if (overlayObject == null)
        {
            Transform existing = transform.Find("PlayerDarknessOverlay");
            if (existing != null)
                overlayObject = existing.gameObject;
            else
            {
                overlayObject = new GameObject("PlayerDarknessOverlay");
                overlayObject.transform.SetParent(transform, false);
            }

            overlayRenderer = overlayObject.GetComponent<SpriteRenderer>();
            if (overlayRenderer == null)
                overlayRenderer = overlayObject.AddComponent<SpriteRenderer>();
        }

        Texture2D texture = GenerateRadialDarknessTexture();
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), texture.width);

        overlayRenderer.sprite = sprite;
        overlayRenderer.sortingLayerName = sortingLayerName;
        overlayRenderer.sortingOrder = sortingOrder;

        overlayObject.transform.localScale = new Vector3(overlayDiameterWorld, overlayDiameterWorld, 1f);
        overlayObject.transform.localPosition = Vector3.zero;
    }

    private Texture2D GenerateRadialDarknessTexture()
    {
        Texture2D texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Bilinear;

        float half = (textureSize - 1) * 0.5f;
        float inner = lightRadius;
        float outer = Mathf.Clamp01(lightRadius + edgeSoftness);

        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                float dx = (x - half) / half;
                float dy = (y - half) / half;
                float distance = Mathf.Sqrt(dx * dx + dy * dy);

                float t = Mathf.InverseLerp(inner, outer, distance);
                float alpha = Mathf.Lerp(0f, darknessAlpha, t);
                texture.SetPixel(x, y, new Color(0f, 0f, 0f, alpha));
            }
        }

        texture.Apply(false, false);
        return texture;
    }
}
