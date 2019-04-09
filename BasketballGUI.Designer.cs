namespace BasketBall.AI
{
    partial class BasketballGui
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
            this.components = new System.ComponentModel.Container();
            this.court = new System.Windows.Forms.PictureBox();
            this.controls = new System.Windows.Forms.GroupBox();
            this.scoreLabel = new System.Windows.Forms.Label();
            this.stop = new System.Windows.Forms.Button();
            this.timeLabel = new System.Windows.Forms.Label();
            this.start = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timerA = new System.Windows.Forms.Timer(this.components);
            this.timerB = new System.Windows.Forms.Timer(this.components);
            this.timer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.court)).BeginInit();
            this.controls.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // court
            // 
            this.court.BackColor = System.Drawing.Color.OrangeRed;
            this.court.Dock = System.Windows.Forms.DockStyle.Fill;
            this.court.Location = new System.Drawing.Point(10, 10);
            this.court.Margin = new System.Windows.Forms.Padding(0);
            this.court.Name = "court";
            this.court.Size = new System.Drawing.Size(844, 408);
            this.court.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.court.TabIndex = 0;
            this.court.TabStop = false;
            this.court.Paint += new System.Windows.Forms.PaintEventHandler(this.CourtPaint);
            // 
            // controls
            // 
            this.controls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controls.Controls.Add(this.scoreLabel);
            this.controls.Controls.Add(this.stop);
            this.controls.Controls.Add(this.timeLabel);
            this.controls.Controls.Add(this.start);
            this.controls.Location = new System.Drawing.Point(12, 12);
            this.controls.Name = "controls";
            this.controls.Size = new System.Drawing.Size(864, 53);
            this.controls.TabIndex = 1;
            this.controls.TabStop = false;
            this.controls.Text = "Controls";
            // 
            // scoreLabel
            // 
            this.scoreLabel.AutoSize = true;
            this.scoreLabel.Location = new System.Drawing.Point(143, 23);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(62, 13);
            this.scoreLabel.TabIndex = 3;
            this.scoreLabel.Text = "Score: 0 - 0";
            // 
            // stop
            // 
            this.stop.Location = new System.Drawing.Point(760, 18);
            this.stop.Name = "stop";
            this.stop.Size = new System.Drawing.Size(94, 23);
            this.stop.TabIndex = 2;
            this.stop.Text = "Stop";
            this.stop.UseVisualStyleBackColor = true;
            this.stop.Click += new System.EventHandler(this.StopClick);
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(383, 24);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(85, 13);
            this.timeLabel.TabIndex = 1;
            this.timeLabel.Text = "Time: 0 seconds";
            // 
            // start
            // 
            this.start.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.start.Location = new System.Drawing.Point(660, 18);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(94, 23);
            this.start.TabIndex = 0;
            this.start.Text = "Start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.StartClick);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.DarkSlateGray;
            this.panel1.Controls.Add(this.court);
            this.panel1.Location = new System.Drawing.Point(12, 71);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(864, 428);
            this.panel1.TabIndex = 2;
            // 
            // timerA
            // 
            this.timerA.Interval = 1000;
            // 
            // BasketballGui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 511);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.controls);
            this.Name = "BasketballGui";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BasketballGUI";
            ((System.ComponentModel.ISupportInitialize)(this.court)).EndInit();
            this.controls.ResumeLayout(false);
            this.controls.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox court;
        private System.Windows.Forms.GroupBox controls;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Timer timerA;
        private System.Windows.Forms.Button stop;
        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.Timer timerB;
        private System.Windows.Forms.Timer timer;
    }
}