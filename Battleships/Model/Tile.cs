using System;

namespace Battleships.Model
{
    public class Tile : IEquatable<Tile>
    {
        public int X { get; }
        public int Y { get; }
        public bool IsHit { get; set; }

        public Tile(int x, int y)
        {
            X = x;
            Y = y;
            IsHit = false;
        }

        public override string ToString()
        {
            return $"{GlobalVar.Letters[Y]}{X + 1}";
        }

        public bool Equals(Tile other)
        {
            return X.Equals(other.X)
                   && Y.Equals(other.Y);
        }
    }
}
