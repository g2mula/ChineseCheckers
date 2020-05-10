using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ChineseCheckers.Core
{
    public static class Utils
    {
        public static int DistanceTo(this Hex first, Hex second)
            => Enumerable.Max(new[] { Math.Abs(first.X - second.X), Math.Abs(first.Y - second.Y), Math.Abs(first.Z - second.Z) });

        public static int ToDistance(this (Hex first, Hex second) move)
            => move.first.DistanceTo(move.second);

        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> keyValuePair, out TKey key, out TValue value)
            => (key, value) = (keyValuePair.Key, keyValuePair.Value);

        public static (float X, float Y) GetOffset(this Board board, int holeSize)
        {
            var triangleBase = board.TriangleBase;

            var heightPoint = new Hex(triangleBase, triangleBase * 2).ToPoint(holeSize);
            var widthPoint = new Hex(triangleBase, triangleBase).ToPoint(holeSize);

            var offsetX = widthPoint.X + holeSize;
            var offsetY = heightPoint.Y + holeSize;

            return (offsetX, offsetY);
        }
    }
}
