using System;
using Microsoft.Xna.Framework;

namespace SquareBall.Ways
{

    public class CircleWay : Way
    {
        int R;
        Point O;
        int left;

        public CircleWay(int x0, int y0, int r)
            : base(0, 0, 0, 0)
        {
            O = new Point(x0, y0);
            R = Math.Abs(r);
            left = (r < 0) ? 1 : -1;
            //(y-y0)2 + (x-x0)2 = R2
            //x = s2(R2-(y-y0)2) + x0
        }

        public override Vector2 GetWayXYbyPointer(float x, float y)
        {
            if (y > O.Y + R / 1.2 || y < O.Y - R / 1.2)
                return Vector2.Zero;
            return new Vector2((float)(left*Math.Sqrt(R * R - (y - O.Y) * (y - O.Y)) + O.X), y);
        }
    }
}
