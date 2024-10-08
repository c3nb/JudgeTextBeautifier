﻿using UnityEngine;

namespace JudgeTextBeautifier
{
    public class Language
    {
        public readonly string TextColor;
        public readonly string TopLeft;
        public readonly string BottomLeft;
        public readonly string TopRight;
        public readonly string BottomRight;
        public readonly string Gradient;
        public readonly string Reset;
        public readonly string ColorMode;

        public string TooEarly;
        public string VeryEarly;
        public string EarlyPerfect;
        public string Perfect;
        public string LatePerfect;
        public string VeryLate;
        public string TooLate;
        public string Multipress;
        public string FailMiss;
        public string FailOverload;
        public string FontSetting;
        public string FontWarning;
        public string FontSize;
        public string TextDuration;
        public string TextPunchDuration;
        public string TextOffset;
        public string CachedTextCount;

        public static Language Current => RDString.language == SystemLanguage.Korean ? Korean : English;
        public static readonly Language Korean = new Language("텍스트 색상", "왼쪽 상단", "왼쪽 하단", "오른쪽 상단", "오른쪽 하단", "그라데이션", "초기화", "색상 모드")
        {
            TooEarly = "너무 빠름",
            VeryEarly = "빠름!",
            EarlyPerfect = "빠름",
            Perfect = "정확",
            LatePerfect = "느림",
            VeryLate = "느림!",
            TooLate = "너무 느림",
            Multipress = "다중 입력",
            FailMiss = "놓침",
            FailOverload = "과부하",
            FontSetting = "폰트 설정",
            FontWarning = "색상 모드가 아닐 경우, OS에 설치된 폰트만 가능합니다!",
            FontSize = "글자 크기",
            TextDuration = "글자 지속시간",
            TextPunchDuration = "글자 나타나는 효과 지속시간",
            TextOffset = "글자 오프셋",
            CachedTextCount = "캐시된 텍스트 개수"
        };
        public static readonly Language English = new Language("Text Color", "Top Left", "Bottom Left", "Top Right", "Bottom Right", "Gradient", "Reset", "Color Mode")
        {
            TooEarly = "TooEarly",
            VeryEarly = "VeryEarly",
            EarlyPerfect = "EarlyPerfect",
            Perfect = "Perfect",
            LatePerfect = "LatePerfect",
            VeryLate = "VeryLate",
            TooLate = "TooLate",
            Multipress = "Multipress",
            FailMiss = "FailMiss",
            FailOverload = "FailOverload",
            FontSetting = "Font Setting",
            FontWarning = "If Not In Color Mode, Only Fonts Installed In The OS Are Available!",
            FontSize = "Font Size",
            TextDuration = "Text Appear Animation Duration",
            TextOffset = "Text Offset",
            CachedTextCount = "Cached Text Count"
        };
        public Language(string textColor, string topLeft, string bottomLeft, string topRight, string bottomRight, string gradient, string reset, string colorMode)
        {
            TextColor = textColor;
            TopLeft = topLeft;
            BottomLeft = bottomLeft;
            TopRight = topRight;
            BottomRight = bottomRight;
            Gradient = gradient;
            Reset = reset;
            ColorMode = colorMode;
        }
    }
}
