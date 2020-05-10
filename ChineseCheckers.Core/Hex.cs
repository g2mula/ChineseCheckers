using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ChineseCheckers.Core
{
    public struct Hex
    {
        public Hex(int q, int r) => (Q, R) = (q, r);
        public Hex(int x, int y, int z)
        {
            if (x + y + z != 0)
            {
                throw new ArgumentException("x + y + z should equal 0");
            }

            (Q, R) = (x, z);
        }

        public Hex(PointF point, int size)
        {
            var x = (Math.Sqrt(3) / 3 * point.X - 1.0 / 3 * point.Y) / size;
            var z = (2.0 / 3 * point.Y) / size;
            var y = -x - z;

            var rx = Math.Round(x);
            var ry = Math.Round(y);
            var rz = Math.Round(z);

            var x_diff = Math.Abs(rx - x);
            var y_diff = Math.Abs(ry - y);
            var z_diff = Math.Abs(rz - z);

            if (x_diff > y_diff && x_diff > z_diff)
            {
                rx = -ry - rz;
            }
            else if (y_diff > z_diff)
            {
                ry = -rx - rz;
            }
            else
            {
                rz = -rx - ry;
            }

            Q = (int)rx;
            R = (int)rz;
        }

        public int Q { get; }
        public int R { get; }

        public int X => Q;
        public int Y => -Q - R;
        public int Z => R;

        public PointF ToPoint(int size) => ToPoint(size, (0, 0));
        public PointF ToPoint(int size, (float X, float Y) offset)
        {
            var x = size * (Math.Sqrt(3) * Q + Math.Sqrt(3) / 2 * R);
            var y = size * (3.0 / 2 * R);

            return new PointF((float)x + offset.X, (float)y + offset.Y);
        }

        public static Hex FromPoint(PointF point, int size, (float X, float Y) offset)
        {
            var unoffsetX = point.X - offset.X;
            var unoffsetY = point.Y - offset.Y;

            var r = (2 / 3.0 * unoffsetY) / size;
            var q = (Math.Sqrt(3) / 3.0 * unoffsetX - 1.0 / 3 * unoffsetY) / size;

            return GetRoundedHex(q, r);
        }

        private static Hex GetRoundedHex(double q, double r)
        {
            var (x, y, z) = (q, -q - r, r);

            var rx = (int)Math.Round(x);
            var ry = (int)Math.Round(y);
            var rz = (int)Math.Round(z);

            var x_diff = Math.Abs(rx - x);
            var y_diff = Math.Abs(ry - y);
            var z_diff = Math.Abs(rz - z);

            if (x_diff > y_diff && x_diff > z_diff)
            {
                rx = -ry - rz;
            }
            else if (y_diff > z_diff)
            {
                ry = -rx - rz;
            }
            else
            {
                rz = -rx - ry;
            }

            return new Hex(rx, ry, rz);
        }

        public int DistanceTo(Hex destination)
            => Enumerable.Max(new[] { Math.Abs(X - destination.X), Math.Abs(Y - destination.Y), Math.Abs(Z - destination.Z) });

        public static Hex operator +(Hex point, (int X, int Y, int Z) vector)
            => new Hex(point.X + vector.X, point.Y + vector.Y, point.Z + vector.Z);

        public static bool operator ==(Hex point1, Hex point2)
            => point1.Q == point2.Q && point1.R == point2.R;

        public static bool operator !=(Hex point1, Hex point2)
            => !(point1 == point2);

        public static bool Equals(Hex point1, Hex point2)
            => point1.Q.Equals(point2.Q) && point1.R.Equals(point2.R);

        public override bool Equals(object obj)
            => obj is Hex point && Equals(this, point);

        public bool Equals(Hex point)
            => Equals(this, point);

        public override string ToString()
            => $"{X}, {Y}, {Z}";

        public override int GetHashCode()
        {
            var hashCode = -1997189103;
            hashCode = hashCode * -1521134295 + Q.GetHashCode();
            hashCode = hashCode * -1521134295 + R.GetHashCode();

            return hashCode;
        }
    }
}
