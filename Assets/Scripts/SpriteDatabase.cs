using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteDatabase", menuName = "Database/Sprite Database")]
public class SpriteDatabase : ScriptableObject
{
    public NamedSprite[] sprites;

    private Dictionary<string, Sprite> lookup;

    private void OnEnable()
    {
        BuildLookup();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        BuildLookup();
    }
#endif

    private void BuildLookup()
    {
        lookup = new Dictionary<string, Sprite>(sprites.Length);
        foreach (var entry in sprites)
        {
            if (string.IsNullOrWhiteSpace(entry.id) || entry.sprite == null)
                continue;

            lookup[entry.id] = entry.sprite;
        }
    }

    public Sprite Get(string id)
    {
        if (lookup == null)
            BuildLookup();

        return lookup.TryGetValue(id, out var sprite) ? sprite : null;
    }

    public bool TryGet(string id, out Sprite sprite)
    {
        if (lookup == null)
            BuildLookup();

        return lookup.TryGetValue(id, out sprite);
    }
}
