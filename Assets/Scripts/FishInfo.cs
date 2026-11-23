using TMPro;
using UnityEngine;

public class FishInfo : MonoBehaviour
{
    public TMP_Text infoText;
    public string displayName = "Reef Fish";

    [TextArea(3, 8)]
    public string description =
        "This is a placeholder fish for testing our aquarium interactions.";

    public void ShowInfo()
    {
        if (infoText == null)
        {
            Debug.LogWarning("Assign the information text.", this);
            return;
        }

        infoText.text = displayName + "\n\n" + description;
    }
}