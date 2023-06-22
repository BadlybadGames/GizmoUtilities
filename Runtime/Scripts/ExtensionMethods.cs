using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Random = System.Random;

using UnityEngine;

public static class ExtensionMethods
{
    
    public static void Shuffle<T>(this IList<T> list)
    {
        var rng = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    // GetActor a random element
    public static T RandomElement<T>(this IList<T> list)
    {
        var rng = new Random();
        int i = rng.Next(0, list.Count);

        return list[i];
    }

    public static KeyValuePair<TKey, TValue> RandomElement<TKey, TValue>(this IDictionary<TKey, TValue> list)
    {
        var rng = new Random();
        int i = rng.Next(0, list.Count);

        return list.ElementAt(i);
    }

    // https://answers.unity.com/questions/661383/whats-the-most-efficient-way-to-rotate-a-vector2-o.html
    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
    
    public static Vector3Int[] AdjacentNoDiagonal(this Vector3Int v)
    {
        int x, y, z;
        x = v.x;
        y = v.y;
        z = v.z;

        Vector3Int[] ret = new Vector3Int[6];

        int i = 0;
        ret[i++] = new Vector3Int(x + 1, y + 0, z + 0);
        ret[i++] = new Vector3Int(x - 1, y + 0, z + 0);
        ret[i++] = new Vector3Int(x + 0, y + 1, z + 0);
        ret[i++] = new Vector3Int(x + 0, y - 1, z + 0);
        ret[i++] = new Vector3Int(x + 0, y + 0, z + 1);
        ret[i++] = new Vector3Int(x + 0, y + 0, z - 1);

        return ret;
    }

    /// <summary>
    /// All adjacent, no y difference, no diagonals
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector3Int[] AdjacentSameHeight(this Vector3Int v)
    {
        int x, y, z;
        x = v.x;
        y = v.y;
        z = v.z;

        Vector3Int[] ret = new Vector3Int[6];

        int i = 0;
        ret[i++] = new Vector3Int(x + 1, y + 0, z + 0);
        ret[i++] = new Vector3Int(x - 1, y + 0, z + 0);
        //ret[i++] = new Vector3Int(x + 0, y + 1, z + 0);
        //ret[i++] = new Vector3Int(x + 0, y - 1, z + 0);
        ret[i++] = new Vector3Int(x + 0, y + 0, z + 1);
        ret[i++] = new Vector3Int(x + 0, y + 0, z - 1);

        return ret;
    }

    public static bool IsAdjacent(this Vector3Int v, Vector3Int other)
    {
        throw new NotImplementedException();
    }

    public static bool IsAdjacentNoDiagonals(this Vector3Int v, Vector3Int other)
    {
        Vector3Int diff = (v - other);
        if (diff.sqrMagnitude == 0)
        {
            return false;
        }

        int xDiff = Mathf.Abs(diff.x);
        int yDiff = Mathf.Abs(diff.y);
        int zDiff = Mathf.Abs(diff.z);

        if (xDiff > 1 || yDiff > 1 || zDiff > 1)
        {
            return false;
        }

        if ((xDiff == 1) ^ (yDiff == 1) ^ (zDiff == 1))
        {
            return true;
        }

        return false;

    }
}
