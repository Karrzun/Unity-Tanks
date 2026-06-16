using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class DictionaryExtensions
{

    public static void Log<T1, T2>(this Dictionary<T1, T2> dictionary)
    {
        StringBuilder sb = new StringBuilder("(");

        foreach (KeyValuePair<T1, T2> kvp in dictionary)
        {
            sb.Append(kvp.Key + ": " + kvp.Value + ", ");
        }

        sb.Remove(sb.Length - 2, 2);
        sb.Append(")");

        Debug.Log(sb.ToString());
    }

}
