using System;
using System.Linq;
using Battleships.Control;

namespace Battleships.Model
{
    public class Player
    {
        public Fleet Fleet { get; }

        private Field _field;        

        public Player(Field field)
        {
            _field = field;
            Fleet = new Fleet();
        }

        private Ship _detectedShip = new Ship();
        private Fleet _detectedFleet = new Fleet();

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
                var leftTested = tile.X == 0 || _field._shots.Contains(new Tile(tile.X - 1, tile.Y));
                var rightTested = tile.X == 9 || _field._shots.Contains(new Tile(tile.X + 1, tile.Y));
                var upTested = tile.Y == 0 || _field._shots.Contains(new Tile(tile.X, tile.Y - 1));
                var downTested = tile.Y == 9 || _field._shots.Contains(new Tile(tile.X, tile.Y + 1));

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
                _detectedShip.AddTile(_field._shots.Last());

            //If the ship is sunk, we are done with this ship
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
                _detectedShip.AddTile(_field._shots.Last());
            }
        }

        private bool ShootRandom()
        {
            var r = new Random();
            bool hit;
            bool result;
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
                bool sunk;
                result = _field.Shoot(x, y, out hit, out sunk);
            } while (!result);
            return hit;
        }
    }
}
