using TMPro;
using UnityEngine;

public class LevelGroupContainer : MonoBehaviour
{
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private Transform levelsRoot;

    public TMP_Text ProgressText => progressText;
    public Transform LevelsRoot => levelsRoot;

    public void SetProgressText(string value)
    {
        if (progressText != null)
            progressText.text = value;
    }
}
