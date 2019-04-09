using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SettlersEngine;

namespace BasketBall.AI.PathFinding
{
    public class PathNode : IPathNode<Object>, IEqualityComparer<PathNode>
    {
        public Int32 X { get; set; }
        public Int32 Y { get; set; }
        public Boolean IsWall {get; set;}

        public bool IsWalkable(Object unused)
        {
            return !IsWall;
        }

        public bool Equals(PathNode x, PathNode y)
        {
            return x.X == y.X && x.Y == y.Y;
        }

        public int GetHashCode(PathNode obj)
        {
            throw new NotImplementedException();
        }
    }
}
