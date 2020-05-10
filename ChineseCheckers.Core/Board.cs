using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;

namespace ChineseCheckers.Core
{
    public class Board : Dictionary<Hex, Marble>
    {
        private static readonly (int X, int Y, int Z)[] _neighborOffset = new[]
        {
            (0, 1, -1),
            (1, 0, -1),
            (1, -1, 0),
            (0, -1, 1),
            (-1, 0, 1),
            (-1, 1, 0),
        };

        private static readonly Color[] _homeColors = new Color[]
        {
            Color.Blue,
            Color.Green,
            Color.Yellow,
            Color.Orange,
            Color.Pink,
            Color.Purple
        };

        public static Board Create(int triangleBase) => new Board(triangleBase).Create();

        private Board(int triangleBase) => TriangleBase = triangleBase;

        private Board Create()
        {
            for (var x = -TriangleBase; x <= TriangleBase; x++)
            {
                for (var y = -TriangleBase; y <= TriangleBase; y++)
                {
                    var z = -x - y;
                    var distance = Math.Abs(x) + Math.Abs(y) + Math.Abs(z);
                    var isHomeBase = distance > TriangleBase * 2;

                    var position1 = new Hex(x, z);
                    Add(position1, null);

                    if (!isHomeBase)
                    {
                        continue;
                    }

                    var position2 = new Hex(x, y);
                    Add(position2, null);

                    var position3 = new Hex(z, x);
                    Add(position3, null);

                    if (x < 0)
                    {
                        this[position1] = new Marble(_homeColors[0]);
                        this[position2] = new Marble(_homeColors[2]);
                        this[position3] = new Marble(_homeColors[4]);
                    }
                    else
                    {
                        this[position1] = new Marble(_homeColors[1]);
                        this[position2] = new Marble(_homeColors[3]);
                        this[position3] = new Marble(_homeColors[5]);
                    }
                }
            }

            return this;
        }

        public int TriangleBase { get; }

        public IEnumerable<Hex> GetNeighbors(Hex position)
            => _neighborOffset.Select(offset => position + offset).Where(ContainsKey);

        public IEnumerable<Hex> PotentialMoves(Hex position)
            => GetEmptyNeighbors(position).Concat(GetJumpNeighbors(position)).Distinct();

        public IEnumerable<Hex> PotentialMoves(Hex position, (Hex from, Hex to) previousMove)
        {
            var distance = previousMove.ToDistance();
            if (distance == 2)
            {
                return GetJumpNeighbors(position);
            }

            return Enumerable.Empty<Hex>();
        }

        private IEnumerable<Hex> GetEmptyNeighbors(Hex position)
            => GetNeighbors(position).Where(pos => this[pos] == null);

        private IEnumerable<Hex> GetJumpNeighbors(Hex position) =>
            _neighborOffset
                .Select(offset => (Offset: offset, Position: position + offset))
                .Where(offsetPos => TryGetValue(offsetPos.Position, out var marble) && marble != null)
                .Select(offSetPos => offSetPos.Position + offSetPos.Offset)
                .Where(jump => TryGetValue(jump, out var marble) && marble == null)
                .Distinct();
    }
}
