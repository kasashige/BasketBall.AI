using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketBall.AI.Decision_Making
{
    interface IBehavior
    {
        BehaviorTree GetBehavior();
    }
}
