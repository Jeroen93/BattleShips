using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Battleships.Model;

namespace Battleships.Control
{
    public partial class Field : UserControl
    {
        private readonly PictureBox[,] _boxesArray;
        private readonly Player _player;
        private readonly List<Tile> _shots;

        public FormMain MainForm;
        public bool IsAi;
        public bool Horizontal { get; set; }

        public Field()
        {
            InitializeComponent();
            _boxesArray = FillArray();
            _player = new Player();
            _shots = new List<Tile>();
            Horizontal = true;
        }

        public void SetFieldName(string text)
        {
            lblWhoseShips.Text = text;
        }

        private PictureBox[,] FillArray()
        {
            var array = new PictureBox[10, 10]; //Create a 2-dim array
            var boxes = Controls.OfType<PictureBox>().ToList(); //Get all pictureboxes in the control

            for (var y = 0; y < 10; y++)
            {
                var box = boxes.Where(b => b.Name.Remove(0, 10).StartsWith(GlobalVar.Letters[y])).ToList(); //Get all pictureboxes of a certain row
                for (var x = 0; x < 10; x++)
                {
                    var x1 = x;
                    foreach (var r in box.Where(r => Convert.ToUInt32(r.Name.Remove(0, 11)) == x1 + 1)) //Get the box with the correct x value
                    {
                        array[y, x] = r; //Add the box in the correct place in the array
                    }
                }
            }
            return array;
        }

