using System;
using Microsoft.Xna.Framework;

namespace SquareBall.Ways
{
    public class Way
    {
        float k;
        float b;

        Point A, B;

        public Way(int x0, int y0, int x, int y)
        {
            A = new Point(x0, y0);
            B = new Point(x, y);
            k = (float)(y - y0) / (x - x0);
            b = y0;
            //y = k*x + b
        }

        public virtual Vector2 GetWayXYbyPointer(float x, float y)
        {
            if (x < A.X || x > B.X)
                return Vector2.Zero;
            return new Vector2(x, A.Y + k * (-A.X + x));
        }
    }
}
