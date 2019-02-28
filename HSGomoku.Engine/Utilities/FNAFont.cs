using System;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;

namespace HSGomoku.Engine.Utilities
{
    internal static class FNAFont
    {
        private static String _fontFamily;

        public static Font Font8 { get; private set; }
        public static Font Font10 { get; private set; }
        public static Font Font14 { get; private set; }
        public static Font Font16 { get; private set; }
        public static Font Font18 { get; private set; }
        public static Font Font20 { get; private set; }

        public static void InitFont()
        {
            if (CheckFontExist("思源黑体"))
            {
                _fontFamily = "思源黑体";
            }
            else if (CheckFontExist("微软雅黑"))
            {
                _fontFamily = "微软雅黑";
            }
            else
            {
                _fontFamily = "宋体";
            }

            Font8 = new Font(new FontFamily(_fontFamily), 8);
            Font10 = new Font(new FontFamily(_fontFamily), 10);
            Font14 = new Font(new FontFamily(_fontFamily), 14);
            Font16 = new Font(new FontFamily(_fontFamily), 16);
            Font18 = new Font(new FontFamily(_fontFamily), 18);
            Font20 = new Font(new FontFamily(_fontFamily), 20);
        }

        public static Font GetFont(Int32 fontSize)
        {
            return new Font(new FontFamily(_fontFamily), fontSize);
        }

        public static Boolean CheckFontExist(String fontName)
        {
            InstalledFontCollection ifc = new InstalledFontCollection();
            FontFamily[] fontFamilys = ifc.Families;
            if (fontFamilys == null || fontFamilys.Length < 1)
            {
                return false;
            }
            if (fontFamilys.Any(f => f.Name == fontName))
            {
                return true;
            }

            return false;
        }
    }
}