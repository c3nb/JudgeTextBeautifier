using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Overlayer;

namespace JudgeTextBeautifier
{
    public static class Overlayer
    {
        public static void Enable()
        {
            Compilers = new TagCompiler[10]
            {
                new TagCompiler(Tag.Tags),
                new TagCompiler(Tag.Tags),
                new TagCompiler(Tag.Tags),
                new TagCompiler(Tag.Tags),
                new TagCompiler(Tag.Tags),
                new TagCompiler(Tag.Tags),
                new TagCompiler(Tag.Tags),
                new TagCompiler(Tag.Tags),
                new TagCompiler(Tag.Tags),
                new TagCompiler(Tag.Tags),
            };
        }
        public static void Disable()
            => Compilers = null;
        private static Array Compilers;
        public static string GetResult(HitMargin hitMargin)
            => ((TagCompiler[])Compilers)[(int)hitMargin].Result;
        public static void Compile(HitMargin hitMargin, string source)
            => ((TagCompiler[])Compilers)[(int)hitMargin].Compile(source);
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
