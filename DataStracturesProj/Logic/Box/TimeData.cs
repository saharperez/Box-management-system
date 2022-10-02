using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class TimeData //TimeData Class Works with Date And Got The Information Of The Box
    {
        public double Width { get; private set; }
        public double Height { get; private set; }
        public DateTime Update { get; set; }
        internal BoxBase BoxBase { get; set; }

        internal BoxHeight BoxHeight { get; set; }

        internal TimeData(BoxBase box, BoxHeight height)
        {
            Width = box.Width;
            Height = height.Height;
            Update = DateTime.Now;
            BoxBase = box;
            BoxHeight = height;
        }

        public override string ToString()
        {
            return $"Box{Width} X {Height} Will be expired in ";
        }
    }
}
