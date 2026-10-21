using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuBuilder : MonoBehaviour
{
    [Header("Scene names (must match your scene files)")]
    public string[] levelScenes = { "Level 1", "Level 2", "Level 3" };
    public string[] levelLabels = { "Level 1", "Level 2", "Level 3" };

    [Header("Text (edit in the Inspector)")]
    public string gameTitle = "APPLIED MATH EXAM";
    public string developerName = "YOUR NAME HERE";
    public string credits = "Environment assets: Mid Moon Studio";
    [TextArea(3, 6)]
    public string overview =
        "A multi-level 3D game that applies vectors, coordinate systems and linear " +
        "transformations (translation, rotation, scaling) to character control, " +
        "patrolling hazards and aimed projectiles.";

    GameObject mainPanel, levelPanel, aboutPanel;

    static readonly Color BgColor = new Color(0.04f, 0.06f, 0.12f);

    void Start()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (Camera.main != null)
        {
            Camera.main.clearFlags = CameraClearFlags.SolidColor;
            Camera.main.backgroundColor = BgColor;
        }

        UIKit.EnsureEventSystem();

        Canvas canvas = UIKit.NewCanvas("MenuCanvas", 0);
        var bg = UIKit.NewRect("Background", canvas.transform);
        UIKit.Stretch(bg);
        bg.gameObject.AddComponent<Image>().color = BgColor;

        mainPanel  = BuildMain(canvas.transform);
        levelPanel = BuildLevelSelect(canvas.transform);
        aboutPanel = BuildAbout(canvas.transform);
        Show(mainPanel);
    }

    GameObject BuildMain(Transform parent)
    {
        var root = MakeCard("MainPanel", parent, new Vector2(700, 800), out RectTransform card);
        UIKit.MakeText(card, gameTitle, 52, Color.white, 130, TextAlignmentOptions.Center, true);
        UIKit.Divider(card);
        UIKit.MakeButton(card, "Start Game",   () => LoadLevel(levelScenes[0]));
        UIKit.MakeButton(card, "Level Select", () => Show(levelPanel));
        UIKit.MakeButton(card, "About",        () => Show(aboutPanel));
        UIKit.MakeButton(card, "Quit",         Quit, new Color(0.55f, 0.18f, 0.22f));
        return root;
    }

    GameObject BuildLevelSelect(Transform parent)
    {
        var root = MakeCard("LevelPanel", parent, new Vector2(700, 700), out RectTransform card);
        UIKit.MakeText(card, "SELECT LEVEL", 52, Color.white, 110, TextAlignmentOptions.Center, true);
        UIKit.Divider(card);
        for (int i = 0; i < levelScenes.Length; i++)
        {
            int index = i;
            string label = i < levelLabels.Length ? levelLabels[i] : levelScenes[i];
            UIKit.MakeButton(card, label, () => LoadLevel(levelScenes[index]));
        }
        UIKit.MakeButton(card, "Back", () => Show(mainPanel), new Color(0.30f, 0.30f, 0.38f), 70);
        return root;
    }

    GameObject BuildAbout(Transform parent)
    {
        string hex = ColorUtility.ToHtmlStringRGB(UIKit.Accent);
        string controls =
            $"<color=#{hex}><b>W A S D</b></color>   Move\n" +
            $"<color=#{hex}><b>X</b></color>   Rotate character\n" +
            $"<color=#{hex}><b>Y</b></color>   Scale character (press again to reverse)\n" +
            $"<color=#{hex}><b>K</b></color>   Toggle speed boost\n" +
            $"<color=#{hex}><b>Esc</b></color>   Back to main menu";

        var root = MakeCard("AboutPanel", parent, new Vector2(1100, 940), out RectTransform card);
        UIKit.MakeText(card, "ABOUT", 52, Color.white, 90, TextAlignmentOptions.Center, true);
        UIKit.Divider(card);
        UIKit.MakeText(card, $"Developer: {developerName}\n{credits}", 28, Color.white, 100);
        UIKit.MakeText(card, overview, 26, UIKit.DimText, 130);
        UIKit.MakeText(card, "CONTROLS", 34, UIKit.Accent, 50, TextAlignmentOptions.Center, true);
        UIKit.MakeText(card, controls, 30, Color.white, 220);
        UIKit.MakeButton(card, "Back", () => Show(mainPanel), new Color(0.30f, 0.30f, 0.38f), 70);
        return root;
    }

    void Show(GameObject panel)
    {
        mainPanel.SetActive(panel == mainPanel);
        levelPanel.SetActive(panel == levelPanel);
        aboutPanel.SetActive(panel == aboutPanel);
    }

    void LoadLevel(string sceneName)
    {
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"Scene '{sceneName}' can't be loaded. Add it in Build Profiles > Scene List.");
            return;
        }
        SceneManager.LoadScene(sceneName);
    }

    void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    GameObject MakeCard(string name, Transform parent, Vector2 size, out RectTransform card)
    {
        var root = UIKit.NewRect(name, parent);
        UIKit.Stretch(root);

        card = UIKit.NewRect("Card", root);
        card.anchorMin = card.anchorMax = card.pivot = new Vector2(0.5f, 0.5f);
        card.sizeDelta = size;
        card.anchoredPosition = Vector2.zero;

        var img = card.gameObject.AddComponent<Image>();
        img.sprite = UIKit.Round;
        img.type = Image.Type.Sliced;
        img.color = UIKit.CardColor;

        var v = card.gameObject.AddComponent<VerticalLayoutGroup>();
        v.padding = new RectOffset(50, 50, 45, 45);
        v.spacing = 18;
        v.childAlignment = TextAnchor.UpperCenter;
        v.childControlWidth = true;
        v.childControlHeight = true;
        v.childForceExpandWidth = true;
        v.childForceExpandHeight = false;
        return root.gameObject;
    }
}