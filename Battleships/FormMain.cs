using System;
using System.Windows.Forms;
using Battleships.Model;

namespace Battleships
{
    public partial class FormMain : Form
    {
        public GameState State;

        public FormMain()
        {
            InitializeComponent();
            State = GameState.Setup;
            fieldMine.MainForm = fieldEnemy.MainForm = this;
            fieldEnemy.SetFieldName("Enemy ships");
            fieldEnemy.IsAi = true;

            btnRandom.Click += (s, e) => fieldMine.PlaceShipsRandom(true);
            btnRotate.Click += (s, e) => fieldMine.Horizontal = !fieldMine.Horizontal;
            Log("Welcome to BattleShips! Place your ships to start");
        }

        public Ship GetSelectedShip(Player player)
        {
            if (rbBattleShip.Checked)
                return player.Fleet.Battleship;
            if (rbCarrier.Checked)
                return player.Fleet.Carrier;
            if (rbCruiser.Checked)
                return player.Fleet.Cruiser;
            return rbDestroyer.Checked ? player.Fleet.Destroyer : player.Fleet.Submarine;
        }

        public bool ShootAtEnemy(Tile t, out Ship outShip)
        {
            return fieldEnemy.IsShotAHit(t, out outShip);
        }

        public bool EnemyShootsAtYou(Tile t, out Ship outShip)
        {
            return fieldMine.IsShotAHit(t, out outShip);
        }

        public bool EnemyFleetDestroyed()
        {
            return fieldEnemy.FleetIsDestroyed();
        }

        public bool PlayerFleetDestroyed()
        {
            return fieldMine.FleetIsDestroyed();
        }

        public void DrawRemainingTiles()
        {
            fieldEnemy.DrawTilesNotHit();
        }

        public void CheckStart(Player player)
        {
            btnStart.Enabled = player.Fleet.AllPlaced();
        }

        public enum GameState
        {
            Setup,
            TurnPlayer,
            TurnAi,
            End
        }

        private delegate void SetStateDelegate(GameState state);

        public void SetState(GameState state)
        {
            if (btnStart.InvokeRequired)
            {
                Invoke(new SetStateDelegate(SetState), state);
                return;
            }
            State = state;
            if (State == GameState.End)
            {
                btnStart.Enabled = true;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (State == GameState.End)
            {
                gbShips.Enabled = true;
                fieldEnemy.Reset();
                fieldMine.Reset();
                lbLog.Items.Clear();
                State = GameState.Setup;
                Log("Welcome to BattleShips! Place your ships to start");
                return;
            }
            gbShips.Enabled = btnStart.Enabled = false;
            var r = new Random();
            fieldEnemy.PlaceShipsRandom(false);
            Log("Good luck, Captain!");
            if (r.NextDouble() > 0.5)
            {
                State = GameState.TurnPlayer;
                Log("You go first");
            }
            else
            {
                State = GameState.TurnAi;
                Log("The enemy goes first");
                Think();
            }
        }

        public void Think()
        {
            fieldMine.Player.Think();
        }

        private delegate void LogDelegate(string message);

        public void Log(string message)
        {
            if (lbLog.InvokeRequired)
            {
                Invoke(new LogDelegate(Log), message);
                return;
            }
            lbLog.Items.Add(message);
            lbLog.TopIndex = lbLog.Items.Count - 1;
        }
    }
}
