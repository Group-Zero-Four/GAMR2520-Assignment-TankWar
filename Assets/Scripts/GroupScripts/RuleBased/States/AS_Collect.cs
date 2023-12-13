using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroFour.RuleBased {
    public class AS_Collect : AdvancedState
    {
        public AS_Collect(SmartAbbleTank_Base ourTank) : base(ourTank)
        {
        }

        public override void CollisionCallback(Collision collision)
        {
        }

        public override void StateEnter()
        {

        }

        public override void StateExit()
        {

        }

        public override void StateUpdate()
        {
            ourTank.MoveTankToPoint(ourTank.closestConsumable, 0.75f);
        }
    }
}