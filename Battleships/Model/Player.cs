using System;
using System.Linq;
using Battleships.Control;

namespace Battleships.Model
{
    public class Player
    {
        public Fleet Fleet { get; }

        private readonly Field _field;

        public Player(Field field)
        {
            _field = field;
            Fleet = new Fleet();
        }

        private Ship _detectedShip = new Ship();
        private readonly Fleet _detectedFleet = new Fleet();

        public void Think()
        {
            var hit = false;
            var sunk = false;
            var count = _detectedShip.GetTiles().Count;
            var random = new Random();

            //if we have detected a tile of a ship
            if (count == 1)
            {
                var tile = _detectedShip.GetTiles()[0];
                var leftTested = tile.X == 0 || _field.Shots.Contains(new Tile(tile.X - 1, tile.Y));
                var rightTested = tile.X == 9 || _field.Shots.Contains(new Tile(tile.X + 1, tile.Y));
                var upTested = tile.Y == 0 || _field.Shots.Contains(new Tile(tile.X, tile.Y - 1));
                var downTested = tile.Y == 9 || _field.Shots.Contains(new Tile(tile.X, tile.Y + 1));

                if (leftTested && rightTested)
                    _detectedShip.Direction = Direction.Vertical;
                if (upTested && downTested)
                    _detectedShip.Direction = Direction.Horizontal;

                var redo = true;
                do
                {
                    switch (random.Next(0, 4))
                    {
                        case 0:
                            //shoot left
                            if (!leftTested)
                                redo = !_field.Shoot(tile.X - 1, tile.Y, out hit, out sunk);
                            break;
                        case 1:
                            //shoot right
                            if (!rightTested)
                                redo = !_field.Shoot(tile.X + 1, tile.Y, out hit, out sunk);
                            break;
                        case 2:
                            //shoot up
                            if (!upTested)
                                redo = !_field.Shoot(tile.X, tile.Y - 1, out hit, out sunk);
                            break;
                        case 3:
                            //shoot down
                            if (!downTested)
                                redo = !_field.Shoot(tile.X, tile.Y + 1, out hit, out sunk);
                            break;
                    }
                } while (redo);
            }
            else if (count > 1)
            {
                var tilefirst = _detectedShip.GetTiles().First();
                var tilelast = _detectedShip.GetTiles().Last();

                if (_detectedShip.Direction == Direction.Unknown)
                {
                    _detectedShip.Direction = tilefirst.X == tilelast.X ? Direction.Vertical : Direction.Horizontal;
                }

                if (_detectedShip.Direction == Direction.Vertical)
                {
                    if (tilelast.Y > tilefirst.Y)
                    {
                        if (!_field.Shoot(tilelast.X, tilelast.Y + 1, out hit, out sunk))
                        {
                            _field.Shoot(tilefirst.X, tilefirst.Y - 1, out hit, out sunk);
                        }
                    }
                    else
                    {
                        if (!_field.Shoot(tilelast.X, tilelast.Y - 1, out hit, out sunk))
                        {
                            _field.Shoot(tilefirst.X, tilefirst.Y + 1, out hit, out sunk);
                        }
                    }
                }
                else
                {
                    if (tilelast.X > tilefirst.X)
                    {
                        if (!_field.Shoot(tilelast.X + 1, tilelast.Y, out hit, out sunk))
                        {
                            _field.Shoot(tilefirst.X - 1, tilefirst.Y, out hit, out sunk);
                        }
                    }
                    else
                    {
                        if (!_field.Shoot(tilelast.X - 1, tilelast.Y, out hit, out sunk))
                        {
                            _field.Shoot(tilefirst.X + 1, tilefirst.Y, out hit, out sunk);
                        }
                    }
                }
            }

            //If it is a hit, add it to the ship
            if (hit)
                _detectedShip.AddTile(_field.Shots.Last());

            //If the ship is sunk, get the name of the sunken ship, and add the tiles to
            //the same ship in the fleet the AI has already detected. Reset the ship afterwards
            if (sunk)
            {
                var name = Fleet.GetNameOfSunkShip(_detectedShip);
                var sunkShip = _detectedFleet.AsList().First(s => s.Name.Equals(name));
                sunkShip.AddTiles(_detectedShip.GetTiles());
                _detectedShip = new Ship();
            }

            //We have nothing to investigate, shoot a random tile
            if (count == 0 && ShootRandom())
            {
                //If we have detected a ship, add the tile to our list
                _detectedShip.AddTile(_field.Shots.Last());
            }
        }

        private bool ShootRandom()
        {
            var r = new Random();
            bool hit;
            do
            {
                /*  - Pick random x and y
                 *  - Check if it is within the field
                 *  - Check if it hasn't been tried before
                 *  - Check if the tile isn't adjecent to a ship
                 *  - Check if there is enough space to place the smallest ship in each direction
                 *  - If any of them fails, pick a new x and y
                 *  - else, shoot there
                 */
                var x = r.Next(0, 10);
                var y = r.Next(0, 10);
                var t = new Tile(x, y);

                if (!IsValid(t)) continue;
                if (!HasEnoughSpace(x, y)) continue;
                bool sunk;
                _field.Shoot(x, y, out hit, out sunk);
                break;
            } while (true);
            return hit;
        }

        //Checks if a tile is within bounds, not already shot, and not adjacent to another ship
        private bool IsValid(Tile t)
        {
            return t.X >= 0 && t.X <= 9 && t.Y >= 0 && t.Y <= 9
                   && !_field.Shots.Contains(t)
                   && !_detectedFleet.ScanTileForOccupation(t);
        }

        private bool HasEnoughSpace(int x, int y)
        {
            var reqLength = Fleet.GetSmallestLength();
            var horizontal = CheckInDirection(x, y, -1, 0, reqLength) + CheckInDirection(x, y, 1, 0, reqLength) - 1;
            var vertical = CheckInDirection(x, y, 0, -1, reqLength) + CheckInDirection(x, y, 0, -1, reqLength) - 1;
            return horizontal >= reqLength || vertical >= reqLength;
        }

        private int CheckInDirection(int x, int y, int xOffset, int yOffset, int reqLength)
        {
            var foundLength = 1;
            for (var i = 0; i < reqLength; i++)
            {
                if (!IsValid(new Tile(x + i * xOffset, y + i * yOffset)))
                    break;
                foundLength++;
            }
            return foundLength;
        }
    }
}
