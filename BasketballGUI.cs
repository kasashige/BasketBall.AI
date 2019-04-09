using System.Drawing.Drawing2D;
using System.Threading;
using BasketBall.AI.Environment;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasketBall.AI
{
    public partial class BasketballGui : Form
    {
        public static Court LogicalCourt { get; set; }
        private float _columnSize;
        private float _rowSize;
        private readonly Player _p1;
        private readonly Player _p2;
        private BackgroundWorker _backgroundWorker1;
        private BackgroundWorker _backgroundWorker2;
        private int _t;
        private int _clockViolation;

        public BasketballGui()
        {
            InitializeComponent();
            LogicalCourt = new Court(5, 11);
            _p1 = new Player(LogicalCourt, new Point(LogicalCourt.Height / 2, LogicalCourt.Width - 1))
                      {
                          Name = "A1",
                          Position = new Point(LogicalCourt.Height / 2, LogicalCourt.Width / 2 - 1),
                          Speed = 350,
                          ShootingAccuracy = 0.8,
                      };
            _p2 = new Player(LogicalCourt, new Point(LogicalCourt.Height / 2, 0))
                     {
                          Name = "B1",
                          Position = new Point(LogicalCourt.Height / 2, LogicalCourt.Width / 2 + 1),
                          Speed = 1021,
                          ShootingAccuracy = 0.5
                     };

            // Set ball initial position
            LogicalCourt[LogicalCourt.Height / 2, LogicalCourt.Width / 2] = "B";
            LogicalCourt.BallPos = new Point(LogicalCourt.Height / 2, LogicalCourt.Width / 2);
            // Set players initial positions
            LogicalCourt[LogicalCourt.Height / 2, LogicalCourt.Width / 2 - 1] = "A1";
            LogicalCourt[LogicalCourt.Height / 2, LogicalCourt.Width / 2 + 1] = "B1";
        }

        private void CourtPaint(object sender, PaintEventArgs e)
        {
            var pen = new Pen(new SolidBrush(Color.White));
            var courtH = court.Height;
            var courtW = court.Width;

            _columnSize = courtW / LogicalCourt.Width + 0.5f;
            _rowSize = courtH / LogicalCourt.Height;

            // If ball in Basket (Reset)
            if (LogicalCourt.BallPos.X == _p1.ScoringBasket.X && LogicalCourt.BallPos.Y == _p1.ScoringBasket.Y
                || LogicalCourt.BallPos.X == _p2.ScoringBasket.X && LogicalCourt.BallPos.Y == _p2.ScoringBasket.Y || _clockViolation == 15)
            {
                LogicalCourt[_p1.Position.X, _p1.Position.Y] = "";
                LogicalCourt[_p2.Position.X, _p2.Position.Y] = "";
                LogicalCourt[LogicalCourt.BallPos.X, LogicalCourt.BallPos.Y] = "";
                _p1.Path.Clear();
                _p2.Path.Clear();

                if (LogicalCourt.BallPos.X == _p1.ScoringBasket.X && LogicalCourt.BallPos.Y == _p1.ScoringBasket.Y)
                {
                    LogicalCourt[LogicalCourt.Height/2, LogicalCourt.Width/2 - 1] = "A1";
                    LogicalCourt[0, LogicalCourt.Width - 1] = "B1,B";
                    LogicalCourt.BallPos = new Point(0, LogicalCourt.Width - 1);
                    _p1.Position = new Point(LogicalCourt.Height / 2, LogicalCourt.Width / 2 - 1);
                    _p1.BallPossession = false;
                    _p2.Position = new Point(0, LogicalCourt.Width - 1);
                    _p2.BallPossession = true;
                }
                else
                {
                    LogicalCourt[LogicalCourt.Height / 2, LogicalCourt.Width / 2 + 1] = "B1";
                    LogicalCourt[0, 0] = "A1,B";
                    LogicalCourt.BallPos = new Point(0,0);
                    _p1.Position = new Point(0,0);
                    _p1.BallPossession = true;
                    _p2.Position = new Point(LogicalCourt.Height / 2, LogicalCourt.Width / 2 + 1);
                    _p2.BallPossession = false;
                }
                _clockViolation = 0;
            }

            // Draw grid
            for (var i = 0; i < LogicalCourt.Width; i++)
                e.Graphics.DrawLine(pen, i * _columnSize, 0, i * _columnSize, courtH);

            for (var i = 0; i <= LogicalCourt.Height; i++)
                e.Graphics.DrawLine(pen, 0, i * _rowSize, courtW, i * _rowSize);

            // Draw Baskets
            e.Graphics.FillRectangle(new SolidBrush(Color.DarkOrange), 0, LogicalCourt.Height / 2 * _rowSize, _columnSize, _rowSize);
            e.Graphics.FillRectangle(new SolidBrush(Color.DarkOrange), (LogicalCourt.Width - 1) * _columnSize, LogicalCourt.Height / 2 * _rowSize, _columnSize, _rowSize);

            // Draw players and ball
            for (var i = 0; i < LogicalCourt.Height; i++)
                for (var j = 0; j < LogicalCourt.Width; j++)
                {
                    if (!string.IsNullOrEmpty(LogicalCourt[i,j]))
                    {
                        var elems = LogicalCourt[i, j].Split(',');
                        // Check elements on cell
                        foreach (var item in elems)
                        {
                            if (item == "B")
                                PaintEllipse(i * _rowSize + _rowSize / 2 - _rowSize / 6, j * _columnSize + _columnSize / 2 - _columnSize / 6, _columnSize / 3, _rowSize / 3, Color.White, e.Graphics);
                            else if (item.Contains("A"))
                                PaintEllipse(i*_rowSize, j*_columnSize, _columnSize, _rowSize, Color.SteelBlue,
                                             e.Graphics);
                            else
                                PaintEllipse(i * _rowSize, j * _columnSize, _columnSize, _rowSize, Color.ForestGreen, e.Graphics); 
                        }
                    }
                } 
     
            // Score
            scoreLabel.Text = string.Format("Score: {0} - {1}", LogicalCourt.ScoreTeamA, LogicalCourt.ScoreTeamB);
        }

        private void PaintEllipse(float r, float c, float w, float h, Color color, Graphics g)
        {
            g.FillEllipse(new SolidBrush(color), c, r, w, h);
        }

        private void StartClick(object sender, EventArgs e)
        {
            _t = 0;
            timer.Tick += delegate
            {
                timeLabel.Text = string.Format("Time: {0} seconds", _t++);
                _clockViolation++;
            };
            timer.Interval = 1000;
            timer.Start();

            timerA.Interval = _p1.Speed;
            timerA.Tick += delegate
                              {
                                  _backgroundWorker1 = new BackgroundWorker {WorkerSupportsCancellation = true};
                                  _backgroundWorker1.DoWork += delegate
                                                                   {
                                                                       lock (LogicalCourt)
                                                                       {
                                                                           _p1.Action(); 
                                                                       }
                                                                     
                                                                   };
                                  _backgroundWorker1.RunWorkerCompleted += OnRunWorkerCompleted;
                                  _backgroundWorker1.RunWorkerAsync();
                                 
                              };

            timerB.Interval = _p2.Speed;
            timerB.Tick += delegate
                               {
                                   _backgroundWorker2 = new BackgroundWorker { WorkerSupportsCancellation = true };
                                   _backgroundWorker2.DoWork += delegate
                                   {
                                       lock (LogicalCourt)
                                       {
                                           _p2.Action();
                                       }

                                   };
                                   _backgroundWorker2.RunWorkerCompleted += OnRunWorkerCompleted;
                                   _backgroundWorker2.RunWorkerAsync();
                               };

            timerA.Start();
            timerB.Start();
        }

        private void OnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            court.Invalidate();
        }

        private void StopClick(object sender, EventArgs e)
        {
            timerA.Stop();
            timerB.Stop();
        }
    }
}
