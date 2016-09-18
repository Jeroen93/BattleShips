using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Battleships.Model;

namespace Battleships.Control
{
    public partial class Field : UserControl
    {
        private readonly PictureBox[,] _boxesArray;

        public readonly Player Player;
        public readonly List<Tile> Shots;
        public FormMain MainForm;
        public bool IsAi;
        public bool Horizontal { get; set; }

        public Field()
        {
            InitializeComponent();
            _boxesArray = FillArray();
            Player = new Player(this);
            Shots = new List<Tile>();
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
                    var selectedShip = MainForm.GetSelectedShip(Player);
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
                if (!Player.Fleet.ScanTileForOccupation(t)) continue;
                return false;
            }
            for (var i = rOff; i < lOff; i++)
            {
                ship.AddTile(Horizontal ? new Tile(x + i, y) : new Tile(x, y + i));
            }
            if (!draw) return true;
            Drawship(ship);
            MainForm.CheckStart(Player);
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
            foreach (var ship in Player.Fleet.AsList())
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

        public bool Shoot(int x, int y, out bool hit, out bool sunk)
        {
            hit = false;
            sunk = false;

            if (x < 0 || x > 9 || y < 0 || y > 9)
                return false;

            var tile = new Tile(x, y);
            if (Shots.Contains(tile))
                return false;
            Shots.Add(tile);

            if (IsAi)
            {
                Ship s;
                var result = MainForm.ShootAtEnemy(tile, out s) ? "Hit" : "Miss";
                DrawAtBox(new Tile(x, y), result);
                MainForm.Log($"Shooting at {tile}, It's a {result.ToLower()}");
                if (s != null && s.IsSunk())
                {
                    MainForm.Log($"You sunk the enemy's {s.Name}!");
                    if (MainForm.EnemyFleetDestroyed())
                    {
                        MainForm.Log("You destoyed the enemy's fleet!");
                        MainForm.State = FormMain.GameState.End;
                        return true;
                    }
                }

                MainForm.State = FormMain.GameState.TurnAi;
                Task.Factory.StartNew(() => MainForm.Think());
                //MainForm.Think();
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
                    if (MainForm.PlayerFleetDestroyed())
                    {
                        MainForm.Log("The enemy destroyed your fleet..");
                        MainForm.State = FormMain.GameState.End;
                        return true;
                    }
                }

                MainForm.State = FormMain.GameState.TurnPlayer;
            }
            return true;
        }

        public bool FleetIsDestroyed()
        {
            return Player.Fleet.IsDestroyed();
        }

        public bool IsShotAHit(Tile t, out Ship s)
        {
            s = null;
            if (!Player.Fleet.TileIsOccupied(t)) return false;
            Tile tile;
            Player.Fleet.GetHitTileAndShip(t, out tile, out s);
            tile.IsHit = true;
            return true;
        }

        private PictureBox GetBox(Tile t)
        {
            return _boxesArray[t.Y, t.X];
        }

        private void DrawAtBox(Tile t, string imageName)
        {
            try
            {
                GetBox(t).Load(GlobalVar.Path + $@"\Resources\{imageName}.bmp");
            }
            catch (Exception)
            {
                //The resource is still in use, wait 10ms then try again
                Thread.Sleep(10);
                DrawAtBox(t, imageName);
            }
        }
    }
}
