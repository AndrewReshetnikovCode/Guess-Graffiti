using System.Collections.Generic;
using UnityEngine;

public class SpriteProgressionManager : MonoBehaviour
{
    [SerializeField] private SpriteDatabase spriteDatabase;

    // difficulty -> список спрайтов
    private Dictionary<int, List<NamedSprite>> spritesByDifficulty;
    private List<int> orderedDifficulties;

    private int currentDifficultyIndex;
    private int currentSpriteIndex;

    private NamedSprite? current;
    private bool finished;

    private void Awake()
    {
        BuildDatabase();
    }

    private void BuildDatabase()
    {
        spritesByDifficulty = new Dictionary<int, List<NamedSprite>>();

        foreach (var entry in spriteDatabase.sprites)
        {
            if (!spritesByDifficulty.TryGetValue(entry.difficulty, out var list))
            {
                list = new List<NamedSprite>();
                spritesByDifficulty.Add(entry.difficulty, list);
            }

            list.Add(entry);
        }

        orderedDifficulties = new List<int>(spritesByDifficulty.Keys);
        orderedDifficulties.Sort();

        currentDifficultyIndex = 0;
        currentSpriteIndex = 0;
        current = null;
        finished = orderedDifficulties.Count == 0;
    }

    public NamedSprite? Next()
    {
        if (finished)
            return null;

        // Если текущая сложность закончилась — переходим дальше
        while (currentDifficultyIndex < orderedDifficulties.Count)
        {
            var difficulty = orderedDifficulties[currentDifficultyIndex];
            var list = spritesByDifficulty[difficulty];

            if (currentSpriteIndex < list.Count)
            {
                current = list[currentSpriteIndex];
                currentSpriteIndex++;
                return current;
            }

            // Переход на следующую сложность
            currentDifficultyIndex++;
            currentSpriteIndex = 0;
        }

        finished = true;
        current = null;
        return null;
    }

    public NamedSprite? GetCurrent()
    {
        return current;
    }

    public int GetCurrentDifficulty()
    {
        if (finished) return -1;
        return orderedDifficulties[currentDifficultyIndex];
    }

    public int GetSpritesCountOnCurrentDifficulty()
    {
        if (finished) return 0;

        var difficulty = orderedDifficulties[currentDifficultyIndex];
        return spritesByDifficulty[difficulty].Count;
    }

    public int GetCurrentSpriteNumberOnDifficulty()
    {
        if (finished) return 0;

        return currentSpriteIndex; // порядковый номер (1-based логически)
    }
}
