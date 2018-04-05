using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace googoleOcr
{
    class BoundingText
    {
        public BoundingText(string text, int left, int top, int width, int height)
        {
            Text = text;
            this.Width = width;
            this.Height = height;
            this.Top = top;
            this.Left = left;
        }
        public string Text { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
    }
}
