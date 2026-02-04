using TMPro;
using UnityEngine;

public class DifficultyProgressTMP : MonoBehaviour
{
    [SerializeField] private SpriteProgressionManager progressionManager;
    [SerializeField] private DifficultyDatabaseSO difficultyDatabase;
    [SerializeField] private TMP_Text label;

    private void Update()
    {
        Refresh();
    }

    public void Refresh()
    {
        int difficulty = progressionManager.GetCurrentDifficulty();

        if (difficulty < 0)
        {
            label.text = "";
            return;
        }

        var entry = GetDifficultyEntry(difficulty);

        int current = progressionManager.GetCurrentSpriteNumberOnDifficulty();
        int total = progressionManager.GetSpritesCountOnCurrentDifficulty();

        label.text = $"Уровень:\n{entry.difficultyName}\n{current-1}/{total}";
        label.color = entry.color;
    }

    private DifficultyEntry GetDifficultyEntry(int index)
    {
        foreach (var d in difficultyDatabase.difficulties)
        {
            if (d.difficultyIndex == index)
                return d;
        }

        return null;
    }
}
