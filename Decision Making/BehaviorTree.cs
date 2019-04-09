using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasketBall.AI.Environment;

namespace BasketBall.AI.Decision_Making
{
    public abstract class BehaviorTree
    {
        public List<BehaviorTree> Children { get; set; }
        public BehaviorTree Value { get; set; }
        public Court Court { get; set; }

        protected BehaviorTree(Court court)
        {
            Court = court;
        }

        public abstract bool Exec();
    }

    public abstract class Primitive : BehaviorTree
    {
        protected Primitive(Court court) : base(court)
        {
        }
    }

    public class Action: Primitive
    {
        public delegate bool Act();
        public Act Function { get; set; }

        public Action(Court court):base(court)
        {
        }

        public override bool Exec()
        {
            return Function();
        }
    }

    public class Conditional : Primitive
    {
        public delegate bool Pred();
        public Pred Predicate { get; set; }

        public Conditional(Court court)
            : base(court)
        {
        }

        public override bool Exec()
        {
            return Predicate();
        }
    }

    public abstract class Composite : BehaviorTree
    {
        protected Composite(Court court):base(court)
        {
        }
    }

    public class Sequence: Composite
    {
        public Sequence(Court court)
            : base(court)
        {
            
        }

        public List<int> Order { get; set; }

        public override bool Exec()
        {
            if (Order.Count != Children.Count)
                throw new Exception("Order and children count must be the same");

            foreach (var i in Order)
            {
                if (!Children[i].Exec())
                    return false;
            }
                
            return true;
        }
    }

    public class Selector : Composite
    {
        public Selector(Court court)
            : base(court)
        {

        }

        public int Selection { get; set; }

        public override bool Exec()
        {
            return Children[Selection].Exec();
        }
    }

}
