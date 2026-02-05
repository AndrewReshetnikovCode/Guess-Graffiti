using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionPanelBuilder : MonoBehaviour
{
    [Header("Database")]
    [SerializeField] private SpriteDatabase spriteDatabase;
    [SerializeField] private DifficultyDatabaseSO difficultyDatabase;
    [SerializeField] private LevelProgressionSO levelProgression;

    [Header("Prefabs")]
    [SerializeField] private LevelGroupContainer groupContainerPrefab;
    [SerializeField] private SpriteMenuSelection levelItemPrefab;

    [Header("Layout")]
    [SerializeField] private Transform groupParent;

    public void Build()
    {
        if (groupParent == null)
            return;

        ClearChildren(groupParent);

        var difficulties = new List<DifficultyEntry>(difficultyDatabase.difficulties);
        difficulties.Sort((a, b) => a.difficultyIndex.CompareTo(b.difficultyIndex));

        foreach (var difficulty in difficulties)
        {
            var groupContainer = Instantiate(groupContainerPrefab, groupParent);
            if (groupContainer == null || groupContainer.LevelsRoot == null)
                continue;

            var sprites = GetSpritesByDifficulty(difficulty.difficultyIndex);

            int solvedCount = 0;
            for (int i = 0; i < sprites.Count; i++)
            {
                var entry = sprites[i];
                var item = Instantiate(levelItemPrefab, groupContainer.LevelsRoot);
                item.SetMainSprite(entry.sprite);

                if (levelProgression != null && levelProgression.IsSpriteSolved(difficulty.difficultyIndex, entry.id))
                {
                    solvedCount++;
                    item.SetOverlayVisible(true);
                }
                else
                {
                    item.SetOverlayVisible(false);
                }
            }

            int total = sprites.Count;
            int needed = difficulty.minSpritesToUnlockNext;
            if (groupContainer != null)
                groupContainer.SetProgressText($"{Mathf.Min(solvedCount, total)}/{needed}");
        }
    }

    private List<NamedSprite> GetSpritesByDifficulty(int difficultyIndex)
    {
        var result = new List<NamedSprite>();
        if (spriteDatabase == null || spriteDatabase.sprites == null)
            return result;

        foreach (var entry in spriteDatabase.sprites)
        {
            if (entry.difficulty == difficultyIndex)
                result.Add(entry);
        }

        return result;
    }

    private void ClearChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            var child = parent.GetChild(i);
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }
    }
}
