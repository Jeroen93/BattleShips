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
            //Converts the tile to a coodinate. For example; 0,0 to A1
            return $"{GlobalVar.Letters[Y]}{X + 1}";
        }

        public bool Equals(Tile other)
        {
            return X.Equals(other.X)
                   && Y.Equals(other.Y);
        }
    }
}
