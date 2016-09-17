using System.Collections.Generic;
using System.Linq;

namespace Battleships.Model
{
    public class Fleet
    {
        public Ship Battleship { get; }
        public Ship Carrier { get; }
        public Ship Cruiser { get; }
        public Ship Submarine { get; }
        public Ship Destroyer { get; }

        public Fleet()
        {
            Battleship = new Ship(5, "Battleship");
            Carrier = new Ship(4, "Carrier");
            Cruiser = new Ship(3, "Cruiser");
            Submarine = new Ship(3, "Submarine");
            Destroyer = new Ship(2, "Destroyer");
        }

        public bool TileIsOccupied(Tile tile)
        {
            //Returns true if any of the ships contains the tile
            return Battleship.HitTest(tile)
                   || Carrier.HitTest(tile)
                   || Cruiser.HitTest(tile)
                   || Submarine.HitTest(tile)
                   || Destroyer.HitTest(tile);
        }

        public bool ScanTileForOccupation(Tile t)
        {
            //Returns true if there is a ship on, or adjecent to the tile
            var result = TileIsOccupied(t);
            result = result || TileIsOccupied(new Tile(t.X + 1, t.Y));
            result = result || TileIsOccupied(new Tile(t.X - 1, t.Y));
            result = result || TileIsOccupied(new Tile(t.X, t.Y + 1));
            result = result || TileIsOccupied(new Tile(t.X, t.Y - 1));
            return result;
        }

        public void GetHitTileAndShip(Tile t, out Tile outTile, out Ship outShip)
        {
            //First loops over the fleet. For each ship, if the tiles contain the searched-for tile, said tile and the ship are returned
            foreach (var ship in AsList().ToList())
            {
                foreach (var tile in ship.GetTiles().Where(tile => tile.Equals(t)))
                {
                    outTile = tile;
                    outShip = ship;
                    return;
                }
            }
            outTile = null;
            outShip = null;
        }

        public IEnumerable<Ship> AsList()
        {
            //Returns all the ships in the fleet as a list, to allow easy iterating
            return new List<Ship> { Battleship, Carrier, Cruiser, Submarine, Destroyer };
        }

        public string GetNameOfSunkShip(Ship ship)
        {
            foreach (var s in AsList().Where(s => s.Equals(ship)))
            {
                return s.Name;
            }
            return "";
        }

        public int GetSmallestLength()
        {
            if (!Battleship.IsSunk())
                return 5;
            if (!Carrier.IsSunk())
                return 4;
            return !Submarine.IsSunk() || !Cruiser.IsSunk() ? 3 : 2;
        }

        public bool AllPlaced()
        {
            //Returns true if all ships in a player's fleet are placed
            //Used for enabling the Start-button
            return AsList().All(s => s.IsPlaced());
        }

        public bool IsDestroyed()
        {
            //Returns true if all ships in the fleet are destoyed
            //Called when a ship is sunk, to determine if the game has ended
            return AsList().All(s => s.IsSunk());
        }
    }
}
