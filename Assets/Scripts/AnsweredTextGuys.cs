using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace ThinIce
{
    public class AnsweredTextGuys
    {
        private static readonly string Key = "answeredIds";
        private static readonly string Path = $"{Application.persistentDataPath}/{Key}.json";
        private static HashSet<uint> _answered;

        public static void Answer(TextGuy textGuy)
        {
            if (textGuy == null)
                return;

            _answered ??= ReadSavedData();
            _answered.Add(textGuy.id);
            WriteAnsweredIds();
        }

        public static bool IsAnswered(TextGuy textGuy)
        {
            if (textGuy == null)
                return false;
            _answered ??= ReadSavedData();
            return _answered.Contains(textGuy.id);
        }

        private static HashSet<uint> ReadSavedData()
        {
            var text = GetSavedText();
            _answered = new HashSet<uint>(JsonConvert.DeserializeObject<uint[]>(text ?? string.Empty) ?? Array.Empty<uint>());

            return _answered;
        }

        private static void WriteAnsweredIds()
        {
            SaveText(JsonConvert.SerializeObject(_answered));
        }

        private static string GetSavedText()
        {
#if UNITY_EDITOR

            if (File.Exists(Path))
            {
                try
                {
                    return File.ReadAllText(Path);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return string.Empty;
#else
            return PlayerPrefs.GetString(Key);
#endif
        }

        private static void SaveText(string text)
        {
#if UNITY_EDITOR
            File.WriteAllText(Path, text);
#else
            PlayerPrefs.SetString(Key, text);
#endif
        }
    }
}