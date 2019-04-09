using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BasketBall.AI.Decision_Making;
using BasketBall.AI.PathFinding;
using SettlersEngine;
using Action = BasketBall.AI.Decision_Making.Action;

namespace BasketBall.AI.Environment
{
    public class Player
    {
        public Point Position { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public int Speed { get; set; }
        public double ShootingAccuracy { get; set; }
        public bool BallPossession { get; set; }
        public Point ScoringBasket { get; set; }
        public LinkedList<PathNode> Path; 
        private readonly Court _court;
        private readonly Random _random;

        public Player(Court court, Point basket)
        {
            ScoringBasket = new Point(basket.X, basket.Y);
            _court = court;
            Path = new LinkedList<PathNode>();
            _random = new Random();
        }

        public void Action()
        {
            var percepts = GetPercepts();

            if (percepts.Contains(Percept.CloseToBasket))
                Shoot();
            else if (percepts.Contains(Percept.BallLoose))
                MoveToBall();
            else if (percepts.Contains(Percept.BallInPossession))
                MoveToBasket();
            else if (percepts.Contains(Percept.OnDefense))
                Defend();
        }

        private IEnumerable<Percept> GetPercepts()
        {
            var result = new List<Percept>();

            if (IsCloseToBasket())
                result.Add(Percept.CloseToBasket);
            if (IsBallLoose())
                result.Add(Percept.BallLoose);
            if (IsCloseToBasket())
                result.Add(Percept.CloseToBasket);
            if (BallPossession)
                result.Add(Percept.BallInPossession);
            else
                result.Add(Percept.OnDefense);

            return result;
        }

        #region Predicates

        private bool IsBallLoose()
        {
            return _court[_court.BallPos.X, _court.BallPos.Y] == "B";
        }

        private bool IsCloseToBasket()
        {
            return Math.Abs(Position.X - ScoringBasket.X) <= 1 && Math.Abs(Position.Y - ScoringBasket.Y) <= 1;
        }

        private bool IsOpponentReachable()
        {
            var opponents = FindOpponents();
            var factor = ScoringBasket.Y == 0 ? 1 : -1;

            foreach (var opponent in opponents)
            {
                if ((Position.Y - opponent.Y) * factor >= 0)
                    return true;
            }

            return false;
        }

        #endregion

        #region Actions

        private List<Direction> FeasibleMoves()
        {
            var result = new List<Direction>();

            if (Position.Y > 0 && (_court.IsEmpty(Position.X, Position.Y - 1) || _court[Position.X, Position.Y - 1] == "B"))
                result.Add(Direction.Left);

            if (Position.Y < _court.Width - 1 && (_court.IsEmpty(Position.X, Position.Y + 1) || _court[Position.X, Position.Y + 1] == "B"))
                result.Add(Direction.Right);

            if (Position.X > 0 && (_court.IsEmpty(Position.X - 1, Position.Y) || _court[Position.X - 1, Position.Y] == "B"))
                result.Add(Direction.Up);

            if (Position.X < _court.Height - 1 && (_court.IsEmpty(Position.X + 1, Position.Y) || _court[Position.X + 1, Position.Y] == "B"))
                result.Add(Direction.Down);

            return result;
        }

        private void Move(Direction direction)
        {
            if (!FeasibleMoves().Contains(direction))
                return;

            _court[Position.X, Position.Y] = "";

            switch (direction)
            {
                    case Direction.Left:
                        Position = new Point(Position.X, Position.Y - 1);
                        break;
                    case Direction.Right:
                        Position = new Point(Position.X, Position.Y + 1);
                        break;
                    case Direction.Up:
                        Position = new Point(Position.X - 1, Position.Y);
                        break;
                    case Direction.Down:
                        Position = new Point(Position.X + 1, Position.Y);
                        break;
            }

            // To write his correct value on the grid.
            _court[Position.X, Position.Y] = (_court.BallPos.X == Position.X && _court.BallPos.Y == Position.Y) || BallPossession
                                                 ? Name + ",B"
                                                 : Name;
            
            if (_court[Position.X, Position.Y].Split(',').Contains("B"))
                BallPossession = true;
        }

        private void MoveToBall()
        {
            var ballPos = _court.BallPos;

            if (ballPos.X == Position.X)
                Move(ballPos.Y > Position.Y ? Direction.Right : Direction.Left);
            else if (ballPos.Y == Position.Y)
                Move(ballPos.X > Position.X ? Direction.Up : Direction.Down);
        }

        private void MoveToBasket()
        {
            if (Path.Count == 0)
            {
                Path = new LinkedList<PathNode>(PathFinding(Position, ScoringBasket));
                Path.RemoveFirst();
            }
            
            // Already got an strategy
            if (Path.Count > 0)
            {
                var nextMove = Path.First();
                Path.RemoveFirst();

                 // Check if move still available
                if (string.IsNullOrEmpty(_court[nextMove.X, nextMove.Y]))
                    MoveDecision(nextMove);
                else
                    Path.Clear();
             }
        }

        private void MoveDecision(PathNode nextMove)
        {
            if (nextMove.X > Position.X)
                Move(Direction.Down);
            else if (nextMove.X < Position.X)
                Move(Direction.Up);
            else if (nextMove.Y > Position.Y)
                Move(Direction.Right);
            else if (nextMove.Y < Position.Y)
                Move(Direction.Left);
        }

        private IEnumerable<PathNode> PathFinding(Point a, Point b)
        {
            var aStar = new SpatialAStar<PathNode, Object>(_court.ToWallGrid());
            return aStar.Search(a, b, null);
        }

        private IEnumerable<Point> FindOpponents()
        {
            var result = new List<Point>();
            var oppositeTeam = Name.Contains("A") ? "B" : "A";

            // Search opponents on court
            for (var i = 0; i < _court.Height; i++) 
            {
                for (var j = 0; j < _court.Width; j++)
                    if (!string.IsNullOrEmpty(_court[i, j]) && _court[i, j].Contains(oppositeTeam))
                        result.Add(new Point(i, j));
            }

            return result;
        }

        private void Shoot()
        {
            // Shot made
            if (_random.NextDouble() <= ShootingAccuracy)
            {
                if (Name.Contains("A"))
                    _court.ScoreTeamA++;
                else
                    _court.ScoreTeamB++;
                BallPossession = false;
                _court[Position.X, Position.Y] = Name;
                _court[ScoringBasket.X, ScoringBasket.Y] = "B";
                _court.BallPos = ScoringBasket;
            }
        }

        private bool BlockPath()
        {
            var closestOppPos = Closest(FindOpponents());
            
            // Move to same row
            if (closestOppPos.X > Position.X)
                Move(Direction.Down);
            else if (closestOppPos.X < Position.X)
                Move(Direction.Up);
            // Move to same column
            else if (closestOppPos.Y > Position.Y)
                Move(Direction.Right);
            else if (closestOppPos.Y < Position.Y)
                Move(Direction.Left);

            return true;
        }

        private Point Closest(IEnumerable<Point> opponents)
        {
            var result = new Point();
            var minimumDist = double.MaxValue;

            foreach (var opponent in opponents)
            {
                var currentDist = ManhattanDistance(Position, opponent);
                if (currentDist < minimumDist)
                {
                    minimumDist = currentDist;
                    result.X = opponent.X;
                    result.Y = opponent.Y;
                }
            }

            return result;
        }

        private int ManhattanDistance(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        private void Defend()
        {
            DefensiveBehavior(_court).Exec();
        }

        private BehaviorTree DefensiveBehavior(Court court)
        {
            var isReachableNode = new Conditional(court)
                                      {
                                          Predicate = IsOpponentReachable
                                      };

            var blockNode = new Action(court)
                                      {
                                          Function = BlockPath
                                      };

            var defenseBehavior = new Sequence(court)
                                      {
                                          Order = new List<int> {0,1},
                                          Children = new List<BehaviorTree>
                                                         {
                                                             isReachableNode,
                                                             blockNode
                                                         }
                                      };

            return defenseBehavior;
        }

        #endregion
    }

    public enum Percept
    {
        Guarded, 
        CloseToBasket,
        BallLoose,
        BallInPossession,
        OnDefense,
        None
    }

    public enum Direction
    {
        Up, Down, Left, Right
    }
}
