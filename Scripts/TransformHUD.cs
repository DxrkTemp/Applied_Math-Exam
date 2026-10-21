using UnityEngine;
using TMPro;

public class TransformHUD : MonoBehaviour
{
    public PlayerController player;
    public TMP_Text label;

    void Update()
    {
        Vector3 s = player.transform.localScale;
        Vector3 r = player.transform.eulerAngles;
        label.text =
            $"Scale:    X {s.x:F2}  Y {s.y:F2}  Z {s.z:F2}\n" +
            $"Rotation: X {r.x:F0}°  Y {r.y:F0}°  Z {r.z:F0}°\n" +
            $"Speed:    {player.CurrentSpeed:F1} ({(player.Boosted ? "BOOST" : "NORMAL")})";
    }
}