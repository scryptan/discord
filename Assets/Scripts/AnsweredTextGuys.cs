using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace ThinIce
{
    public class AnsweredTextGuys
    {
        private static readonly string Path = $"{Application.persistentDataPath}/answeredIds.json";
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
            if (File.Exists(Path))
            {
                try
                {
                    _answered = JsonConvert.DeserializeObject<HashSet<uint>>(File.ReadAllText(Path) ?? string.Empty);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            _answered ??= new HashSet<uint>();

            return _answered;
        }

        private static void WriteAnsweredIds()
        {
            File.WriteAllText(Path, JsonConvert.SerializeObject(_answered));
        }
    }
}