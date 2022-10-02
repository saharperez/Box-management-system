using DataStracturesPrj;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    internal class BoxBase : IComparable<BoxBase>
    {
        internal double Width { get; private set; }
        internal BST<BoxHeight> BoxHeight { get; set; }

        public BoxBase(double width)
        {
            BoxHeight = new BST<BoxHeight>();
            Width = width;
        }

        

        public int CompareTo(BoxBase other)
        {
            return Width.CompareTo(other.Width);
        }
        public override string ToString()
        {
            return $"The box Width: {Width} ";
        }
        

        
    }
}