        private void PbClick(object sender, EventArgs e)
        {
            //Get X and Y from clicked PictureBox
            var coor = ((PictureBox)sender).Name.Remove(0, 10);
            var y = Array.IndexOf(GlobalVar.Letters, coor.Substring(0, 1));
            var x = Convert.ToInt32(coor.Substring(1)) - 1;

            switch (MainForm.State)
            {
                case FormMain.GameState.Setup:
                    if (IsAi) //Only your field has a function
                        return;
                    var selectedShip = MainForm.GetSelectedShip(_player);
                    if (!AddShip(x, y, selectedShip, true))
                        MessageBox.Show(@"Ship can't be placed here, one of the tiles is already occupied");
                    break;
                case FormMain.GameState.TurnPlayer:
                    bool ignore;
                    if (IsAi) //Only the enemy field works, clicking a tile shoots there
                        Shoot(x, y, out ignore, out ignore);
                    break;
                case FormMain.GameState.TurnAi:
                case FormMain.GameState.End:
                    break; //Both fields have no function
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool AddShip(int x, int y, Ship ship, bool draw)
        {
            if (Horizontal)
            {
                x = AdjustForLength(x, ship.Length);
            }
            else
            {
                y = AdjustForLength(y, ship.Length);
            }
            var q = ship.Length / 2;
            var rOff = -q;
            var lOff = q + (ship.Length % 2 == 0 ? 0 : 1);
            ClearShip(ship);
            for (var i = rOff; i < lOff; i++)
            {
                var t = Horizontal ? new Tile(x + i, y) : new Tile(x, y + i);
                if (!_player.Fleet.ScanTileForOccupation(t)) continue;
                return false;
            }
            for (var i = rOff; i < lOff; i++)
            {
                ship.AddTile(Horizontal ? new Tile(x + i, y) : new Tile(x, y + i));
            }
            if (!draw) return true;
            Drawship(ship);
            MainForm.CheckStart(_player);
            return true;
        }

        private static int AdjustForLength(int x, int length)
        {
            var l = length / 2;
            x = Math.Min(x, 9 - l);
            x = Math.Max(x, l);
            return x;
        }

        private void Drawship(Ship ship)
        {
            foreach (var t in ship.GetTiles())
            {
                DrawAtBox(t, ship.Name);
            }
        }

        private void ClearShip(Ship ship)
        {
            ship.GetTiles().ForEach(p => GetBox(p).Image = null);
            ship.ClearTiles();
        }

        public void PlaceShipsRandom(bool draw)
        {
            var r = new Random();
            foreach (var ship in _player.Fleet.AsList())
            {
                bool result;
                do
                {
                    var x = r.Next(0, 10);
                    var y = r.Next(0, 10);
                    Horizontal = r.NextDouble() > 0.5;
                    result = !AddShip(x, y, ship, draw);
                } while (result);
            }
        }

        private bool Shoot(int x, int y, out bool hit, out bool sunk)
        {
            hit = false;
            sunk = false;

            if (x < 0 || x > 9 || y < 0 || y > 9)
                return false;

            var tile = new Tile(x, y);
            if (_shots.Contains(tile))
                return false;
            _shots.Add(tile);

            if (IsAi)
            {
                Ship s;
                var result = MainForm.ShootAtEnemy(tile, out s) ? "Hit" : "Miss";
                DrawAtBox(new Tile(x, y), result);
                MainForm.Log($"Shooting at {tile}, It's a {result.ToLower()}");
                if (s != null && s.IsSunk())
                    MainForm.Log($"You sunk the enemy's {s.Name}!");
                if (MainForm.EnemyFleetDestroyed())
                {
                    MainForm.Log("You destoyed the enemy's fleet!");
                    MainForm.State = FormMain.GameState.End;
                    return true;
                }

                MainForm.State = FormMain.GameState.TurnAi;
                MainForm.Think();
            }
            else
            {
                Ship s;
                hit = MainForm.EnemyShootsAtYou(tile, out s);
                var resulttext = hit ? "hit" : "miss";
                var imagetext = hit ? s.Name + "_shot" : "Miss";
                DrawAtBox(new Tile(x, y), imagetext);
                MainForm.Log($"The enemy shoots at {tile}, It's a {resulttext}");
                if (s != null && s.IsSunk())
                {
                    sunk = true;
                    MainForm.Log($"The enemy sunk your {s.Name}..");
                }
                if (MainForm.PlayerFleetDestroyed())
                {
                    MainForm.Log("The enemy destroyed your fleet..");
                    MainForm.State = FormMain.GameState.End;
                    return true;
                }

                MainForm.State = FormMain.GameState.TurnPlayer;
            }
            return true;
        }

        public bool FleetIsDestroyed()
        {
            return _player.Fleet.IsDestroyed();
        }

        private bool ShootRandom()
        {
            var r = new Random();
            bool hit;
            bool sunk;
            bool result;
            do
            {
                var x = r.Next(0, 10);
                var y = r.Next(0, 10);
                result = Shoot(x, y, out hit, out sunk);
            } while (!result);
            return hit;
        }

        public bool IsShotAHit(Tile t, out Ship s)
        {
            s = null;
            if (!_player.Fleet.TileIsOccupied(t)) return false;
            Tile tile;
            _player.Fleet.GetHitTileAndShip(t, out tile, out s);
            tile.IsHit = true;
            return true;
        }

        private PictureBox GetBox(Tile t)
        {
            return _boxesArray[t.Y, t.X];
        }

        private void DrawAtBox(Tile t, string imageName)
        {
            var path = Directory.GetCurrentDirectory();
            GetBox(t).Load(path + $@"\Resources\{imageName}.bmp");
        }

        private Ship _detectedShip = new Ship();
        public void Think()
        {
            bool hit = false;
            bool sunk = false;
            var count = _detectedShip.GetTiles().Count;
            var random = new Random();

            //if we have detected a tile of a ship
            if (count == 1)
            {
                var tile = _detectedShip.GetTiles()[0];
                var leftTested = tile.X == 0 || _shots.Contains(new Tile(tile.X - 1, tile.Y));
                var rightTested = tile.X == 9 || _shots.Contains(new Tile(tile.X + 1, tile.Y));
                var upTested = tile.Y == 0 || _shots.Contains(new Tile(tile.X, tile.Y - 1));
                var downTested = tile.Y == 9 || _shots.Contains(new Tile(tile.X, tile.Y + 1));

                if (leftTested && rightTested)
                    _detectedShip.Direction = Direction.Vertical;
                if (upTested && downTested)
                    _detectedShip.Direction = Direction.Horizontal;

                bool redo = true;
                do
                {
                    switch (random.Next(0, 4))
                    {
                        case 0:
                            //shoot left
                            if (!leftTested)
                            {
                                redo = !Shoot(tile.X - 1, tile.Y, out hit, out sunk);
                            }
                            break;
                        case 1:
                            //shoot right
                            if (!rightTested)
                            {
                                redo = !Shoot(tile.X + 1, tile.Y, out hit, out sunk);
                            }
                            break;
                        case 2:
                            //shoot up
                            if (!upTested)
                            {
                                redo = !Shoot(tile.X, tile.Y - 1, out hit, out sunk);
                            }
                            break;
                        case 3:
                            //shoot down
                            if (!downTested)
                            {
                                redo = !Shoot(tile.X, tile.Y + 1, out hit, out sunk);
                            }
                            break;
                        default:
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
                        if (!Shoot(tilelast.X, tilelast.Y + 1, out hit, out sunk))
                        {
                            Shoot(tilefirst.X, tilefirst.Y - 1, out hit, out sunk);
                        }                        
                    }
                    else
                    {
                        if (!Shoot(tilelast.X, tilelast.Y - 1, out hit, out sunk))
                        {
                            Shoot(tilefirst.X, tilefirst.Y + 1, out hit, out sunk);
                        }
                    }
                }
                else
                {
                    if (tilelast.X > tilefirst.X)
                    {
                        if (!Shoot(tilelast.X + 1, tilelast.Y, out hit, out sunk))
                        {
                            Shoot(tilefirst.X - 1, tilefirst.Y, out hit, out sunk);
                        }
                    }
                    else
                    {
                        if (!Shoot(tilelast.X - 1, tilelast.Y, out hit, out sunk))
                        {
                            Shoot(tilefirst.X + 1, tilefirst.Y, out hit, out sunk);
                        }                        
                    }
                }
            }

            //If it is a hit, add it to the ship
            if (hit)
                _detectedShip.AddTile(_shots.Last());

            //If the ship is sunk, we are done with this ship
            if (sunk)
                _detectedShip = new Ship();

            //We have nothing to investigate, shoot a random tile
            if (count == 0 && ShootRandom())
            {
                //If we have detected a ship, add the tile to our list
                _detectedShip.AddTile(_shots.Last());
            }
        }
    }
}
