using DataStracturesPrj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    internal class BoxHeight : IComparable<BoxHeight>
    {
        internal double Height { get; private set; }
        internal int Amount { get; set; }

        public MyLinkedList<TimeData>.Node TimeDataNode { get; set; }

        public BoxHeight(double height)
        {
            Height = height;
            Amount = 0;
        }

        public int CompareTo(BoxHeight other) => Height.CompareTo(other.Height);
        public override string ToString() => $"Height : {Height} , Amount : {Amount}";


    }
}
