using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ChineseCheckers.Core
{
    public class Marble
    {
        public Marble(Color color) => Color = color;

        public Color Color { get; }
    }
}
