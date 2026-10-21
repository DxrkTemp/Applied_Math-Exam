using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif

public static class UIKit
{
    public static readonly Color Accent      = new Color(0.25f, 0.85f, 0.75f);
    public static readonly Color CardColor   = new Color(0.10f, 0.13f, 0.22f, 0.96f);
    public static readonly Color ButtonColor = new Color(0.17f, 0.22f, 0.36f);
    public static readonly Color DimText     = new Color(0.72f, 0.79f, 0.90f);

    static Sprite _round;
    public static Sprite Round
    {
        get { if (_round == null) _round = MakeRounded(64, 24); return _round; }
    }

    static Sprite MakeRounded(int size, int r)
    {
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                float dx = Mathf.Max(r - x - 0.5f, 0f, x + 0.5f - (size - r));
                float dy = Mathf.Max(r - y - 0.5f, 0f, y + 0.5f - (size - r));
                float a = Mathf.Clamp01(r - Mathf.Sqrt(dx * dx + dy * dy) + 0.5f);
                tex.SetPixel(x, y, new Color(1, 1, 1, a));
            }
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f),
                             100f, 0, SpriteMeshType.FullRect, new Vector4(r, r, r, r));
    }

    public static RectTransform NewRect(string name, Transform parent)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        return (RectTransform)go.transform;
    }

    public static void Stretch(RectTransform rt, float pad = 0f)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(pad, pad);
        rt.offsetMax = new Vector2(-pad, -pad);
    }

    public static Canvas NewCanvas(string name, int sortingOrder)
    {
        var go = new GameObject(name);
        var canvas = go.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = sortingOrder;
        var scaler = go.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;
        go.AddComponent<GraphicRaycaster>();
        return canvas;
    }

    public static TextMeshProUGUI MakeText(Transform parent, string text, float size, Color color,
        float height = -1f, TextAlignmentOptions align = TextAlignmentOptions.Center, bool bold = false)
    {
        var rt = NewRect("Text", parent);
        var t = rt.gameObject.AddComponent<TextMeshProUGUI>();
        t.text = text;
        t.fontSize = size;
        t.color = color;
        t.alignment = align;
        t.raycastTarget = false;
        if (bold) t.fontStyle = FontStyles.Bold;
        if (height > 0f)
        {
            var le = rt.gameObject.AddComponent<LayoutElement>();
            le.preferredHeight = height;
        }
        return t;
    }

    public static void Divider(Transform parent)
    {
        var rt = NewRect("Divider", parent);
        var img = rt.gameObject.AddComponent<Image>();
        img.color = Accent;
        img.raycastTarget = false;
        var le = rt.gameObject.AddComponent<LayoutElement>();
        le.preferredHeight = 4f;
    }

    public static Button MakeButton(Transform parent, string label, UnityAction onClick,
                                    Color? color = null, float height = 80f)
    {
        Color baseColor = color ?? ButtonColor;

        var rt = NewRect(label + " Button", parent);
        var img = rt.gameObject.AddComponent<Image>();
        img.sprite = Round;
        img.type = Image.Type.Sliced;

        var btn = rt.gameObject.AddComponent<Button>();
        btn.targetGraphic = img;
        var cb = btn.colors;
        cb.normalColor      = baseColor;
        cb.highlightedColor = Color.Lerp(baseColor, Color.white, 0.25f);
        cb.pressedColor     = Color.Lerp(baseColor, Color.black, 0.30f);
        cb.selectedColor    = baseColor;
        cb.fadeDuration     = 0.08f;
        btn.colors = cb;
        btn.onClick.AddListener(onClick);

        var le = rt.gameObject.AddComponent<LayoutElement>();
        le.preferredHeight = height;

        var t = MakeText(rt, label, 34f, Color.white, -1f, TextAlignmentOptions.Center, true);
        Stretch(t.rectTransform);
        return btn;
    }

    public static void EnsureEventSystem()
    {
        if (Object.FindFirstObjectByType<EventSystem>() != null) return;
        var go = new GameObject("EventSystem");
        go.AddComponent<EventSystem>();
#if ENABLE_INPUT_SYSTEM
        var module = go.AddComponent<InputSystemUIInputModule>();
        module.AssignDefaultActions();
#else
        go.AddComponent<StandaloneInputModule>();
#endif
    }
}