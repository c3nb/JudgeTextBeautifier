using System;

namespace JudgeTextBeautifier
{
    public static class Tags
    {
        public static void Enable()
        {
            Main.mod.Logger.Log("Text Compiling Enabled!");
            Compilers = new Replacer[10]
            {
                new Replacer(TagManager.All),
                new Replacer(TagManager.All),
                new Replacer(TagManager.All),
                new Replacer(TagManager.All),
                new Replacer(TagManager.All),
                new Replacer(TagManager.All),
                new Replacer(TagManager.All),
                new Replacer(TagManager.All),
                new Replacer(TagManager.All),
                new Replacer(TagManager.All),
            };
        }
        public static void Disable()
            => Compilers = null;
        private static Array Compilers;
        public static string GetResult(HitMargin hitMargin)
            => ((Replacer[])Compilers)[(int)hitMargin].Replace();
        public static void Compile(HitMargin hitMargin, string source)
            => ((Replacer[])Compilers)[(int)hitMargin].Source = source;
        public static void CompileAll(Settings settings)
        {
            Compile(HitMargin.TooEarly, settings.TooEarly);
            Compile(HitMargin.VeryEarly, settings.VeryEarly);
            Compile(HitMargin.EarlyPerfect, settings.EarlyPerfect);
            Compile(HitMargin.Perfect, settings.Perfect);
            Compile(HitMargin.LatePerfect, settings.LatePerfect);
            Compile(HitMargin.VeryLate, settings.VeryLate);
            Compile(HitMargin.TooLate, settings.TooLate);
            Compile(HitMargin.Multipress, settings.Multipress);
            Compile(HitMargin.FailMiss, settings.FailMiss);
            Compile(HitMargin.FailOverload, settings.FailOverload);
        }
    }
}
