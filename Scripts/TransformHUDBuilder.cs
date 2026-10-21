using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class TransformHUDBuilder : MonoBehaviour
{
    public PlayerController player;
    public string menuScene = "Main Menu";

    TextMeshProUGUI label;

    void Start()
    {
        Canvas canvas = UIKit.NewCanvas("HUDCanvas", 10);

        var panel = UIKit.NewRect("HUD Panel", canvas.transform);
        panel.anchorMin = panel.anchorMax = panel.pivot = new Vector2(0, 1);
        panel.anchoredPosition = new Vector2(30, -30);
        panel.sizeDelta = new Vector2(520, 210);
        var img = panel.gameObject.AddComponent<Image>();
        img.sprite = UIKit.Round;
        img.type = Image.Type.Sliced;
        img.color = new Color(0.04f, 0.06f, 0.12f, 0.72f);
        img.raycastTarget = false;

        label = UIKit.MakeText(panel, "", 26, Color.white, -1f, TextAlignmentOptions.TopLeft);
        UIKit.Stretch(label.rectTransform, 22);

        var hint = UIKit.MakeText(canvas.transform, "Esc: Main Menu", 22, UIKit.DimText, -1f,
                                  TextAlignmentOptions.BottomRight);
        hint.rectTransform.anchorMin = hint.rectTransform.anchorMax = hint.rectTransform.pivot = new Vector2(1, 0);
        hint.rectTransform.anchoredPosition = new Vector2(-30, 30);
        hint.rectTransform.sizeDelta = new Vector2(400, 40);
    }

    void Update()
    {
        if (EscPressed())
        {
            SceneManager.LoadScene(menuScene);
            return;
        }

        if (player == null)
        {
            player = FindFirstObjectByType<PlayerController>();
            if (player == null) return;
        }

        Vector3 s = player.transform.localScale;
        Vector3 r = player.transform.eulerAngles;
        float multiplier = player.CurrentSpeed / player.baseSpeed;
        string mode = player.Boosted
            ? "<color=#FFB84D>BOOST</color>"
            : "<color=#7CFFB2>NORMAL</color>";
        string hex = ColorUtility.ToHtmlStringRGB(UIKit.Accent);

        label.text =
            $"<color=#{hex}><b>TRANSFORM HUD</b></color>\n" +
            $"Scale     X {s.x:F2}   Y {s.y:F2}   Z {s.z:F2}\n" +
            $"Rotation  X {r.x:F0}   Y {r.y:F0}   Z {r.z:F0}\n" +
            $"Speed     x{multiplier:F1}   ({player.CurrentSpeed:F1} units/s)\n" +
            $"Mode      {mode}   [K]";
    }

    bool EscPressed()
    {
#if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.Escape);
#endif
    }
}