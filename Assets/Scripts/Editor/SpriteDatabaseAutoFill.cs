using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class SpriteDatabaseAutoFill
{
    [MenuItem("Tools/Sprite DB/Fill From Current Folder")]
    private static void FillFromCurrentFolder()
    {
        var db = Selection.activeObject as SpriteDatabase;
        if (db == null)
        {
            EditorUtility.DisplayDialog(
                "Sprite DB",
                "Выберите SpriteDatabase в Project перед запуском.",
                "OK");
            return;
        }

        string folderPath = GetCurrentProjectFolder();
        if (string.IsNullOrEmpty(folderPath))
        {
            EditorUtility.DisplayDialog(
                "Sprite DB",
                "Не удалось определить открытую папку в Project.",
                "OK");
            return;
        }

        var sprites = CollectSpritesFromFolder(folderPath);
        WriteToDatabase(db, sprites);

        EditorUtility.SetDirty(db);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"SpriteDatabase заполнена. Найдено спрайтов: {sprites.Count}");
    }

    private static string GetCurrentProjectFolder()
    {
        // Берём путь текущей открытой папки в Project
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (File.Exists(path))
            return Path.GetDirectoryName(path);

        if (Directory.Exists(path))
            return path;

        return "Assets";
    }

    private static List<(string id, Sprite sprite)> CollectSpritesFromFolder(string folder)
    {
        var result = new List<(string, Sprite)>();

        var guids = AssetDatabase.FindAssets("t:Texture2D", new[] { folder });

        foreach (var guid in guids)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);

            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null || importer.textureType != TextureImporterType.Sprite)
                continue;

            // Важно: поддержка multiple sprites в одном файле (Sprite Mode: Multiple)
            var assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
            foreach (var a in assets)
            {
                if (a is Sprite sprite)
                {
                    string id = Path.GetFileNameWithoutExtension(assetPath);

                    // Если multiple — добавляем имя сабспрайта
                    if (assets.Length > 1)
                        id = sprite.name;

                    result.Add((id, sprite));
                }
            }
        }

        return result;
    }

    private static void WriteToDatabase(SpriteDatabase db, List<(string id, Sprite sprite)> data)
    {
        var so = new SerializedObject(db);
        var arrayProp = so.FindProperty("sprites");

        arrayProp.ClearArray();
        arrayProp.arraySize = data.Count;

        for (int i = 0; i < data.Count; i++)
        {
            var element = arrayProp.GetArrayElementAtIndex(i);
            element.FindPropertyRelative("id").stringValue = data[i].id;
            element.FindPropertyRelative("sprite").objectReferenceValue = data[i].sprite;
        }

        so.ApplyModifiedProperties();
    }
}
