using System;
using UnityEngine;

namespace ThinIce
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