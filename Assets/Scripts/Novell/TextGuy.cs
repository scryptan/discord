using System;
using ThinIce.Helpers;
using UnityEngine;

namespace ThinIce.Novell
{
    [Serializable]
    public class TextGuy
    {
        public uint id;
        [Multiline(3)]
        public string text = "";
        public bool badText = false;

        [Multiline(2)]
        public string girlAnswer = "";
        public GirlEmotion girtAnswerEmotion;
    }
}