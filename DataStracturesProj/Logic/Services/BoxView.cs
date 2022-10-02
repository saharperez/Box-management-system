using DataStracturesPrj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class BoxView
    {
        internal BoxBase BoxBase { get; set; }

        internal BoxHeight BoxHeight { get; set; }
        internal bool Flag { get; private set; }// Flag to work in the manager, True For Bigger Than Amount , False For Smaller Or Equal

        internal BST<BoxHeight> BoxHeights { get; private set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public int Amount { get; set; }

        internal BoxView(BoxBase boxBase, BoxHeight boxHeight, bool flag, int amount)
        {
            BoxBase = boxBase;
            BoxHeight = boxHeight;
            BoxHeights = boxBase.BoxHeight;
            Flag = flag;
            Width = boxBase.Width;
            Height = boxHeight.Height;
            Amount = amount;
        }
        
    }
}
