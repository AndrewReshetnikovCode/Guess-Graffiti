using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelProgression", menuName = "Game/Level Progression")]
public class LevelProgressionSO : ScriptableObject
{
    [SerializeField] private List<DifficultyProgress> difficulties = new();

    public IReadOnlyList<DifficultyProgress> Difficulties => difficulties;

    public bool IsDifficultyUnlocked(int difficultyIndex)
    {
        var progress = GetOrCreateProgress(difficultyIndex);
        return progress.unlocked;
    }

    public void SetDifficultyUnlocked(int difficultyIndex, bool unlocked)
    {
        var progress = GetOrCreateProgress(difficultyIndex);
        progress.unlocked = unlocked;
    }

    public bool IsSpriteSolved(int difficultyIndex, string spriteId)
    {
        var progress = GetOrCreateProgress(difficultyIndex);
        return progress.solvedSpriteIds.Contains(spriteId);
    }

    public void SetSpriteSolved(int difficultyIndex, string spriteId, bool solved)
    {
        var progress = GetOrCreateProgress(difficultyIndex);
        if (solved)
        {
            if (!progress.solvedSpriteIds.Contains(spriteId))
                progress.solvedSpriteIds.Add(spriteId);
        }
        else
        {
            progress.solvedSpriteIds.Remove(spriteId);
        }
    }

    public int GetSolvedCount(int difficultyIndex)
    {
        var progress = GetOrCreateProgress(difficultyIndex);
        return progress.solvedSpriteIds.Count;
    }

    private DifficultyProgress GetOrCreateProgress(int difficultyIndex)
    {
        for (int i = 0; i < difficulties.Count; i++)
        {
            if (difficulties[i].difficultyIndex == difficultyIndex)
                return difficulties[i];
        }

        var created = new DifficultyProgress
        {
            difficultyIndex = difficultyIndex,
            unlocked = false,
            solvedSpriteIds = new List<string>()
        };
        difficulties.Add(created);
        return created;
    }
}

[Serializable]
public class DifficultyProgress
{
    public int difficultyIndex;
    public bool unlocked;
    public List<string> solvedSpriteIds = new();
}
