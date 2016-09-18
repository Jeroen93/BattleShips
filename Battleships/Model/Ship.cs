using System.Collections.Generic;
using System.Linq;

namespace Battleships.Model
{
    public class Ship
    {
        private readonly List<Tile> _tiles;

        public string Name { get; }
        public int Length { get; }
        public Direction Direction { get; set; }

        public Ship(int length, string name)
        {
            Length = length;
            Name = name;
            _tiles = new List<Tile>();
        }

        public Ship()
        {
            _tiles = new List<Tile>();
            Direction = Direction.Unknown;
        }

        public void AddTile(Tile t)
        {
            _tiles.Add(t);
        }

        public void AddTiles(List<Tile> tiles)
        {
            tiles.ForEach(t => _tiles.Add(t));
        }

        public List<Tile> GetTiles()
        {
            return _tiles;
        }

        public void ClearTiles()
        {
            _tiles.Clear();
        }

        public bool HitTest(Tile t)
        {
            return _tiles.Contains(t);
        }

        public bool IsPlaced()
        {
            return _tiles.Count != 0;
        }

        public bool IsSunk()
        {
            return _tiles.TrueForAll(t => t.IsHit);
        }

        public bool Equals(Ship other)
        {
            return other._tiles.All(HitTest);
        }
    }

    public enum Direction
    {
        Horizontal,
        Vertical,
        Unknown
    }
}
