using System.Collections.Generic;
using UnityEngine;

public static class Functions
{
    public static void Rotate2DTowards(Transform transform, Vector3 position)
    {
        Vector3 tp = transform.position;
        Vector3 dir = (position - tp).normalized;
        float z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, z);
    }
    public static void Rotate2DTowards(Transform transform, Vector3 position, Vector3 eulerOffset)
    {
        Vector3 tp = transform.position;
        Vector3 dir = (position - tp).normalized;
        float z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(eulerOffset.x, eulerOffset.y, z + eulerOffset.z);
    }
    public static void Rotate2DTowards(Transform transform, Vector3 position, float speed)
    {
        Vector3 tp = transform.position;
        Vector3 dir = (position - tp).normalized;
        float z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, z), speed * Time.deltaTime);
    }
    public static void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
    public static Queue<string> ShuffleToQueue(string[] source)
    {
        var list = new List<string>(source);

        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(i, list.Count);
            (list[i], list[rnd]) = (list[rnd], list[i]);
        }

        return new Queue<string>(list);
    }
}
