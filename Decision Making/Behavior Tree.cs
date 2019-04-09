using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasketBall.AI.Environment;

namespace BasketBall.AI.Decision_Making
{
    public class BehaviorTree
    {
        public List<BehaviorTree> Children { get; set; }
        public BehaviorTree Value { get; set; }
        public Court Court { get; set; }

        public BehaviorTree(Court court)
        {
            Court = court;
        }

        public virtual void Exec()
        {

        }
    }

    public class Primitive : BehaviorTree
    {
        public Primitive(Court court) : base(court)
        {
        }
    }

    public class Action: Primitive
    {
        public Action<Court> Function { get; set; }

        public Action(Court court):base(court)
        {
        }

        public override void Exec()
        {
            Function(Court);
        }
    }

    public class Conditional : Primitive
    {
        public Predicate<Court> Predicate { get; set; }

        public Conditional(Court court)
            : base(court)
        {
        }

        public override void Exec()
        {
            Predicate(Court);
        }
    }

    public class Composite : BehaviorTree
    {
        public Composite(Court court):base(court)
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

        public override void Exec()
        {
            if (Order.Count != Children.Count)
                throw new Exception("Order and children count must be the same");

            foreach (var i in Order)
                Children[i].Exec();
        }
    }

    public class Selector : Composite
    {
        public Selector(Court court)
            : base(court)
        {

        }

        public int Selection { get; set; }

        public override void Exec()
        {
            Children[Selection].Exec();
        }
    }

}
