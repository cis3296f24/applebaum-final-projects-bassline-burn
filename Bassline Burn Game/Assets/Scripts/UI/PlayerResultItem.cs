using TMPro;
using UnityEngine;

public class PlayerResultItem : MonoBehaviour
{
    public TextMeshProUGUI placementText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI timeText;

    public void SetResult(string name, float time, int place)
    {
        placementText.text = place == 1 ? "1st" :
                             place == 2 ? "2nd" :
                             place == 3 ? "3rd" :
                             $"{place}th";
        nameText.text = name;
        timeText.text = $"{time:F2}s";
    }
}
