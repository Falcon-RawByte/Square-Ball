using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SquareBall
{
    public class Counter
    {
        int length;
        int tick;
        public bool Running = false;
        int completed;

        public Counter(int _l, int _t)
        {
            length = _l;
            tick = _t;
            Running = false;
        }

        public Counter(int _l, int _t, bool Done)
        {
            length = _l;
            tick = _t;
            //UGGHH
            completed = length;
        }

        public bool Add()
        {
            completed += tick;
            Running = true;
            return completed >= length;
        }

        public void Reset()
        {
            completed = 0;
            Running = false;
        }

        public void Reset(int _l)
        {
            length = _l;
            completed = 0;
            Running = false;
        }

    }
}
