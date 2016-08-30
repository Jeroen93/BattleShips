namespace Battleships
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbControls = new System.Windows.Forms.GroupBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.gbShips = new System.Windows.Forms.GroupBox();
            this.btnRandom = new System.Windows.Forms.Button();
            this.rbDestroyer = new System.Windows.Forms.RadioButton();
            this.btnRotate = new System.Windows.Forms.Button();
            this.rbSubmarine = new System.Windows.Forms.RadioButton();
            this.rbCruiser = new System.Windows.Forms.RadioButton();
            this.rbCarrier = new System.Windows.Forms.RadioButton();
            this.rbBattleShip = new System.Windows.Forms.RadioButton();
            this.fieldEnemy = new Battleships.Control.Field();
            this.fieldMine = new Battleships.Control.Field();
            this.lbLog = new System.Windows.Forms.ListBox();
            this.gbControls.SuspendLayout();
            this.gbShips.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbControls
            // 
            this.gbControls.Controls.Add(this.lbLog);
            this.gbControls.Controls.Add(this.btnStart);
            this.gbControls.Controls.Add(this.gbShips);
            this.gbControls.Location = new System.Drawing.Point(12, 499);
            this.gbControls.Name = "gbControls";
            this.gbControls.Size = new System.Drawing.Size(923, 159);
            this.gbControls.TabIndex = 2;
            this.gbControls.TabStop = false;
            this.gbControls.Text = "Game Controls";
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(398, 38);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(144, 63);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start!";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // gbShips
            // 
            this.gbShips.Controls.Add(this.btnRandom);
            this.gbShips.Controls.Add(this.rbDestroyer);
            this.gbShips.Controls.Add(this.btnRotate);
            this.gbShips.Controls.Add(this.rbSubmarine);
            this.gbShips.Controls.Add(this.rbCruiser);
            this.gbShips.Controls.Add(this.rbCarrier);
            this.gbShips.Controls.Add(this.rbBattleShip);
            this.gbShips.Location = new System.Drawing.Point(6, 19);
            this.gbShips.Name = "gbShips";
            this.gbShips.Size = new System.Drawing.Size(200, 134);
            this.gbShips.TabIndex = 1;
            this.gbShips.TabStop = false;
            this.gbShips.Text = "Ships";
            // 
            // btnRandom
            // 
            this.btnRandom.Location = new System.Drawing.Point(119, 76);
            this.btnRandom.Name = "btnRandom";
            this.btnRandom.Size = new System.Drawing.Size(75, 23);
            this.btnRandom.TabIndex = 5;
            this.btnRandom.Text = "Random";
            this.btnRandom.UseVisualStyleBackColor = true;
            // 
            // rbDestroyer
            // 
            this.rbDestroyer.AutoSize = true;
            this.rbDestroyer.Location = new System.Drawing.Point(15, 111);
            this.rbDestroyer.Name = "rbDestroyer";
            this.rbDestroyer.Size = new System.Drawing.Size(70, 17);
            this.rbDestroyer.TabIndex = 4;
            this.rbDestroyer.Text = "Destroyer";
            this.rbDestroyer.UseVisualStyleBackColor = true;
            // 
            // btnRotate
            // 
            this.btnRotate.Location = new System.Drawing.Point(119, 105);
            this.btnRotate.Name = "btnRotate";
            this.btnRotate.Size = new System.Drawing.Size(75, 23);
            this.btnRotate.TabIndex = 0;
            this.btnRotate.Text = "Rotate";
            this.btnRotate.UseVisualStyleBackColor = true;
            // 
            // rbSubmarine
            // 
            this.rbSubmarine.AutoSize = true;
            this.rbSubmarine.Location = new System.Drawing.Point(15, 88);
            this.rbSubmarine.Name = "rbSubmarine";
            this.rbSubmarine.Size = new System.Drawing.Size(75, 17);
            this.rbSubmarine.TabIndex = 3;
            this.rbSubmarine.Text = "Submarine";
            this.rbSubmarine.UseVisualStyleBackColor = true;
            // 
            // rbCruiser
            // 
            this.rbCruiser.AutoSize = true;
            this.rbCruiser.Location = new System.Drawing.Point(15, 65);
            this.rbCruiser.Name = "rbCruiser";
            this.rbCruiser.Size = new System.Drawing.Size(57, 17);
            this.rbCruiser.TabIndex = 2;
            this.rbCruiser.Text = "Cruiser";
            this.rbCruiser.UseVisualStyleBackColor = true;
            // 
            // rbCarrier
            // 
            this.rbCarrier.AutoSize = true;
            this.rbCarrier.Location = new System.Drawing.Point(15, 42);
            this.rbCarrier.Name = "rbCarrier";
            this.rbCarrier.Size = new System.Drawing.Size(55, 17);
            this.rbCarrier.TabIndex = 1;
            this.rbCarrier.Text = "Carrier";
            this.rbCarrier.UseVisualStyleBackColor = true;
            // 
            // rbBattleShip
            // 
            this.rbBattleShip.AutoSize = true;
            this.rbBattleShip.Checked = true;
            this.rbBattleShip.Location = new System.Drawing.Point(15, 19);
            this.rbBattleShip.Name = "rbBattleShip";
            this.rbBattleShip.Size = new System.Drawing.Size(71, 17);
            this.rbBattleShip.TabIndex = 0;
            this.rbBattleShip.TabStop = true;
            this.rbBattleShip.Text = "Battleship";
            this.rbBattleShip.UseVisualStyleBackColor = true;
            // 
            // fieldEnemy
            // 
            this.fieldEnemy.Horizontal = true;
            this.fieldEnemy.Location = new System.Drawing.Point(491, 12);
            this.fieldEnemy.Name = "fieldEnemy";
            this.fieldEnemy.Size = new System.Drawing.Size(444, 490);
            this.fieldEnemy.TabIndex = 1;
            // 
            // fieldMine
            // 
            this.fieldMine.Horizontal = true;
            this.fieldMine.Location = new System.Drawing.Point(12, 12);
            this.fieldMine.Name = "fieldMine";
            this.fieldMine.Size = new System.Drawing.Size(444, 502);
            this.fieldMine.TabIndex = 0;
            // 
            // lbLog
            // 
            this.lbLog.FormattingEnabled = true;
            this.lbLog.Location = new System.Drawing.Point(569, 19);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(348, 134);
            this.lbLog.TabIndex = 3;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(947, 670);
            this.Controls.Add(this.gbControls);
            this.Controls.Add(this.fieldEnemy);
            this.Controls.Add(this.fieldMine);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Battleship";
            this.gbControls.ResumeLayout(false);
            this.gbShips.ResumeLayout(false);
            this.gbShips.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Control.Field fieldMine;
        private Control.Field fieldEnemy;
        private System.Windows.Forms.GroupBox gbControls;
        private System.Windows.Forms.Button btnRotate;
        private System.Windows.Forms.GroupBox gbShips;
        private System.Windows.Forms.RadioButton rbDestroyer;
        private System.Windows.Forms.RadioButton rbSubmarine;
        private System.Windows.Forms.RadioButton rbCruiser;
        private System.Windows.Forms.RadioButton rbCarrier;
        private System.Windows.Forms.RadioButton rbBattleShip;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnRandom;
        private System.Windows.Forms.ListBox lbLog;
    }
}

