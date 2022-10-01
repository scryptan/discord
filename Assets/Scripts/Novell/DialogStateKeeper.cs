using System;
using System.Collections.Generic;
using System.Linq;
using ThinIce.Helpers;
using UnityEngine;

namespace ThinIce.Novell
{
    public class DialogStateKeeper: MonoBehaviour
    {
        public List<GirlEmotion> MainDialogEmotions = new List<GirlEmotion>();
        public Dictionary<uint, DialogStateData> DialogStateDict => DialogState.ToDictionary(x => x.Id);
        public List<DialogStateData> DialogState = new List<DialogStateData>();
    }

    [Serializable]
    public class DialogStateData
    {
        public uint Id;
        public bool IsBadText;
        public GirlEmotion Emotion;
    }
}