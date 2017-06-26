using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Chroma.NetCore.Api.Exceptions;

namespace Chroma.NetCore.Api.Chroma
{
    public class Color
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        private readonly string color;

        public Color()
        {

        }

        public Color(string color)
        {
            this.color = color;
            ToRgb();
        }

        public Color(int r, int g, int b) : this((byte)r, (byte)g, (byte)b)
        {

        }

        public Color(byte r, byte g, byte b)
        {
            this.R = r;
            this.G = g;
            this.B = b;

            //Check borders
            if (this.R > 255) this.R = 255;
            if (this.G > 255) this.G = 255;
            if (this.B > 255) this.B = 255;
            if (this.R < 0) this.R = 0;
            if (this.G < 0) this.G = 0;
            if (this.B < 0) this.B = 0;
        }

        public static Color Black = new Color("000000");
        public static Color Red = new Color("ff0000");
        public static Color Green = new Color("00ff00");
        public static Color Blue = new Color("0000ff");
        public static Color HotPink = new Color(255, 105, 180);
        public static Color Orange = new Color("ffa500");
        public static Color Pink = new Color("ff00ff");
        public static Color Purple = new Color("800080");
        public static Color White = new Color(255, 255, 255);
        public static Color Yellow = new Color(255, 255, 0);


        public string ToRgb()
        {
            if (string.IsNullOrEmpty(color))
                return $"{R} {G} {B}";

            var colorComponents = Regex.Match(color, "^#?([a-f\\d]{2})([a-f\\d]{2})([a-f\\d]{2})$", RegexOptions.IgnoreCase);

            if(colorComponents.Groups.Count != 4)
                throw new ChromaNetCoreApiException("Invalid color format!");

            R = byte.Parse(colorComponents.Groups[1].Value, NumberStyles.HexNumber);
            G = byte.Parse(colorComponents.Groups[2].Value, NumberStyles.HexNumber);
            B = byte.Parse(colorComponents.Groups[3].Value, NumberStyles.HexNumber);

            return $"{R} {G} {B}";
        }

        /// <summary>
        /// Convert color to BGR scheme.
        /// </summary>
        /// <returns>BGR value as int.</returns>
        public uint ToBgr()
        {
            var rHex = this.R.ToString("X");
            if (rHex.Length < 2) rHex = "0" + rHex;
            var gHex = this.G.ToString("X");
            if (gHex.Length < 2) gHex = "0" + gHex;
            var bHex = this.B.ToString("X");
            if (bHex.Length < 2) bHex = "0" + bHex;

            var result = bHex + gHex + rHex;
            
            return uint.Parse(result, NumberStyles.HexNumber);
        }

        public override string ToString()
        {
            return ToBgr().ToString();
        }
    }
}
