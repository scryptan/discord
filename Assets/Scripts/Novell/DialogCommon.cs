using System;
using ThinIce.Helpers;
using UnityEngine;

namespace ThinIce.Novell
{
    [Serializable]
    public struct DialogCommon
    {
        [Multiline(3)]
        public string textGirl;
        public GirlEmotion girlEmotion;

        public TextGuy[] textGuy;
    }
}