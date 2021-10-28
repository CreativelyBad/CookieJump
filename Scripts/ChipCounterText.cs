using UnityEngine;
using TMPro;

public class ChipCounterText : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;

    public void ChipCountText(string count)
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        textMeshPro.SetText(count);
    }
}
