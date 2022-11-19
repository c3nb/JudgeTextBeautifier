using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Overlayer.Core;

namespace JudgeTextBeautifier
{
    public static class Tags
    {
        public static void Enable()
        {
            Main.mod.Logger.Log("Text Compiling Enabled!");
            Compilers = new TextCompiler[10]
            {
                new TextCompiler(TagManager.AllTags),
                new TextCompiler(TagManager.AllTags),
                new TextCompiler(TagManager.AllTags),
                new TextCompiler(TagManager.AllTags),
                new TextCompiler(TagManager.AllTags),
                new TextCompiler(TagManager.AllTags),
                new TextCompiler(TagManager.AllTags),
                new TextCompiler(TagManager.AllTags),
                new TextCompiler(TagManager.AllTags),
                new TextCompiler(TagManager.AllTags),
            };
        }
        public static void Disable()
            => Compilers = null;
        private static Array Compilers;
        public static string GetResult(HitMargin hitMargin)
            => ((TextCompiler[])Compilers)[(int)hitMargin].Result;
        public static void Compile(HitMargin hitMargin, string source)
            => ((TextCompiler[])Compilers)[(int)hitMargin].Compile(source);
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
