using System.Drawing;
using System.Linq;
using BasketBall.AI.PathFinding;

namespace BasketBall.AI.Environment
{
    public class Court
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public Point BallPos { get; set; }
        public int ScoreTeamA { get; set; }
        public int ScoreTeamB { get; set; }
        private readonly string[,] _grid;

        public Court(int h, int w)
        {
            Height = h;
            Width = w;
            _grid = new string[h,w];
        }

        public PathNode[,] ToWallGrid()
        {
            var wallGrid = new PathNode[Height, Width];
            
            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < Width; j++)
                {
                    wallGrid[i, j] = new PathNode
                    {
                        IsWall = !string.IsNullOrEmpty(_grid[i,j]),
                        X = i,
                        Y = j,
                    };
                }
            }  
            return wallGrid;
        }

        public string this[int i, int j]
        {
            get { return _grid[i, j]; }
            set 
            {
                _grid[i, j] = System.String.Format("{0}", value);
                if (IsBall(value))
                    BallPos = new Point(i, j);
            }
        }

        public bool IsEmpty(int i, int j)
        {
            return string.IsNullOrEmpty(_grid[i, j]);
        }

        private bool IsBall(string s)
        {
            return s.Split(',').Contains("B");
        }

        public override string ToString()
        {
            var result = "";

            for (var i = 0; i < _grid.GetLength(0); i++)
            {
                for (var j = 0; j < _grid.GetLength(1); j++)
                    result += _grid[i, j];
                result += '\n';
            }
            
            return result;
        }

    }
}