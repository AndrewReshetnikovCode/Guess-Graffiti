using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Difficulty Database")]
public class DifficultyDatabaseSO : ScriptableObject
{
    public List<DifficultyEntry> difficulties = new();

}
