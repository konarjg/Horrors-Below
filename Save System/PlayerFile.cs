using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class PlayerFile
{
    public static void Save(PlayerStats obj, string path)
    {
        var fields = typeof(PlayerStats).GetFields();
        var lines = new List<string>();

        for (int i = 0; i < fields.Length; ++i)
        {
            var name = fields[i].Name;
            var value = fields[i].GetValue(obj).ToString();

            lines.Add(name + "=" + value);
        }

        File.WriteAllLines(path, lines);
    }

    public static void Load(ref PlayerStats obj, string path)
    {
        if (!File.Exists(path))
            throw new Exception("File doesn't exist!");

        var fields = typeof(PlayerStats).GetFields();
        var lines = File.ReadAllLines(path);

        for (int i = 0; i < fields.Length; ++i)
        {
            var value = lines[i].Split('=')[1];
            fields[i].SetValue(obj, float.Parse(value));
        }
    }
}
